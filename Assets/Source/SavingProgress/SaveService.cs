using System;
using Shops;
using UnityEngine;

namespace SavingProgress
{
    public class SaveService : MonoBehaviour, ISave
    {
        private readonly SaveGameProgress _saveGameProgress = new();
        private GameProgress _gameProgress = new();

        public event Action Loaded;

        public int Level => _gameProgress.Level;

        public int Helper => _gameProgress.Helper;

        public bool HadHelper => _gameProgress.HadHelper;

        public int CompletedTrainingOnComputer => _gameProgress.CompletedTrainingOnComputer;

        public int CompletedTrainingOnMobile => _gameProgress.CompletedTrainingOnMobile;

        private void Start() => _saveGameProgress.Load();

        private void OnEnable() => _saveGameProgress.Loaded += OnFill;

        private void OnDisable() => _saveGameProgress.Loaded -= OnFill;

        public int GetCurrency()
        {
            throw new NotImplementedException();
        }

        public void SaveCurrency(int amount)
        {
            _gameProgress.Currency = amount;
            Save();
        }

        public void SaveLevel(int level)
        {
            _gameProgress.Level = level;
            Save();
        }

        public void SaveCompletedTrainingComputer(bool isCompletedTraining)
        {
            _gameProgress.CompletedTrainingOnComputer = GetCompletedTrainingValue(isCompletedTraining);
            Save();
        }

        public void SaveCompletedTrainingMobile(bool isCompletedTraining)
        {
            _gameProgress.CompletedTrainingOnMobile = GetCompletedTrainingValue(isCompletedTraining);
            Save();
        }

        public void SavePlayerHelper(int indexHelper)
        {
            _gameProgress.Helper = indexHelper;
            _gameProgress.HadHelper = true;
            Save();
        }

        public void SavePurchases(IReadOnlyCharacteristics purchases)
        {
            _gameProgress.Purchases = purchases as Purchases;
            Save();
        }

        public IReadOnlyCharacteristics GetPurchases()
        {
            return _gameProgress.Purchases;
        }

        private void Save()
        {
            _saveGameProgress.Save(_gameProgress);
        }

        private int GetCompletedTrainingValue(bool isCompletedTraining)
        {
            int minValue = 0;
            int maxValue = 1;
            return isCompletedTraining ? maxValue : minValue;
        }

        private void OnFill(GameProgress gameData)
        {
            _gameProgress = gameData;
            Loaded?.Invoke();
        }
    }
}