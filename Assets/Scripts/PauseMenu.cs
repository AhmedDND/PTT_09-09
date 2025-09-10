using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private TMP_Text _youWonText;

    [SerializeField] private TimeController _timeController;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameCompleted += ShowGameOverMenu;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameCompleted -= ShowGameOverMenu;
    }

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
        _gameOverMenu.SetActive(false);
    }

    public void OnMenuButtonClicked()
    {
        if(!GameManager.Instance.IsGameCompleted())
        {
            SaveLoadSystem.Instance.SaveCurrentRound();

            if (SaveLoadSystem.Instance.HasSavedRound())
            {
                _continueButton.SetActive(true);
            }
        }
        else
        {
            // Game is finished, disable Continue button
            SaveLoadSystem.Instance.ClearSavedRound();
            _continueButton.SetActive(false);
        }

        _mainMenu.SetActive(true);
        _playMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
        GameManager.Instance.QuitRound();
    }

    private void ShowGameOverMenu()
    {
        _gameOverMenu.SetActive(true);
        _youWonText.text = _timeController.SetEndGameText();
    }

    public void OnQuitButtonClicked()
    {
        if (!GameManager.Instance.IsGameCompleted())
        {
            SaveLoadSystem.Instance.SaveCurrentRound();

            if (SaveLoadSystem.Instance.HasSavedRound())
            {
                _continueButton.SetActive(true);
            }
        }
        else
        {
            // Game is finished, disable Continue button
            _continueButton.SetActive(false);
            SaveLoadSystem.Instance.ClearSavedRound();
        }

        Application.Quit();

        // If in editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
