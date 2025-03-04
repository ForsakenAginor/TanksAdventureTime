﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPushable
    where T : SpawnableObject
{
    private readonly Queue<SpawnableObject> _spawnQueue = new ();
    private readonly SpawnableObject _spawnableObject;

    public ObjectPool(SpawnableObject spawnableObject)
    {
        _spawnableObject = spawnableObject;
    }

    public void Push(SpawnableObject spawnableObject)
    {
        PushOnInitialize(spawnableObject);
    }

    public T Pull(Vector3 position)
    {
        if (_spawnQueue.Count == 0)
            PushOnInitialize(Object.Instantiate(_spawnableObject, position, Quaternion.identity).Init(this));

        return _spawnQueue.Dequeue().Pull<T>(position);
    }

    private void PushOnInitialize(SpawnableObject spawnableObject)
    {
        spawnableObject.SetActive(false);
        _spawnQueue.Enqueue(spawnableObject);
    }
}