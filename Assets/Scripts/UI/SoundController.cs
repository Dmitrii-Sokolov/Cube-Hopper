using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private GameObject MuteButton;

    [SerializeField]
    private GameObject UnmuteButton;

    private void Start()
    {
        Overlord.Sound.Changed += OnSoundChanged;

        OnSoundChanged(Overlord.Sound.Value);
    }

    private void OnSoundChanged(bool obj)
    {
        MuteButton.SetActive(obj);
        UnmuteButton.SetActive(!obj);
    }

    public void ToggleSound(bool isOn)
    {
        Overlord.Sound.Value = isOn;
    }

    private void OnDestroy()
    {
        Overlord.Sound.Changed -= OnSoundChanged;
    }
}
