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
        Overlord.Progress.Changed += OnProgressChanged;
    }

    private void OnProgressChanged(GameProgress obj)
    {
        switch (obj)
        {
            case GameProgress.Beginning:
                Alpha.Value = 1f;
                break;
            case GameProgress.Processing:
                Alpha.TargetValue = 0f;
                break;
            case GameProgress.Over:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        Alpha.Update();
    }

    private void OnDestroy()
    {
        Overlord.Progress.Changed -= OnProgressChanged;
    }
}
