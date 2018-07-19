using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    [SerializeField]
    private GameObject NewHighscoreText;

    [SerializeField]
    private GameObject Root;

    private void Start()
    {
        Overlord.Progress.Changed += OnProgressChanged;
        Overlord.State.Changed += OnStateChanged;
        Overlord.Highscore.Changed += (c) => NewHighscoreText.SetActive(true);
    }

    public void Restart()
    {
        Overlord.Progress.Value = GameProgress.Beginning;
    }

    private void OnStateChanged(GameState obj)
    {
        Refresh();
    }

    private void OnProgressChanged(GameProgress obj)
    {
        Refresh();
    }

    private void Refresh()
    {
        Root.SetActive(Overlord.Progress.Value == GameProgress.Over && Overlord.State.Value == GameState.Game);

        if (Overlord.Progress.Value == GameProgress.Beginning)
                NewHighscoreText.SetActive(false);
    }

    private void OnDestroy()
    {
        Overlord.Progress.Changed -= OnProgressChanged;
        Overlord.State.Changed -= OnStateChanged;
    }
}

