using UnityEngine;

namespace LevelGeneration
{
    [CreateAssetMenu(fileName = "PointPreset")]
    public class PointPreset : ScriptableObject
    {
        [field: SerializeField] public Point Prefab { get; private set; }

        [field: SerializeField] public PointPresetSize Size { get; private set; }
    }
}