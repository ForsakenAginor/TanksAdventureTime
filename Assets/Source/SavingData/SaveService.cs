using UnityEngine;

public class SaveService : MonoBehaviour
{
    private SaveGameData _saveGameData = new();
    private GameData _gameData = new();

    public SaveGameData SaveGameData => _saveGameData;

    private string[] _products;

    public int Level { get; private set; }

    public int Coins { get; private set; }

    public int Helper { get; private set; }

    public int CompletedTraining { get; private set; }

    private void Awake()
    {
        LoadProgresss();
        Level = _gameData.Level;
        Coins = _gameData.Coins;
        Helper = _gameData.Helper;
        CompletedTraining = _gameData.CompletedTraining;
        _products = new string[] { "Cot", "Popa", "Dada" };
        Debug.Log($"{Level} {Coins} {Helper} {CompletedTraining}");
        Debug.Log($" {_gameData.MasterVolume} {_gameData.MusicVolume} {_gameData.EffectVolume}");
    }
    public void LoadProgresss()
    {
        _gameData = _saveGameData.Load();
    }

    public void SaveLevel(int level)
    {
        _gameData.Level = level;
        _saveGameData.Save(_gameData);
    }

    public void SaveCompletedTraining(bool isCompletedTraining)
    {
        _gameData.CompletedTraining = isCompletedTraining ? 1 : 0;
        _saveGameData.Save(_gameData);
    }

    public void SavePlayerHelper(int indexHelper)
    {
        _gameData.Helper = indexHelper;
        _saveGameData.Save(_gameData);
    }

    public int GetPlayerHelper()
    {
        _gameData = _saveGameData.Load();
        return _gameData.Helper;
    }

    public void SavingSoundSettings(SoundSettingsData soundSettingsData)
    {
        _gameData.EffectVolume = soundSettingsData.EffectVolume;
        _gameData.MusicVolume = soundSettingsData.MusicVolume;
        _gameData.MasterVolume = soundSettingsData.MasterVolume;
        _saveGameData.Save(_gameData);
    }

    public SoundSettingsData LoadSoundSettingData()
    {
        _gameData = _saveGameData.Load();
        return new SoundSettingsData()
        {
            EffectVolume = _gameData.EffectVolume,
            MasterVolume = _gameData.MasterVolume,
            MusicVolume = _gameData.MusicVolume
        };
    }
}