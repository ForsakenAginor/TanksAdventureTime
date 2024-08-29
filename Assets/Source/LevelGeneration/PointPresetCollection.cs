using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    public class PointPresetCollection : MonoBehaviour
    {
        [SerializeField] private PointPreset[] _presets;

        public IEnumerable<PointPreset> Presets => _presets;
    }
}