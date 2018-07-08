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
        EventDispatcher<RestartEvent>.OnEvent += OnStart;
    }

    private void Start()
    {
        MotorTransorm = transform;
        StartPosition = MotorTransorm.position;
    }

    private void OnStart(RestartEvent obj)
    {
        MotorTransorm.position = StartPosition;
    }

    void FixedUpdate ()
    {
        if (Overlord.Processing)
            MotorTransorm.position += Direction * speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        EventDispatcher<RestartEvent>.OnEvent -= OnStart;
    }
}
