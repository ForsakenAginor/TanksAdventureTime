using System;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.Profile
{
    public class PlayerData
    {
        private const string PointsVariableName = "Points";

        public int GetPoints()
        {
            if (PlayerPrefs.HasKey(PointsVariableName))
                return PlayerPrefs.GetInt(PointsVariableName);
            else
                return 0;
        }

        public void SavePoints(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetInt(PointsVariableName, value);
            PlayerPrefs.Save();
        }
    }
}