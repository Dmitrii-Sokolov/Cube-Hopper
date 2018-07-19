using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    [SerializeField]
    private GameObject Root;

    private void Start()
    {
        Overlord.State.Changed += OnStateChanged;
    }

    private void OnStateChanged(GameState obj)
    {
        Root.SetActive(obj == GameState.Exit);
    }

    public void Return()
    {
        Overlord.State.Value = GameState.Pause;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        Overlord.State.Changed -= OnStateChanged;
    }
}
