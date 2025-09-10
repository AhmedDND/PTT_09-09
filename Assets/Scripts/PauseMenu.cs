using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _mainMenu;

    [SerializeField] private TimeController _timeController;

    public void OnPauseButtonPressed()
    {
        _timeController.PauseTimer();
        _pauseMenu.SetActive(true);
    }

    public void OnResumeButtonPressed()
    {
        _timeController.ResumeTimer();
        _pauseMenu.SetActive(false);
    }

    public void OnRestartButtonPressed()
    {
        GameManager.Instance.RestartGame();
        _timeController.ResetTimer();
        _pauseMenu.SetActive(false);
    }

    public void OnMenuButtonClicked()
    {
        _mainMenu.SetActive(true);
        _playMenu.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();

        // If in editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
