using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject Root;

    [SerializeField]
    private GameObject PauseButton;

    [SerializeField]
    private GameObject UnpauseButton;

    private void Start()
    {
        Overlord.State.Changed += OnStateChanged;
    }

    public void Pause(bool isPaused)
    {
        Overlord.State.Value = isPaused ? GameState.Pause : GameState.Game;
    }

    private void OnStateChanged(GameState obj)
    {
        PauseButton.SetActive(obj == GameState.Game);
        UnpauseButton.SetActive(obj != GameState.Game);
        Root.SetActive(obj == GameState.Pause);
    }

    private void OnDestroy()
    {
        Overlord.State.Changed -= OnStateChanged;
    }
}
