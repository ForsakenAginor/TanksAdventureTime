using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : IPushable
{
    private readonly Queue<SpawnableObject> SpawnQueue = new ();
    private readonly SpawnableObject SpawnableObject;

    public ObjectPool(SpawnableObject spawnableObject)
    {
        SpawnableObject = spawnableObject;
    }

    public virtual void Push(SpawnableObject spawnableObject)
    {
        PushOnInitialize(spawnableObject);
    }

    public T Pull<T>(Vector3 position)
        where T : SpawnableObject
    {
        if (SpawnQueue.Count == 0)
            PushOnInitialize(Object.Instantiate(SpawnableObject, position, Quaternion.identity).Init(this));

        return SpawnQueue.Dequeue().Pull<T>(position);
    }

    public T Pull<T>(Transform parent)
        where T : SpawnableObject
    {
        if (SpawnQueue.Count == 0)
            PushOnInitialize(Object.Instantiate(SpawnableObject, parent).Init(this));

        return SpawnQueue.Dequeue().Pull<T>(parent);
    }

    public T Pull<T>(Transform parent, Vector3 position)
        where T : SpawnableObject
    {
        if (SpawnQueue.Count == 0)
            PushOnInitialize(Object.Instantiate(SpawnableObject, parent).Init(this));

        return SpawnQueue.Dequeue().Pull<T>(parent, position);
    }

    private void PushOnInitialize(SpawnableObject spawnableObject)
    {
        spawnableObject.SetActive(false);
        SpawnQueue.Enqueue(spawnableObject);
    }
}