using UnityEngine;

public class MainMenuManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _chooseGridPanel;
    [SerializeField] private GameObject _categoryMenuPanel;
    [SerializeField] private GameObject _gameplayPanel;

    public void OnGridSizeChosen(int rows, int cols)
    {
        // Update grid size in GameManager
        GameManager.Instance.SetGridSize(rows, cols);

        // Switch panels
        _chooseGridPanel.SetActive(false);
        _categoryMenuPanel.SetActive(true);
    }

    public void OnDifficultyChosen(int difficultyIndex)
    {
        Difficulty chosen = (Difficulty)difficultyIndex;
        GameSettings.Instance.SetDifficulty(chosen);
    }

    public void OnQuitClicked()
    {
        Application.Quit();

        // If in editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}