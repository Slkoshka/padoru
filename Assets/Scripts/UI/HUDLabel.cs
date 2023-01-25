using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HUDLabel : MonoBehaviour
{
    public enum HUDText
    {
        None,
        Timer,
        Watermelons,
        Deaths,
    }

    [SerializeField] private HUDText _hudText = HUDText.None;

    private GameplayManager _gameplayManager = null;
    private TMP_Text _textComponent = null;

    private void Start()
    {
        _gameplayManager = FindObjectOfType<GameplayManager>();
        _textComponent = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (_gameplayManager == null || _textComponent == null)
        {
            return;
        }

        _textComponent.text = _hudText switch
        {
            HUDText.Timer => GameplayManager.TimeToString(_gameplayManager.CurrentTime),
            HUDText.Watermelons => $"{_gameplayManager.Watermelons.Length} / {_gameplayManager.TotalWatermelons}",
            HUDText.Deaths => $"{_gameplayManager.Deaths}",
            _ => _textComponent.text,
        };
    }
}
