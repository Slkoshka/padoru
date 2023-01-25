using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAudioFromSettings : MonoBehaviour
{
    [SerializeField] private GameSetting _type;
    [SerializeField] private AudioSource _source;
    [SerializeField] private bool _disableIfSettingIsTrue = false;
    private GameSettings _gameSettings = null;

    public void Awake()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    private void OnEnable()
    {
        if (_gameSettings != null)
        {
            _gameSettings.OnSettingsSaved += OnSettingsSaved;
        }
        ApplySettings();
    }

    private void OnDisable()
    {
        if (_gameSettings != null)
        {
            _gameSettings.OnSettingsSaved -= OnSettingsSaved;
        }
    }

    private void OnSettingsSaved(object sender, EventArgs args)
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (_source != null && _gameSettings != null)
        {
            _source.enabled = _type switch
            {
                GameSetting.Music => _disableIfSettingIsTrue ^ _gameSettings.IsMusicEnabled,
                GameSetting.SoundEffects => _disableIfSettingIsTrue ^ _gameSettings.AreSoundEffectsEnabled,
                GameSetting.EasyMode => _disableIfSettingIsTrue ^ _gameSettings.IsEasyMode,
                _ => _source.enabled,
            };
        }
    }
}
