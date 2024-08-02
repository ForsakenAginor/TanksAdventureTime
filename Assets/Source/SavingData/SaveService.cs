using Shops;
using System;
using UnityEngine;

public class SaveService : MonoBehaviour
{
    private SaveGameData _saveGameData = new();
    private GameData _gameData = new();

    public event Action Loaded;

    public int Level { get; private set; }

    public int Currency { get; private set; }

    public int Helper { get; private set; }

    public int CompletedTraining { get; private set; }

    private void Awake() => _saveGameData.Load();

    private void OnEnable() => _saveGameData.Loaded += Fill;

    private void OnDisable() => _saveGameData.Loaded -= Fill;

    public void Save()
    {
        _saveGameData.Save(_gameData);
    }

    public void SetCurrencyData(int currency)
    {
        _gameData.Currency = currency;
    }

    public void SetLevelData(int level)
    {
        _gameData.Level = level;
    }

    public void SetCompletedTrainingData(bool isCompletedTraining)
    {
        _gameData.CompletedTraining = isCompletedTraining ? 1 : 0;
    }

    public void SetPlayerHelperData(int indexHelper)
    {
        _gameData.Helper = indexHelper;
    }

    public int GetPlayerHelperData()
    {
        return _gameData.Helper;
    }

    public void SavePurchasesData(Purchases<int> purchases)
    {
        _gameData.Purchases = purchases;
    }

    public Purchases<int> GetPurchasesData()
    {
        return _gameData.Purchases;
    }

    private void Fill(GameData gameData)
    {
        _gameData = gameData;
        Level = _gameData.Level;
        Currency = _gameData.Currency;
        Helper = _gameData.Helper;
        CompletedTraining = _gameData.CompletedTraining;
        Debug.Log($"Fill {Level} {Currency} {Helper} {CompletedTraining}");

        if (_gameData.Purchases.Objects.Count != 0)
        {
            Debug.Log($"Key {_gameData.Purchases.Objects[0].Key}, Value {_gameData.Purchases.Objects[0].Value}");
        }

        Loaded?.Invoke();
    }
}