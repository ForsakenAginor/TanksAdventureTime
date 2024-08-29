using System;
using Shops;
using UnityEngine;

namespace SavingData
{
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

        private void OnEnable() => _saveGameData.Loaded += OnFill;

        private void OnDisable() => _saveGameData.Loaded -= OnFill;


        public int GetCurrency()
        {
            return Currency;
        }

        public void SaveCurrencyData(int amount)
        {
            _gameData.Currency = amount;
            Save();
        }

        public void SaveLevelData(int level)
        {
            _gameData.Level = level;
            Save();
        }

        public void SaveCompletedTrainingComputerData(bool isCompletedTraining)
        {
            _gameData.CompletedTrainingOnComputer = GetCompletedTrainingValue(isCompletedTraining);
            Save();
        }

        public void SaveCompletedTrainingMobileData(bool isCompletedTraining)
        {
            _gameData.CompletedTrainingOnMobile = GetCompletedTrainingValue(isCompletedTraining);
            Save();
        }

        public void SavePlayerHelperData(int indexHelper)
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

        private void OnFill(GameData gameData)
        {
            _gameData = gameData;
            Loaded?.Invoke();
        }
    }
}