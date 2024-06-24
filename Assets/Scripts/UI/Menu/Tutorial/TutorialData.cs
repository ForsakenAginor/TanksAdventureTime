using System;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.Tutorial
{
    public class TutorialData
    {
        private const string TutorialVariableName = "IsTutorialCompleted";

        public bool GetTutorialCompletionStatus()
        {
            if (PlayerPrefs.HasKey(TutorialVariableName))
                return Convert.ToBoolean(PlayerPrefs.GetInt(TutorialVariableName));
            else
                return false;
        }

        public void SaveTutorialData()
        {
            PlayerPrefs.SetInt(TutorialVariableName, Convert.ToInt32(true));
            PlayerPrefs.Save();
        }
    }
}