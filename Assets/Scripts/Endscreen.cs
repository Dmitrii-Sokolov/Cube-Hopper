using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    [SerializeField]
    private GameObject NewHighscoreText;

    private void Start()
    {
        Overlord.Progress.Changed += OnProgressChanged;
        Overlord.Highscore.Changed += (c) => NewHighscoreText.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnProgressChanged(GameProgress obj)
    {
        switch (obj)
        {
            case GameProgress.Beginning:
                gameObject.SetActive(false);
                NewHighscoreText.SetActive(false);
                break;
            case GameProgress.Processing:
                break;
            case GameProgress.Over:
                gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        Overlord.Progress.Changed -= OnProgressChanged;
    }
}

