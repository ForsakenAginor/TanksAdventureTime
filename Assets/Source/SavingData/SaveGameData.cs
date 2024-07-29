using Agava.YandexGames;
using System;
using UnityEngine;

public class SaveGameData
{
    private GameData _gameData;

    public event Action Loaded;

    public void Save(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        Debug.Log(json);
        PlayerAccount.SetCloudSaveData(json);
        //PlayerPrefs.SetString("data", json);
    }

    public GameData Load()
    {
        PlayerAccount.GetCloudSaveData(Load);
        //Load(PlayerPrefs.GetString("data"));
        return _gameData;
    }

    private void Load(string value)
    {
        _gameData = JsonUtility.FromJson<GameData>(value);
        Loaded?.Invoke();
    }
}