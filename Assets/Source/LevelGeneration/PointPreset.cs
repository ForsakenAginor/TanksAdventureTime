using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    [CreateAssetMenu(fileName = "PointPreset")]
    public class PointPreset : ScriptableObject
    {
        [field: SerializeField] public Point Prefab { get; private set; }

        //[field: SerializeField] public BuildingPresetType Type { get; private set; }

        [field: SerializeField] public PointPresetSize Size { get; private set; }
    }
}