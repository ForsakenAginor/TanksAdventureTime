using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uitest : MonoBehaviour
{
    [SerializeField] private RectTransform _object;
    private void Update()
    {
        Debug.Log($"{_object.rect.width} {_object.rect.height}");
    }
}
