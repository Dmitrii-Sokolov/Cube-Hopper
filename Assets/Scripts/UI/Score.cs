using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;

    private void Start()
    {
        Overlord.Score.Changed += ScoreChanged;

        ScoreChanged(Overlord.Score.Value);
    }

    private void ScoreChanged(int score)
    {
        ScoreText.text = $"{score}";
    }

    private void OnDestroy()
    {
        Overlord.Score.Changed -= ScoreChanged;
    }
}
