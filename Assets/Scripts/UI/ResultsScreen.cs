using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _winTime = null;
    [SerializeField] private TMP_Text _deathsCounter = null;
    [SerializeField] private Transform _watermelonsContainer = null;
    [SerializeField] private GameObject _watermelonPickedUpPrefab = null;
    [SerializeField] private GameObject _watermelonMissedPrefab = null;
    [SerializeField] private Color _newPBColor = Color.white;
    [SerializeField] private TMP_Text _anyPercentPB = null;
    [SerializeField] private TMP_Text _anyPercentPBZeroDeaths = null;
    [SerializeField] private TMP_Text _hundredPercentPB = null;
    [SerializeField] private TMP_Text _hundredPercentPBZeroDeaths = null;

    public void ShowResults(int ticks, int deaths, int[] watermelons, int totalWatermelons, HighScores results, bool newAnyPercentPB, bool newHundredPercentPB)
    {
        _winTime.text = GameplayManager.TimeToString(ticks);
        _deathsCounter.text = $"{deaths}";

        for (int i = 1; i <= totalWatermelons; i++)
        {
            var prefab = watermelons.Contains(i) ? _watermelonPickedUpPrefab : _watermelonMissedPrefab;
            Instantiate(prefab, _watermelonsContainer);
        }

        _anyPercentPB.text = results.AnyPercentScore.IsValid ? GameplayManager.TimeToString(results.AnyPercentScore.Ticks) : "--:--.--";
        _anyPercentPBZeroDeaths.gameObject.SetActive(results.AnyPercentScore.IsValid && results.AnyPercentScore.Deaths == 0);
        if (newAnyPercentPB)
        {
            _anyPercentPB.color = _newPBColor;
        }

        _hundredPercentPB.text = results.HundredPercentScore.IsValid ? GameplayManager.TimeToString(results.HundredPercentScore.Ticks) : "--:--.--";
        _hundredPercentPBZeroDeaths.gameObject.SetActive(results.HundredPercentScore.IsValid && results.HundredPercentScore.Deaths == 0);
        if (newHundredPercentPB)
        {
            _hundredPercentPB.color = _newPBColor;
        }
    }
}
