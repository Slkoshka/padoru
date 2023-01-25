using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameSetting
{
    Music,
    SoundEffects,
    EasyMode,
}

public class GameSettings : MonoBehaviour
{
    class SerializedSettings
    {
        public bool IsMusicEnabled;
        public bool AreSoundEffectsEnabled;
    }

    public event EventHandler OnSettingsSaved;

    public bool IsEasyMode = false;
    public bool IsMusicEnabled = true;
    public bool AreSoundEffectsEnabled = true;

    public const string SETTINGS_ID = "game_settings";

    private void Awake()
    {
        DontDestroyOnLoad(this);
        var settingsJson = PlayerPrefs.GetString(SETTINGS_ID);
        if (!string.IsNullOrEmpty(settingsJson))
        {
            var settings = JsonConvert.DeserializeObject<SerializedSettings>(settingsJson);
            if (settings is not null)
            {
                IsMusicEnabled = settings.IsMusicEnabled;
                AreSoundEffectsEnabled = settings.AreSoundEffectsEnabled;
            }
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(SETTINGS_ID, JsonConvert.SerializeObject(new SerializedSettings()
        {
            IsMusicEnabled = IsMusicEnabled,
            AreSoundEffectsEnabled = AreSoundEffectsEnabled,
        }));
        PlayerPrefs.Save();
        OnSettingsSaved?.Invoke(this, EventArgs.Empty);
    }
}
