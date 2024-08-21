using Shops;
using System;
using UnityEngine;

public class SaveService : MonoBehaviour, ISave
{
    private SaveGameData _saveGameData = new();
    private GameData _gameData = new();

    public event Action Loaded;

    public int Level { get; private set; }

    public int Currency { get; private set; }

    public int Helper { get; private set; }

    public int CompletedTraining { get; private set; }

    private void Start() => _saveGameData.Load();
    
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

    public void SavePurchasesData(IReadOnlyCharacteristics purchases)
    {
        _gameData.Purchases = purchases;
    }

    public IReadOnlyCharacteristics GetPurchasesData()
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

        if (((Purchases)_gameData.Purchases).Objects.Count != 0)
        {
            Debug.Log($"Key {((Purchases)_gameData.Purchases).Objects[0].Key}, Value {((Purchases)_gameData.Purchases).Objects[0].Value}");
        }

        Loaded?.Invoke();
    }

    public int GetCurrency()
    {
        return Currency;
    }

    public void Save(int currency)
    {
        _gameData.Currency = currency;
        _saveGameData.Save(_gameData);
    }
}