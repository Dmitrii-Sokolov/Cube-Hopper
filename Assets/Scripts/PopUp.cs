using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;

    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private Vector3 Shift;

    [SerializeField]
    private Vector3 FadeShift;

    [SerializeField]
    private float FadeTime;

    [SerializeField]
    private Color[] ColorSet;

    [SerializeField]
    private AnimationCurve Curve;

    [SerializeField]
    private CanvasGroup Canvas;

    private RectTransform PopUpTransform;
    private float FadeState;
    private bool FadeInProgress;
    private Vector3 StartPosition;

    void Awake()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;
        EventDispatcher<RestartEvent>.OnEvent += OnStart;
        PopUpTransform = transform as RectTransform;
    }

    private void Start()
    {
        OnStart(new RestartEvent());
    }

    private void OnStart(RestartEvent obj)
    {
        gameObject.SetActive(false);
        FadeInProgress = false;
        FadeState = 0f;
    }

    private void OnNextPlatform(NextPlatformEvent obj)
    {
        gameObject.SetActive(true);
        FadeInProgress = true;
        FadeState = 0f;

        ScoreText.text = obj.Score.ToString();
        ScoreText.color = ColorSet[obj.Grade];
        StartPosition = MainCamera.WorldToScreenPoint(obj.Position) + Shift;
    }

    private void Update()
    {
        if (FadeInProgress)
        {
            FadeState = Mathf.Clamp01(FadeState + Time.deltaTime / FadeTime);
            FadeInProgress = FadeState < 1f;
            PopUpTransform.position = StartPosition + FadeShift * Curve.Evaluate(FadeState);
            Canvas.alpha = 1f - Curve.Evaluate(FadeState);
        }
    }

    private void OnDestroy()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        EventDispatcher<RestartEvent>.OnEvent -= OnStart;
    }
}
