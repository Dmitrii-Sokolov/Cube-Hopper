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

    public static ActionProperty<int> Score = new ActionProperty<int>();
    public static ActionProperty<int> Highscore = new ActionProperty<int>();
    public static ActionProperty<bool> Sound = new ActionProperty<bool>();
    public static ActionProperty<GameProgress> Progress = new ActionProperty<GameProgress>();
    public static RestrictedActionProperty<GameState> State = new RestrictedActionProperty<GameState>(c => (c != GameState.Pause || Progress.Value != GameProgress.Over));

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
            TapOnEmptySpace();

        if (Input.GetButtonDown("Pause"))
            Overlord.State.Value = GameState.Pause;

        if (Input.GetButtonDown("Mute"))
            Sound.Value = !Sound.Value;

        if (Input.GetButtonDown("Exit"))
            Overlord.State.Value = GameState.Exit;
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
        switch (obj)
        {
            case GameProgress.Beginning:
                Score.Value = 0;
                break;
            case GameProgress.Processing:
                break;
            case GameProgress.Over:
                SaveScore();
                break;
            default:
                break;
        }
    }

    //Used for main UI raycaster
    public static void TapOnEmptySpace()
    {
        switch (State.Value)
        {
            case GameState.Game:
                if (Progress.Value == GameProgress.Over)
                    Progress.Value = GameProgress.Beginning;
                else
                    JumpPressed();
                break;
            case GameState.Pause:
                State.Value = GameState.Game;
                break;
            case GameState.Exit:
                break;
            default:
                break;
        }
    }

    public static void SaveScore()
    {
        Highscore.Value = Mathf.Max(Score.Value, Highscore.Value);
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
