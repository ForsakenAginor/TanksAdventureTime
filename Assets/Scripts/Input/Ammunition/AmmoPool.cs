using System.Collections.Generic;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
    private readonly Stack<Ammo> ammos = new();

    [SerializeField] private Ammo _prefab;

    public Ammo Pull()
    {
        Ammo ammo;

        if (ammos.Count > 0)
        {
            ammo = ammos.Pop();
            ammo.gameObject.SetActive(true);
            return ammo;
        }

        ammo = Instantiate(_prefab);
        ammo.Init(this);
        return ammo;
    }

    public void Push(Ammo ammo)
    {
        ammo.gameObject.SetActive(false);
        ammos.Push(ammo);
    }
}