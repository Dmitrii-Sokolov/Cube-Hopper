using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [SerializeField]
    private Vector3 Direction;

    [SerializeField]
    private float speed;

    private Transform MotorTransorm;
    private Vector3 StartPosition;

    private void Awake()
    {
        Overlord.Progress.Changed += OnProgressChanged;
    }

    private void Start()
    {
        MotorTransorm = transform;
        StartPosition = MotorTransorm.localPosition;
    }

    private void OnProgressChanged(GameProgress obj)
    {
        if (obj == GameProgress.Beginning)
            MotorTransorm.localPosition = StartPosition;
    }

    void FixedUpdate ()
    {
        if (Overlord.Progress.Value == GameProgress.Processing)
            MotorTransorm.localPosition += Direction * speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        Overlord.Progress.Changed -= OnProgressChanged;
    }
}
