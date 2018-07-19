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

    void Start ()
    {
        Overlord.JumpPerformed += OnJump;
        EventDispatcher<NextPlatformEvent>.OnEvent += OnNextPlatform;

        ToggleSound(Overlord.Sound.Value);
        Overlord.Sound.Changed += ToggleSound;
    }

    private void ToggleSound(bool isOn)
    {
        SFX.mute = !isOn;
        Music.mute = !isOn;
    }

    private void OnJump()
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
        Overlord.JumpPerformed -= OnJump;
        EventDispatcher<NextPlatformEvent>.OnEvent -= OnNextPlatform;
        Overlord.Sound.Changed -= ToggleSound;
    }
}
