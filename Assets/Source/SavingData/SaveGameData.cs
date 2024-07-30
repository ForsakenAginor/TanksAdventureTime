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
        Debug.Log(json);
        //PlayerAccount.SetCloudSaveData(json);
        PlayerPrefs.SetString("data", json);
    }

    public void Load()
    {
        // PlayerAccount.GetCloudSaveData(Load);
        Load(PlayerPrefs.GetString("data"));
    }

    private void Load(string value)
    {
        _gameData = JsonUtility.FromJson<GameData>(value);
        Loaded?.Invoke(_gameData);
    }
}