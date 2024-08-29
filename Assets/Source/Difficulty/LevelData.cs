using System;
using UnityEngine;

namespace Difficulty
{
    public class LevelData
    {
        private const string LevelsVariableName = "Levels";

        public int GetLevel()
        {
            if (PlayerPrefs.HasKey(LevelsVariableName) == false)
                return 1;

            return PlayerPrefs.GetInt(LevelsVariableName);
        }

        public void SaveLevel(int level)
        {
            if (level < 1)
                throw new ArgumentOutOfRangeException(nameof(level));

            PlayerPrefs.SetInt(LevelsVariableName, level);
            PlayerPrefs.Save();
        }
    }
}