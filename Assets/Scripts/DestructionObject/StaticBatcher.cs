using Cysharp.Threading.Tasks;
using DestructionObject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticBatcher : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private List<MeshRenderer> _parts = new();
    private int _limit = 100;
    private int _multiplier = 2;

    private void Awake()
    {
        var objects = GetComponentsInChildren<Destruction>();

        foreach (var part in objects)
        {
            part.Destroyed += OnPartDestroyed;
        }
    }

    private void OnPartDestroyed(List<MeshRenderer> list)
    {
        if(list == null)
            throw new ArgumentNullException(nameof(list));

        _parts.AddRange(list);

        foreach (var part in list)
            part.transform.parent = _parent;

        UniteMesh();
    }

    private void UniteMesh()
    {
        if(_parts.Count >= _limit)
        {
            _limit *= _multiplier;
            _parts.Where(o => o.gameObject.activeSelf == false).ToList().ForEach(o => o.transform.parent = null);
            StaticBatchingUtility.Combine(_parent.gameObject);
            Debug.Log("Batched");
        }
    }
}
