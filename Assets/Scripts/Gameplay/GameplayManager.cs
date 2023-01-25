using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public struct Score
{
    public bool IsValid;
    public int Ticks;
    public int Deaths;
}

public struct HighScores
{
    public Score AnyPercentScore;
    public Score HundredPercentScore;
}

public class GameplayManager : MonoBehaviour
{
    public enum GameState
    {
        Countdown,
        Playing,
        Paused,
        Finished,
    }

    public int CurrentTime { get; private set; } = 0;
    public int[] Watermelons => _watermelons.ToArray();
    public int TotalWatermelons { get; private set; } = 0;
    public int Deaths { get; private set; } = 0;
    public GameState State { get => _state; private set { OnStateChanged(_state, value); _state = value; } }
    private GameState _state = GameState.Countdown;
    public bool IsEasyMode { get; private set; } = false;

    public static bool Pepega { get; private set; } = false;

    [SerializeField] private GameObject _player = null;
    [SerializeField] private GameObject _pauseMenu = null;
    [SerializeField] private FollowGameObject _followCamera = null;
    [SerializeField] private PlayerControls _playerControls = null;
    [SerializeField] private TMP_Text _startText = null;
    [SerializeField] private ResultsScreen _resultsScreen = null;

    [SerializeField] private AudioSource _timerTickSounds = null;
    [SerializeField] private AudioSource _gameStartSounds = null;

    private string _saveId => IsEasyMode ? "pdr_highscores_ez" : "pdr_highscores";
    private string _kekId => IsEasyMode ? "kek_ez" : "kek";

    private readonly List<int> _watermelons = new();

    public static string TimeToString(int ticks)
    {
        var hundredths = ticks % 100;
        var seconds = (ticks / 100) % 60;
        var minutes = ticks / 100 / 60;

        return $"{minutes:00}:{seconds:00}.{hundredths:00}";
    }

    private void Awake()
    {
        var gameSettings = FindObjectOfType<GameSettings>();
        if (gameSettings != null)
        {
            IsEasyMode = gameSettings.IsEasyMode;
        }
    }

    private void Start()
    {
        TotalWatermelons = FindObjectsOfType<PickupItem>().Where(x => x.ItemType == PickupItem.PickupItemType.Watermelon).Count();

        ReadScores();
        State = GameState.Countdown;

        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        for (int i = 3;i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            var soundEffect = i == 0 ? _gameStartSounds : _timerTickSounds;
            if (soundEffect != null && soundEffect.enabled)
            {
                soundEffect.Play();
            }
            _startText.text = i == 0 ? "" : i.ToString();
        }

        State = GameState.Playing;
    }

    private void OnStateChanged(GameState oldState, GameState newState)
    {
        switch ((oldState, newState))
        {
            case (_, GameState.Countdown):
                _playerControls.SetPaused(true);
                _pauseMenu.SetActive(false);
                _player.SetActive(false);
                Time.timeScale = 1.0f;
                break;

            case (_, GameState.Playing):
                _playerControls.SetPaused(false);
                _pauseMenu.SetActive(false);
                _player.SetActive(true);
                Time.timeScale = 1.0f;
                break;

            case (_, GameState.Paused):
                _pauseMenu.SetActive(true);
                _playerControls.SetPaused(true);
                Time.timeScale = 0.0f;
                break;

            case (_, GameState.Finished):
                _playerControls.SetPaused(true);
                _followCamera.enabled = false;
                break;
        }
    }

    public void PickupWatermelon(int id)
    {
        if (!_watermelons.Contains(id) && State == GameState.Playing)
        {
            _watermelons.Add(id);
        }
    }

    public void Died()
    {
        if (State == GameState.Playing)
        {
            Deaths++;
        }
    }

    public void Win()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        State = GameState.Finished;

        _resultsScreen.gameObject.SetActive(true);

        var prevScores = ReadScores();

        var isHundo = Enumerable.Range(1, TotalWatermelons).Sum() == _watermelons.Sum();
        var newAnyPercentPB = !prevScores.AnyPercentScore.IsValid || CurrentTime < prevScores.AnyPercentScore.Ticks || (prevScores.AnyPercentScore.Ticks == CurrentTime && prevScores.AnyPercentScore.Deaths < Deaths);
        var newHundredPercentPB = isHundo && (!prevScores.HundredPercentScore.IsValid || CurrentTime < prevScores.HundredPercentScore.Ticks || (prevScores.HundredPercentScore.Ticks == CurrentTime && prevScores.HundredPercentScore.Deaths < Deaths));

        var newScores = new HighScores()
        {
            AnyPercentScore = new Score()
            {
                IsValid = true,
                Ticks = newAnyPercentPB ? CurrentTime : prevScores.AnyPercentScore.Ticks,
                Deaths = newAnyPercentPB ? Deaths : prevScores.AnyPercentScore.Deaths,
            },
            HundredPercentScore = prevScores.HundredPercentScore.IsValid || isHundo ? new Score()
            {
                IsValid = true,
                Ticks = newHundredPercentPB ? CurrentTime : prevScores.HundredPercentScore.Ticks,
                Deaths = newHundredPercentPB ? Deaths : prevScores.HundredPercentScore.Deaths,
            } : new Score(),
        };

        SaveScores(newScores);

        _resultsScreen.ShowResults(CurrentTime, Deaths, _watermelons.ToArray(), TotalWatermelons, newScores, newAnyPercentPB, newHundredPercentPB);
    }

    public void TogglePause()
    {
        switch (State)
        {
            case GameState.Paused:
                State = GameState.Playing;
                break;

            case GameState.Playing:
                State = GameState.Paused;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (State == GameState.Playing)
        {
            CurrentTime++;
        }
    }

    private void Update()
    {
        if ((Input.GetButtonDown("Pause") || (Input.GetButtonDown("Cancel") && State == GameState.Paused)) && (State == GameState.Playing || State == GameState.Paused))
        {
            TogglePause();
        }
    }

    private static string ComputeHash(string data)
    {
        return string.Concat(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(data)).Select(x => x ^ 0x57)
            .Select(x => x.ToString("X2")));
    }

    public HighScores ReadScores()
    {
        var scoresJson = PlayerPrefs.GetString(_saveId);
        var hash = PlayerPrefs.GetString(_kekId);

        if (string.IsNullOrEmpty(scoresJson))
        {
            return new HighScores();
        }

        if (ComputeHash(scoresJson) != hash)
        {
            Pepega = true;
            return new HighScores();
        }

        try
        {
            return JsonConvert.DeserializeObject<HighScores>(scoresJson);
        }
        catch
        {
            return new HighScores();
        }
    }

    public void SaveScores(HighScores scores)
    {
        var scoresJson = JsonConvert.SerializeObject(scores);
        PlayerPrefs.SetString(_saveId, scoresJson);
        PlayerPrefs.SetString(_kekId, ComputeHash(scoresJson));
        PlayerPrefs.Save();
    }

    public void ClearScores()
    {
        PlayerPrefs.DeleteKey(_saveId);
        PlayerPrefs.DeleteKey(_kekId);
        PlayerPrefs.Save();
    }
}
