using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class PointPresetCollection : MonoBehaviour
    {
        [SerializeField] private PointPreset[] _presets;

        public IEnumerable<PointPreset> Presets => _presets;
    }
}