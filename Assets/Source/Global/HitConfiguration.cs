using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HitConfiguration", menuName = "Hit Damage Configuration", order = 2)]
public class HitConfiguration : UpdatableConfiguration<HitTypes, int>
{
    public int GetDamage(HitTypes type)
    {
        return Content.First(pair => pair.Key == type).Value;
    }
}