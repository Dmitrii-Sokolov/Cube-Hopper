using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FallEvent { }
public struct RestartEvent { }
public struct FirstJumpEvent { }
public struct JumpEvent { }
public struct NextPlatformEvent { public float Accuracy; public int Grade; public int Score; public Vector3 Position; }

public class Overlord : MonoBehaviour
{
    [SerializeField]
    private Settings MainSettings;

    public static ActionProperty<int> Score = new ActionProperty<int>();
    public static ActionProperty<int> Highscore = new ActionProperty<int>();
    public static ActionProperty<bool> Sound = new ActionProperty<bool>();

    public static Action JumpPressed;

    public static bool Processing { get; private set; } = false;

    private void Awake()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;
        EventDispatcher<FirstJumpEvent>.OnEvent += OnFirstJump;
        EventDispatcher<FallEvent>.OnEvent += OnFall;

        Highscore.Value = MainSettings.Highscore;
        Sound.Value = MainSettings.Sound;

        Highscore.Changed += (c) => MainSettings.Highscore = c;
        Sound.Changed += (c) => MainSettings.Sound = c;
    }

    private void OnFall(FallEvent obj)
    {
        Highscore.Value = Mathf.Max(Score.Value, Highscore.Value);
        Processing = false;
    }

    public void Pause(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
    }

    public void Restart()
    {
        Processing = false;
        Score.Value = 0;
        new RestartEvent().Broadcast();
    }

    public void Jump()
    {
        JumpPressed();
        if (!Processing)
            new FirstJumpEvent().Broadcast();
    }

    private void OnFirstJump(FirstJumpEvent obj)
    {
        Processing = true;
    }

    private void OnNextPlatform(NextPlatformEvent ev)
    {
        Score.Value += ev.Score;
    }

    private void OnDestroy()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        EventDispatcher<FirstJumpEvent>.OnEvent -= OnFirstJump;
        EventDispatcher<FallEvent>.OnEvent -= OnFall;
    }
}
