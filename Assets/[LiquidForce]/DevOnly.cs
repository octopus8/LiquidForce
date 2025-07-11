using System;
using UnityEngine;

public class DevOnly : MonoBehaviour
{
    private void Awake()
    {
        DestroyImmediate(gameObject);
    }
}
