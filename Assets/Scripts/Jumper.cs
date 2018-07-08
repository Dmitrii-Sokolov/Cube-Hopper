using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField]
    private float JumpTime;

    [SerializeField]
    private float JumpAngle;

    [SerializeField]
    private float JumpShift;

    [SerializeField]
    private float JumpHeight;

    [SerializeField]
    private AnimationCurve HeightCurve;

    [SerializeField]
    private AnimationCurve RotationCurve;

    [SerializeField]
    private AnimationCurve ShiftCurve;

    private Vector3 JumpDirection;
    private float JumpRotation;
    private Transform CubeTransform;

    private bool JumpInProgress = false;
    private float JumpState = 0f;
    private Vector3 JumpPoint;

    private Transform StickyTransform;
    private Vector3 StickyShift;

    private bool StablePosition = true;

    private Vector3 StartPosition;
    private Quaternion StartRotation;

    void Start ()
    {
        CubeTransform = transform;
        JumpDirection = CubeTransform.forward;
        JumpRotation = CubeTransform.rotation.eulerAngles.y;

        StartPosition = CubeTransform.position;
        StartRotation = CubeTransform.rotation;

        EventDispatcher<JumpEvent>.OnEvent += StartJump;
        EventDispatcher<RestartEvent>.OnEvent += OnStart;
    }

    private void OnStart(RestartEvent obj)
    {
        StickyTransform = null;
        CubeTransform.position = StartPosition;
        CubeTransform.rotation = StartRotation;
        JumpInProgress = false;
        JumpState = 0f;
        StablePosition = true;
    }

    public void StartJump(JumpEvent ev)
    {
        if (!JumpInProgress && StablePosition)
        {
            JumpPoint = CubeTransform.position;
            JumpInProgress = true;
            JumpState = 0f;
            StickyTransform = null;
        }
    }

    private void Update()
    {
        if (JumpInProgress)
        {
            JumpState = Mathf.Clamp01(JumpState + Time.deltaTime / JumpTime);
            JumpInProgress = JumpState < 1f;
            CubeTransform.SetPositionAndRotation(JumpPoint + new Vector3(0, JumpHeight * HeightCurve.Evaluate(JumpState), 0) + JumpShift * JumpDirection * ShiftCurve.Evaluate(JumpState), Quaternion.Euler(JumpAngle * RotationCurve.Evaluate(JumpState), JumpRotation, 0f));
        }
        else
        {
            if (StickyTransform != null)
                CubeTransform.position = StickyTransform.position + StickyShift;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StablePosition = StablePosition && Physics.Raycast(CubeTransform.position, Vector3.down);

        if (StablePosition)
        {
            StickyTransform = collision.transform;
            StickyShift = CubeTransform.position - StickyTransform.position;

            var Length = 2f;
            if (collision.collider is BoxCollider)
                Length = 2f * ((BoxCollider)collision.collider).size.x;

            if (Overlord.Processing)
                new NextPlatformEvent() { Accuracy = 1f - Mathf.Abs(StickyShift.x / Length)}.Broadcast();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
            new FallEvent().Broadcast();
    }

    private void OnDestroy()
    {
        EventDispatcher<JumpEvent>.OnEvent -= StartJump;
        EventDispatcher<RestartEvent>.OnEvent -= OnStart;
    }
}
