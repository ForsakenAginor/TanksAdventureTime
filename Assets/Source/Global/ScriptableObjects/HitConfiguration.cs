using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HitConfiguration", menuName = "Hit Damage Configuration", order = 2)]
public class HitConfiguration : ScriptableObject
{
    [SerializeField] private List<SerializedPair<HitTypes, int>> _damages;

    private void OnValidate()
    {
        foreach (var pair in Create().Where(pair => _damages.Exists(item => item.Key == pair.Key) == false))
            _damages.Add(pair);
    }

    public int GetDamage(HitTypes type)
    {
        return _damages.Find(pair => pair.Key == type).Value;
    }

    private List<SerializedPair<HitTypes, int>> Create()
    {
        List<SerializedPair<HitTypes, int>> result = new ();
        int value = (int)ValueConstants.Zero;

        foreach (HitTypes type in Enum.GetValues(typeof(HitTypes)))
            result.Add(new SerializedPair<HitTypes, int>(type, value));

        return result;
    }
}