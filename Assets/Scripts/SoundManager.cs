using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitAudioSettings
{
    public AudioClip Clip;
    public float Volume;
}

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private AudioSource Music;

    [SerializeField]
    private AudioSource SFX;

    [SerializeField]
    private AudioClip Jump;

    [SerializeField]
    private float MinJumpPitch = 0.9f;

    [SerializeField]
    private float MaxJumpPitch = 1.1f;

    [SerializeField]
    private HitAudioSettings[] HitSettings;

    void Awake ()
    {
        EventDispatcher<JumpEvent>.OnEvent += OnJump;
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;

        ToggleSound(Overlord.Sound.Value);
        Overlord.Sound.Changed += ToggleSound;
    }

    private void ToggleSound(bool isOn)
    {
        SFX.mute = !isOn;
        Music.mute = !isOn;
    }

    private void OnJump(JumpEvent obj)
    {
        SFX.pitch = MinJumpPitch + UnityEngine.Random.value * (MaxJumpPitch - MinJumpPitch);
        SFX.PlayOneShot(Jump);
    }

    private void OnNextPlatform(NextPlatformEvent obj)
    {
        SFX.pitch = 1f;
        SFX.PlayOneShot(HitSettings[obj.Grade].Clip, HitSettings[obj.Grade].Volume);
    }

    private void OnDestroy()
    {
        EventDispatcher<JumpEvent>.OnEvent -= OnJump;
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        Overlord.Sound.Changed -= ToggleSound;
    }
}
