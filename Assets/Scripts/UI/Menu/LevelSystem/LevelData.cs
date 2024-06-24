using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.LevelSystem
{
    public class LevelData
    {
        private const string LevelsVariableName = "Levels";

        public IEnumerable<Scenes> GetLevels()
        {
            if (PlayerPrefs.HasKey(LevelsVariableName) == false)
                return null;

            char divider = ' ';
            return PlayerPrefs.GetString(LevelsVariableName).Trim().Split(divider).Select(o => (Scenes)Convert.ToInt32(o)).ToList();
        }

        public void SaveLevels(IEnumerable<Scenes> levelsList)
        {
            if (levelsList == null)
                throw new ArgumentNullException(nameof(levelsList));

            List<Scenes> list = new ();

            foreach (var scene in levelsList)
                if (list.Contains(scene) == false)
                    list.Add(scene);

            StringBuilder builder = new ();
            char divider = ' ';

            foreach (var level in list)
                builder.Append($"{(int)level}{divider}");

            PlayerPrefs.SetString(LevelsVariableName, builder.ToString());
            PlayerPrefs.Save();
        }
    }
}