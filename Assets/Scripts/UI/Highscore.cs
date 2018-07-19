using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;

    private void Start()
    {
        Overlord.Highscore.Changed += ScoreChanged;

        ScoreChanged(Overlord.Highscore.Value);
    }

    private void ScoreChanged(int score)
    {
        ScoreText.text = $"BEST SCORE   {score}";
    }

    private void OnDestroy()
    {
        Overlord.Highscore.Changed -= ScoreChanged;
    }
}
