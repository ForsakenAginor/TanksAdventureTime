using System;
using Agava.YandexGames;
using UnityEngine;

namespace SavingProgress
{
    public class SaveGameProgress
    {
        private GameProgress _gameProgress;

        public event Action<GameProgress> Loaded;

        public void Save(GameProgress gameProgress)
        {
            string json = JsonUtility.ToJson(gameProgress);

#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.SetCloudSaveData(json);
#elif UNITY_EDITOR
            PlayerPrefs.SetString(nameof(_gameProgress), json);
            PlayerPrefs.Save();
#endif
        }

        public void Load()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.GetCloudSaveData(Load);
#elif UNITY_EDITOR

            if (PlayerPrefs.HasKey(nameof(_gameProgress)))
            {
                Load(PlayerPrefs.GetString(nameof(_gameProgress)));
                return;
            }

            _gameProgress = new ();
            string json = JsonUtility.ToJson(_gameProgress);
            Load(json);
#endif
        }

        private void Load(string value)
        {
            _gameProgress = JsonUtility.FromJson<GameProgress>(value);
            Loaded?.Invoke(_gameProgress);
        }
    }
}