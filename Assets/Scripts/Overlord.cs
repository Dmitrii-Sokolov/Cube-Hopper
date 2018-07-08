using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FallEvent { }
public struct RestartEvent { }
public struct FirstJumpEvent { }
public struct JumpEvent { }
public struct NextPlatformEvent { public float Accuracy; }

public class Overlord : MonoBehaviour
{
    [SerializeField]
    private Settings MainSettings;
    
    public static ActionProperty<int> Score = new ActionProperty<int>();
    public static ActionProperty<int> Highscore = new ActionProperty<int>();

    public static bool Processing { get; private set; } = false;

    private void Awake()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;
        EventDispatcher<FirstJumpEvent>.OnEvent += OnFirstJump;
        EventDispatcher<FallEvent>.OnEvent += OnFall;
    }

    private void OnFall(FallEvent obj)
    {
        Debug.Log(666666);
        Highscore.Value = Mathf.Max(Score.Value, Highscore.Value);
    }

    public void Restart()
    {
        Processing = false;
        Score.Value = 0;
        new RestartEvent().Broadcast();
    }

    public void Jump()
    {
        new JumpEvent().Broadcast();
        if (!Processing)
            new FirstJumpEvent().Broadcast();
    }

    private void OnFirstJump(FirstJumpEvent obj)
    {
        Processing = true;
    }

    private void OnNextPlatform(NextPlatformEvent ev)
    {
        Score.Value += Mathf.CeilToInt(ev.Accuracy * 100f);
        //Debug.Log("Accuracy " + ev.Accuracy + "  Score : " + Score.Value);
    }

    private void OnDestroy()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        EventDispatcher<FirstJumpEvent>.OnEvent -= OnFirstJump;
        EventDispatcher<FallEvent>.OnEvent -= OnFall;
    }
}
