using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleSettingAction : MonoBehaviour
{
    public enum TogglableSetting
    {
        None,
        Music,
        SoundEffects,
    }

    [SerializeField] private TogglableSetting _setting = TogglableSetting.None;
    [SerializeField] private TMP_Text _valueLabel = null;
    private GameSettings _gameSettings = null;
    private bool _fakeValue = false;

    private void Awake()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    private ref bool GetValue()
    {
        if (_gameSettings == null)
        {
            return ref _fakeValue;
        }
        switch (_setting)
        {
            case TogglableSetting.Music:
                return ref _gameSettings.IsMusicEnabled;
            case TogglableSetting.SoundEffects:
                return ref _gameSettings.AreSoundEffectsEnabled;
            default:
                return ref _fakeValue;
        }
    }

    private void RefreshValue()
    {
        if (_valueLabel != null)
        {
            _valueLabel.text = GetValue() ? "Enabled" : "Disabled";
        }
    }

    public void OnEnable()
    {
        RefreshValue();
    }

    public void Trigger()
    {
        GetValue() = !GetValue();
        if (_gameSettings != null)
        {
            _gameSettings.Save();
        }
        RefreshValue();
    }
}
