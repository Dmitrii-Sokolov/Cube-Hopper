using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField]
    private Toggle SoundToggle;

	void Start ()
    {
        SoundToggle.isOn = !Overlord.Sound.Value;
        SoundToggle.onValueChanged.AddListener(c => Overlord.Sound.Value = !c);
    }
}
