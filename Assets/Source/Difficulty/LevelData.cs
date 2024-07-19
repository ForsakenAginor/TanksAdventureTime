using System;
using UnityEngine;

namespace Assets.Source.Difficulty
{
    public class LevelData
    {
        private const string LevelsVariableName = "Levels";

        public int GetLevel()
        {
            if (PlayerPrefs.HasKey(LevelsVariableName) == false)
                return 0;

            return PlayerPrefs.GetInt(LevelsVariableName);
        }

        public void SaveLevel(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            PlayerPrefs.SetInt(LevelsVariableName, level);
            PlayerPrefs.Save();
        }
    }
}