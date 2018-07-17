using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NextPlatformEvent { public float Accuracy; public int Grade; public int Score; public Vector3 Position; }

public enum GameProgress
{
    Beginning,
    Processing,
    Over
}

public enum GameState
{
    Game,
    Pause,
    Exit
}

public class Overlord : MonoBehaviour
{
    [SerializeField]
    private string BestScoreKey = "Highscore";

    [SerializeField]
    private string SoundKey = "Sound";

    [SerializeField]
    private string[] PauseButtons;

    [SerializeField]
    private string[] JumpButtons;

    public static ActionProperty<int> Score = new ActionProperty<int>();
    public static ActionProperty<int> Highscore = new ActionProperty<int>();
    public static ActionProperty<bool> Sound = new ActionProperty<bool>();
    public static ActionProperty<GameProgress> Progress = new ActionProperty<GameProgress>();
    public static ActionProperty<GameState> State = new ActionProperty<GameState>();

    public static Action JumpPressed;
    public static Action JumpPerformed;

    private Exception TextException = null;

    private void Awake()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;
        Overlord.Progress.Changed += OnProgressChanged;
        Overlord.State.Changed += OnStateChanged; 

        Highscore.Value = PlayerPrefs.GetInt(BestScoreKey, 0);
        Sound.Value = PlayerPrefs.GetInt(SoundKey, 1) == 1;

        Highscore.Changed += (c) => PlayerPrefs.SetInt(BestScoreKey, c);
        Sound.Changed += (c) => PlayerPrefs.SetInt(SoundKey, c ? 1 : 0);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            if (Progress.Value == GameProgress.Over)
                Restart();
            else
                Jump();

        //if (Input.GetButtonDown("Pause"))
        //    Pause();

        //if (Input.GetButtonDown("Mute"))
        //    Mute();
    }

    private void OnStateChanged(GameState obj)
    {
        switch (obj)
        {
            case GameState.Game:
                Time.timeScale = 1f;
                break;
            case GameState.Pause:
                Time.timeScale = 0f;
                break;
            case GameState.Exit:
                Time.timeScale = 0f;
                break;
            default:
                break;
        }
    }

    private void OnProgressChanged(GameProgress obj)
    {
        if (obj == GameProgress.Over)
            Highscore.Value = Mathf.Max(Score.Value, Highscore.Value);
    }

    public void Pause(bool paused)
    {
        Overlord.State.Value = paused ? GameState.Pause : GameState.Game;
    }

    //public void Pause()
    //{
    //    Overlord.State.Value = Overlord.State.Value == GameState.Pause ? GameState.Pause : GameState.Game;
    //}

    public void Restart()
    {
        Score.Value = 0;
        Progress.Value = GameProgress.Beginning;
    }

    public void Jump()
    {
        JumpPressed();
    }

    private void OnNextPlatform(NextPlatformEvent ev)
    {
        Score.Value += ev.Score;
    }

    private void OnDestroy()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        Overlord.Progress.Changed -= OnProgressChanged;
        Overlord.State.Changed -= OnStateChanged;
        }
    }
