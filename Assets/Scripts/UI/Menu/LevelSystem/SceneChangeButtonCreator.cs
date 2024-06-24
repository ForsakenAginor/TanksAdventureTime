using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.LevelSystem
{
    public class SceneChangeButtonCreator : MonoBehaviour
    {
        private readonly LevelData _levelData = new ();

        [SerializeField] private SceneChanger _prefab;
        [SerializeField] private Transform _holder;

        private void Start()
        {
            List<Scenes> unlockedLevels;
            var levels = _levelData.GetLevels();

            if (levels != null)
            {
                unlockedLevels = levels.OrderByDescending(o => (int)o).ToList();
            }
            else
            {
                unlockedLevels = new List<Scenes>() { Scenes.FirstLevel };
                _levelData.SaveLevels(unlockedLevels);
            }

            foreach (var level in unlockedLevels)
                Instantiate(_prefab, _holder).Init(level);
        }
    }
}