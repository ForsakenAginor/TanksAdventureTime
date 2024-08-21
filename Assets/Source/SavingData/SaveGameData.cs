using Agava.YandexGames;
using System;
using UnityEngine;

public class SaveGameData
{
    private GameData _gameData;

    public event Action<GameData> Loaded;

    public void Save(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);

#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.SetCloudSaveData(json);
#elif UNITY_EDITOR
        PlayerPrefs.SetString(nameof(_gameData), json);
#endif
    }

    public void Load()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.GetCloudSaveData(Load);
#elif UNITY_EDITOR

        if(PlayerPrefs.HasKey(nameof(_gameData)))
        {
            Load(PlayerPrefs.GetString(nameof(_gameData)));
            return;
        }

        _gameData = new ();
        string json = JsonUtility.ToJson(_gameData);
        Load(json);
#endif
    }

    private void Load(string value)
    {
        _gameData = JsonUtility.FromJson<GameData>(value);
        Loaded?.Invoke(_gameData);
    }
}