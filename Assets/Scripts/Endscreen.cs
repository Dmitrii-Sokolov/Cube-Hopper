using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    [SerializeField]
    private GameObject NewHighscoreText;

    private void Awake()
    {
        EventDispatcher<RestartEvent>.OnEvent += OnStart;
        EventDispatcher<FallEvent>.OnEvent += OnFall;
        Overlord.Highscore.Changed += (c) => NewHighscoreText.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnStart(RestartEvent obj)
    {
        gameObject.SetActive(false);
        NewHighscoreText.SetActive(false);
    }

    private void OnFall(FallEvent obj)
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventDispatcher<RestartEvent>.OnEvent -= OnStart;
        EventDispatcher<FallEvent>.OnEvent -= OnFall;
    }
}

