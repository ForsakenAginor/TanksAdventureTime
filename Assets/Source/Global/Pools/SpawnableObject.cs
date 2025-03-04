﻿using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    private GameObject _gameObject;
    private IPushable _spawner;

    public Transform Transform { get; private set; }

    public SpawnableObject Init(IPushable spawner)
    {
        _spawner = spawner;
        _gameObject = gameObject;
        Transform = transform;

        SetActive(false);
        return this;
    }

    public T Pull<T>(Vector3 position)
        where T : SpawnableObject
    {
        Transform.position = position;
        return Pull<T>();
    }

    public void Push()
    {
        _spawner.Push(this);
        Transform.position = Vector3.zero;
    }

    public void SetActive(bool value)
    {
        _gameObject.SetActive(value);
    }

    private T Pull<T>()
        where T : SpawnableObject
    {
        SetActive(true);
        return this as T;
    }
}