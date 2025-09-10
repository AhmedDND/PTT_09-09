using UnityEngine;

public class MainMenuManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _chooseGridPanel;
    [SerializeField] private GameObject _categoryMenuPanel;
    [SerializeField] private GameObject _gameplayPanel;

    [SerializeField] private CardsCollectionSO _animalsCollection;
    [SerializeField] private CardsCollectionSO _vegetablesCollection;
    [SerializeField] private CardsCollectionSO _fruitsCollection;
    [SerializeField] private CardsCollectionSO _vehiclesCollection;

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

    public void OnAnimalsSelected()
    {
        GameSettings.Instance.SetCategory(_animalsCollection);
    }

    public void OnVegetablesSelected()
    {
        GameSettings.Instance.SetCategory(_vegetablesCollection);
    }

    public void OnFruitsSelected()
    {
        GameSettings.Instance.SetCategory(_fruitsCollection);
    }

    public void OnVehiclesSelected()
    {
        GameSettings.Instance.SetCategory(_vehiclesCollection);
    }

    public void OnContinueButtonClicked()
    {
        SaveLoadSystem.Instance.LoadSavedRound();
    }

    public void OnPlayButtonChosen()
    {
        GameManager.Instance.InitializeGame();
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