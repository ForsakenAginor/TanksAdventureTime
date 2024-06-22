using System.Collections.Generic;
using UnityEngine;

public class BuildingPresetCollection : MonoBehaviour
{
    [SerializeField] private BuildingPreset[] _presets;

    public IEnumerable<BuildingPreset> Presets => _presets;
}
