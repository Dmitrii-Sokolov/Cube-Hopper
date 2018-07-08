using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RestartEvent { }
public struct FirstJumpEvent { }
public struct JumpEvent { }
public struct NextPlatformEvent { public float Accuracy; }

public class Overlord : MonoBehaviour
{
    public static bool Processing { get; private set; } = false;

    private void Awake()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;
        EventDispatcher<FirstJumpEvent>.OnEvent += OnFirstJump;
    }

    public void Restart()
    {
        Processing = false;
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
        Debug.Log("Accuracy " + ev.Accuracy);
    }

    private void OnDestroy()
    {
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        EventDispatcher<FirstJumpEvent>.OnEvent -= OnFirstJump;
    }
}
