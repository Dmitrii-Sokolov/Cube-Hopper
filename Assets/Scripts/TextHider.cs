using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHider : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup Canvas;

    [SerializeField]
    private AnimationCurve Curve;

    [SerializeField]
    private float HideTime;

    private AnimatedFloat Alpha;

    void Start ()
    {
        Alpha = new AnimatedFloat(1f, HideTime, Curve, (c) => Canvas.alpha = c);

        EventDispatcher<RestartEvent>.OnEvent += OnStart;
        EventDispatcher<FirstJumpEvent>.OnEvent += StartHide;
    }

    private void StartHide(FirstJumpEvent obj)
    {
        Alpha.TargetValue = 0f;
    }

    private void OnStart(RestartEvent obj)
    {
        Alpha.Value = 1f;
    }

    private void Update()
    {
        Alpha.Update();
    }

    private void OnDestroy()
    {
        EventDispatcher<RestartEvent>.OnEvent -= OnStart;
        EventDispatcher<FirstJumpEvent>.OnEvent -= StartHide;
    }
}
