using UnityEngine;

[CreateAssetMenu(fileName = "BuildingPreset")]
public class BuildingPreset : ScriptableObject
{
    [field: SerializeField] public GameObject Model { get; private set; }

    [field: SerializeField] public BuildingPresetType Type { get; private set; }

    [field: SerializeField] public BuildingPresetSize Size { get; private set; }
}
