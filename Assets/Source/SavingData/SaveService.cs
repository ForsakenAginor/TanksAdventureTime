using Shops;
using System;
using UnityEngine;

public class SaveService : MonoBehaviour, ISave
{
    private SaveGameData _saveGameData = new ();
    private GameData _gameData = new ();

    public event Action Loaded;

    public int Level => _gameData.Level;

    public int Currency => _gameData.Currency;

    public int Helper => _gameData.Helper;

    public int CompletedTraining => _gameData.CompletedTraining;

    private void Start() => _saveGameData.Load();
    
    private void OnEnable() => _saveGameData.Loaded += Fill;

    private void OnDisable() => _saveGameData.Loaded -= Fill;


    public int GetCurrency()
    {
        return Currency;
    }

    public void SetCurrencyData(int amount)
    {
        _gameData.Currency = amount;
        Save();
    }

    public void SetLevelData(int level)
    {
        _gameData.Level = level;
        Save();
    }

    public void SetCompletedTrainingData(bool isCompletedTraining)
    {
        _gameData.CompletedTraining = isCompletedTraining ? 1 : 0;
        Save();
    }

    public void SetPlayerHelperData(int indexHelper)
    {
        _gameData.Helper = indexHelper;
        Save();
    }

    public int GetPlayerHelperData()
    {
        return _gameData.Helper;
    }

    public void SavePurchasesData(IReadOnlyCharacteristics purchases)
    {
        _gameData.Purchases = purchases as Purchases;
        Save();
    }

    public IReadOnlyCharacteristics GetPurchasesData()
    {
        return _gameData.Purchases;
    }

    private void Save()
    {
        _saveGameData.Save(_gameData);
    }

    private void Fill(GameData gameData)
    {
        _gameData = gameData;

        if (((Purchases)_gameData.Purchases).Objects.Count != 0)
        {
            Debug.Log($"Key {((Purchases)_gameData.Purchases).Objects[0].Key}, Value {((Purchases)_gameData.Purchases).Objects[0].Value}");
        }

        Loaded?.Invoke();
    }
}