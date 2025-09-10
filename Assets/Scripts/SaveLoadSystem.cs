using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem Instance;

    [SerializeField] private TimeController _timeController;
    [SerializeField] private ScoreManager _scoreManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
    }

    public void SaveCurrentRound()
    {
        GameSaveData saveData = new GameSaveData
        {
            RoundTime = _timeController.RoundTime,
            TotalScore = _scoreManager.TotalScore,
            TurnsTaken = _scoreManager.TurnsTaken,
            CurrentCombo = _scoreManager.CurrentCombo, // make this public/getter
            Rows = GameSettings.Instance.Rows,
            Columns = GameSettings.Instance.Columns,
            CategoryName = GameSettings.Instance.GetCategory().name,
            Difficulty = GameSettings.Instance.CurrentDifficulty
        };

        foreach (var card in GameManager.Instance.Cards)
        {
            saveData.Cards.Add(new CardDataSave
            {
                CardName = card.CardData.name,
                IsFlipped = card.IsFlipped,
                IsMatched = card.IsMatched
            });
        }

        SaveLoadManager.SaveGame(saveData);
    }

    public void LoadSavedRound()
    {
        GameSaveData saveData = SaveLoadManager.LoadGame();
        if (saveData == null) return;

        // Apply settings
        GameSettings.Instance.Rows = saveData.Rows;
        GameSettings.Instance.Columns = saveData.Columns;
        GameSettings.Instance.SetDifficulty(saveData.Difficulty);

        var category = GameSettings.Instance.GetCategory();
        if (category.name != saveData.CategoryName)
        {
            Debug.LogWarning("Saved category does not match current category!");
        }

        List<CardSO> savedCards = new List<CardSO>();
        foreach (var savedCard in saveData.Cards)
        {
            CardSO cardSO = GameSettings.Instance.GetCategory().cards.Find(c => c.name == savedCard.CardName);
            if (cardSO != null)
                savedCards.Add(cardSO);
        }

        GameManager.Instance.InitializeGame();

        // Match cards to saved state
        var cards = GameManager.Instance.Cards;
        for (int i = 0; i < saveData.Cards.Count; i++)
        {
            var savedCard = saveData.Cards[i];
            var cardController = cards.Find(c => c.CardData.name == savedCard.CardName && !c.IsMatched && !c.IsFlipped);

            if (cardController != null)
            {
                if (savedCard.IsFlipped)
                    cardController.ForceFlipWithoutAnimation();

                if (savedCard.IsMatched)
                    cardController.MarkAsMatched();
            }
        }

        _timeController.SetRoundTime(saveData.RoundTime);
        ScoreManager.Instance.SetScoreFromSaveFile(saveData.TotalScore, saveData.TurnsTaken, saveData.CurrentCombo);
    }
}
