using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField]
    private float CrouchHeight;

    [SerializeField]
    private float CrouchTime;

    [SerializeField]
    private AnimationCurve CrouchCurve;

    private Transform PlatformTransform;
    private float YTargetPosition;
    private float YStartPosition;
    private bool CrouchInProgress;
    private float CrouchState;

    public void Init(float ypos)
    {
        CrouchState = 0f;
        CrouchInProgress = false;
        YTargetPosition = ypos;
        PlatformTransform = transform;
        PlatformTransform.localPosition = new Vector3(PlatformTransform.localPosition.x, YTargetPosition, PlatformTransform.localPosition.z);
    }

    private void Start()
    {
        CrouchState = 0f;
        CrouchInProgress = false;
        PlatformTransform = transform;
        YTargetPosition = 0f;
    }

    public void Crouch()
    {
        if (!CrouchInProgress)
        {
            CrouchState = 0f;
            CrouchInProgress = true;
            YStartPosition = PlatformTransform.localPosition.y;
        }
    }

	void Update ()
    {
        if (CrouchInProgress)
        {
            CrouchState = Mathf.Clamp01(CrouchState + Time.deltaTime / CrouchTime);
            CrouchInProgress = CrouchState < 1f;
            PlatformTransform.localPosition = new Vector3(PlatformTransform.localPosition.x, Mathf.Lerp(YStartPosition, YTargetPosition, CrouchState) + CrouchHeight * CrouchCurve.Evaluate(CrouchState), PlatformTransform.localPosition.z);
        }
    }
}
