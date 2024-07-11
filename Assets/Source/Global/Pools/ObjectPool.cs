using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPushable
    where T : SpawnableObject
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

    public T Pull(Vector3 position)
    {
        if (SpawnQueue.Count == 0)
            PushOnInitialize(Object.Instantiate(SpawnableObject, position, Quaternion.identity).Init(this));

        return SpawnQueue.Dequeue().Pull<T>(position);
    }

    public T Pull(Transform parent)
    {
        if (SpawnQueue.Count == 0)
            PushOnInitialize(Object.Instantiate(SpawnableObject, parent).Init(this));

        return SpawnQueue.Dequeue().Pull<T>(parent);
    }

    public T Pull(Transform parent, Vector3 position)
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