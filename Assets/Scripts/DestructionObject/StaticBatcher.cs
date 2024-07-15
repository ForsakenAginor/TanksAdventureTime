using Cysharp.Threading.Tasks;
using DestructionObject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticBatcher : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private List<GameObject> _parts = new();

    private void Awake()
    {
        var objects = GetComponentsInChildren<Destruction>();

        foreach (var obj in objects)
        {
            //obj.Destroyed1 += OnPartDestroyed;
        }
    }

    private void OnPartDestroyed(List<GameObject> list)
    {
        var sorted = list.Where(o =>
        {
            return o.activeSelf == true && o.TryGetComponent<MeshRenderer>(out _);
        }).
        ToList();
        BatchingCome(sorted);
    }

    private async void BatchingCome(List<GameObject> list)
    {
        await UniTask.Delay(20000);
        list.ForEach(o =>
        {
            //o.isStatic = true;
            o.transform.parent = _parent;
        });
        _parts.AddRange(list);
        _parts.Where(o => o.activeSelf == false).ToList().ForEach(o => o.transform.parent = null);
        StaticBatchingUtility.Combine(_parent.gameObject);
        Debug.Log("Batched");
    }
}
