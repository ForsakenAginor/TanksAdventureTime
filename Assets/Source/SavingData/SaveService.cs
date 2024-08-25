using Shops;
using System;
using UnityEngine;

public class SaveService : MonoBehaviour, ISave
{
    private SaveGameData _saveGameData = new();
    private GameData _gameData = new();

    public event Action Loaded;

    public int Level => _gameData.Level;

    public int Currency => _gameData.Currency;

    public int Helper => _gameData.Helper;

    public bool HadHelper => _gameData.HadHelper;

    public int CompletedTrainingOnComputer => _gameData.CompletedTrainingOnComputer;

    public int CompletedTrainingOnMobile => _gameData.CompletedTrainingOnMobile;

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

    public void SetCompletedTrainingComputerData(bool isCompletedTraining)
    {
        _gameData.CompletedTrainingOnComputer = GetCompletedTrainingValue(isCompletedTraining);
        Save();
    }

    public void SetCompletedTrainingMobileData(bool isCompletedTraining)
    {
        _gameData.CompletedTrainingOnComputer = GetCompletedTrainingValue(isCompletedTraining);
        Save();
    }

    public void SetPlayerHelperData(int indexHelper)
    {
        _gameData.Helper = indexHelper;
        _gameData.HadHelper = true;
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

    private int GetCompletedTrainingValue(bool isCompletedTraining)
    {
        int minValue = 0;
        int maxValue = 1;
        return isCompletedTraining ? maxValue : minValue;
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