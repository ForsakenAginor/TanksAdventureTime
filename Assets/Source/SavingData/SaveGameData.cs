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
        PlayerAccount.SetCloudSaveData(json);
    }

    public void Load()
    {
        PlayerAccount.GetCloudSaveData(Load);
    }

    private void Load(string value)
    {
        _gameData = JsonUtility.FromJson<GameData>(value);
        Loaded?.Invoke(_gameData);
    }
}