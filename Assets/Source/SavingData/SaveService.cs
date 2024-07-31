using System;
using UnityEngine;

public class SaveService : MonoBehaviour
{
    private SaveGameData _saveGameData = new();
    private GameData _gameData = new();

    public event Action Loaded;

    private string[] _products;

    public int Level { get; private set; }

    public int Coins { get; private set; }

    public int Helper { get; private set; }

    public int CompletedTraining { get; private set; }

    private void Awake() => _saveGameData.Load();

    private void OnEnable() => _saveGameData.Loaded += Fill;

    private void OnDisable() => _saveGameData.Loaded -= Fill;

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
        return _gameData.Helper;
    }

    public void SaveProducts(string[] products)
    {
        _products = products;
        _saveGameData.Save(_gameData);
    }

    private void Fill(GameData gameData)
    {
        _gameData = gameData;
        Level = _gameData.Level;
        Coins = _gameData.Coins;
        Helper = _gameData.Helper;
        CompletedTraining = _gameData.CompletedTraining;
        Debug.Log($"Fill {Level} {Coins} {Helper} {CompletedTraining}");
        Loaded?.Invoke();
    }
}