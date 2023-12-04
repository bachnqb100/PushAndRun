using System;
using UnityEngine;

public class FpsTarget : MonoBehaviour
{
    [SerializeField] private int targetFps = 60;
    private void Awake()
    {
        Application.targetFrameRate = targetFps;
    }
}
