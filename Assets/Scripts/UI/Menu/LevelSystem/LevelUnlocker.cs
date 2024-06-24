using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Menu.LevelSystem
{
    public class LevelUnlocker
    {
        private readonly LevelData _levelData = new ();
        private readonly Scenes _currentScene;
        private readonly Scenes _nextScene;

        public LevelUnlocker()
        {
            if (Enum.TryParse(SceneManager.GetActiveScene().name, out _currentScene) == false)
                throw new Exception("Wrong scene name");

            int levelsQuantity = Enum.GetNames(typeof(Scenes)).Length - 1;

            if ((int)_currentScene == levelsQuantity)
                _nextScene = _currentScene;
            else
                _nextScene = (Scenes)((int)_currentScene + 1);
        }

        public Scenes NextScene => _nextScene;

        public Scenes CurrentScene => _currentScene;

        public void UnlockNext()
        {
            if (_currentScene == _nextScene)
                return;

            var levels = _levelData.GetLevels().ToList();
            levels.Add(_nextScene);
            _levelData.SaveLevels(levels);
        }
    }
}