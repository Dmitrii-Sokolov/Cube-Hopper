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

    private bool JumpInProgress;
    private float JumpState;
    private Vector3 JumpPoint;

    private Transform StickyTransform;
    private Vector3 StickyShift;

    private bool StablePosition;

    private Vector3 StartPosition;
    private Quaternion StartRotation;

    void Start ()
    {
        CubeTransform = transform;
        JumpDirection = CubeTransform.forward;
        JumpRotation = CubeTransform.localRotation.eulerAngles.y;

        StartPosition = CubeTransform.localPosition;
        StartRotation = CubeTransform.localRotation;

        EventDispatcher<JumpEvent>.OnEvent += StartJump;
        EventDispatcher<RestartEvent>.OnEvent += OnStart;

        OnStart(new RestartEvent());
    }

    private void OnStart(RestartEvent obj)
    {
        StickyTransform = null;

        CubeTransform.localPosition = StartPosition;
        CubeTransform.localRotation = StartRotation;

        RaycastHit hit;
        if (Physics.Raycast(CubeTransform.position, Vector3.down, out hit) && hit.transform.tag == "Platform")
        {
            StickyTransform = hit.transform;
            StickyShift = CubeTransform.localPosition - StickyTransform.position;
        }

        JumpInProgress = false;
        JumpState = 0f;
        StablePosition = true;
    }

    public void StartJump(JumpEvent ev)
    {
        if (!JumpInProgress && StablePosition)
        {
            JumpPoint = CubeTransform.localPosition;
            JumpInProgress = true;
            JumpState = 0f;

            if (StickyTransform != null)
                StickyTransform.GetComponent<Platform>().Crouch();

            StickyTransform = null;
        }
    }

    private void Update()
    {
        if (JumpInProgress)
        {
            JumpState = Mathf.Clamp01(JumpState + Time.deltaTime / JumpTime);
            CubeTransform.SetPositionAndRotation(new Vector3(JumpPoint.x, Mathf.Lerp(JumpPoint.y, 0f, JumpState) + JumpHeight * HeightCurve.Evaluate(JumpState), JumpPoint.z) + JumpShift * JumpDirection * ShiftCurve.Evaluate(JumpState), Quaternion.Euler(JumpAngle * RotationCurve.Evaluate(JumpState), JumpRotation, 0f));

            if (JumpState >= 1f)
            {
                JumpInProgress = false;
                CheckGround();
            }
        }
        else
        {
            if (StickyTransform != null)
                CubeTransform.localPosition = StickyTransform.position + StickyShift;
        }
    }

    private void CheckGround()
    {
        RaycastHit hit;
        StablePosition = Physics.Raycast(CubeTransform.position, Vector3.down, out hit) && hit.transform.tag == "Platform";

        if (StablePosition)
        {
            StickyTransform = hit.transform;
            StickyShift = CubeTransform.localPosition - StickyTransform.position;
            StickyTransform.GetComponent<Platform>().Crouch();

            var Length = 2f;
            if (hit.collider is BoxCollider)
                Length = 2f * ((BoxCollider)hit.collider).size.x;

            new NextPlatformEvent() { Accuracy = 1f - Mathf.Abs(StickyShift.x / Length)}.Broadcast();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
            new FallEvent().Broadcast();
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
