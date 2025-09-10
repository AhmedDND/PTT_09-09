using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _gameGrid;
    [SerializeField] private TimeController _timeController;

    private List<CardController> _cards = new List<CardController>();
    private CardController _firstSelectedCard;
    private CardController _secondSelectedCard;
    private int _matchedPairs = 0;

    public event Action OnGameCompleted;

    public bool IsProcessing { get; private set; }

    public List<CardController> Cards => _cards;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeGame()
    {
        CreateCardGrid();
        _matchedPairs = 0;
        if (_timeController != null)
            _timeController.StartTimer();
    }

    public void SetGridSize(int rows, int columns)
    {
        GameSettings.Instance.Rows = rows;
        GameSettings.Instance.Columns = columns;

        Debug.Log($"Grid size set to {rows}x{columns}");
    }

    private void CreateCardGrid()
    {
        int totalCards = GameSettings.Instance.Rows * GameSettings.Instance.Columns;

        // Destroy old cards if Restart button is clicked
        foreach (Transform child in _gameGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _cards.Clear();

        // Create Card Grid
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObject = Instantiate(_cardPrefab, _gameGrid.transform);
            cardObject.transform.name = "Card (" + i.ToString() + ")";
            CardController cardController = cardObject.GetComponent<CardController>();
            _cards.Add(cardController);
        }

        CardsCollectionSO cardCollection = GameSettings.Instance.SelectedCategory;
        if (cardCollection == null)
        {
            Debug.LogError("No category selected!");
            return;
        }

        List<CardSO> availableCards = new List<CardSO>(GameSettings.Instance.GetCategory().cards);
        int neededPairs = totalCards / 2;
        // Error checking in case of an invalid input
        if (availableCards.Count < neededPairs)
        {
            Debug.LogError("Not enough unique cards in collection!");
            return;
        }

        List<CardSO> chosenCards = new List<CardSO>();
        for (int i = 0; i < neededPairs; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableCards.Count);
            chosenCards.Add(availableCards[randomIndex]);
            availableCards.RemoveAt(randomIndex);
        }

        List<CardSO> allCards = new List<CardSO>();
        foreach (CardSO card in chosenCards)
        {
            allCards.Add(card);
            allCards.Add(card);
        }

        for (int i = 0; i < allCards.Count; i++)
        {
            CardSO temp = allCards[i];
            int randomIndex = UnityEngine.Random.Range(i, allCards.Count);
            allCards[i] = allCards[randomIndex];
            allCards[randomIndex] = temp;
        }

        // Assign cards to slots
        Sprite cardBackground = GameSettings.Instance.GetCardBackground();
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].Initialize(allCards[i], cardBackground);
        }
    }

    public void CreateGridFromSave(List<CardSO> savedCards, List<CardDataSave> savedCardStates)
    {
        int totalCards = GameSettings.Instance.Rows * GameSettings.Instance.Columns;

        // Destroy old cards
        foreach (Transform child in _gameGrid.transform)
            Destroy(child.gameObject);

        _cards.Clear();
        _matchedPairs = 0;
        _firstSelectedCard = null;
        _secondSelectedCard = null;

        // Create card GameObjects
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObject = Instantiate(_cardPrefab, _gameGrid.transform);
            cardObject.transform.name = "Card (" + i + ")";
            _cards.Add(cardObject.GetComponent<CardController>());
        }

        // Use saved cards order
        Sprite cardBackground = GameSettings.Instance.GetCardBackground();
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].Initialize(savedCards[i], cardBackground);

            // Restore flipped/matched states
            if (savedCardStates[i].IsFlipped)
                _cards[i].ForceFlipWithoutAnimation();

            if (savedCardStates[i].IsMatched)
            {
                _cards[i].MarkAsMatched();
                _matchedPairs++;
            }
        }

        _matchedPairs = savedCardStates.Count(c => c.IsMatched) / 2;
        // Start timer
        if (_timeController != null)
            _timeController.StartTimer();
    }

    public void CardFlipped(CardController card)
    {
        if (card.IsMatched) return;

        if (_firstSelectedCard == null)
        {
            _firstSelectedCard = card;
        }
        else if (_secondSelectedCard == null && card != _firstSelectedCard)
        {
            _secondSelectedCard = card;
            StartCoroutine(CheckForMatch(_firstSelectedCard, _secondSelectedCard));

            _firstSelectedCard = null;
            _secondSelectedCard = null;
        }
    }

    private IEnumerator CheckForMatch(CardController first, CardController second)
    {
        yield return new WaitForSeconds(0.8f);

        ScoreManager.Instance.RegisterTurn();

        if (first.CardData.name == second.CardData.name)
        {
            first.MarkAsMatched();
            second.MarkAsMatched();
            _matchedPairs++;

            ScoreManager.Instance.RegisterMatch(_timeController.RoundTime);

            if (_matchedPairs >= (GameSettings.Instance.Rows * GameSettings.Instance.Columns) / 2)
            {
                Debug.Log("Game Completed!");
               
                if (_timeController != null)
                {
                    _timeController.PauseTimer();
                    _timeController.SetEndGameText();
                }

                ScoreManager.Instance.SaveHighScore(
                GameSettings.Instance.GetCategory().name,
                GameSettings.Instance.Rows,
                GameSettings.Instance.Columns,
                GameSettings.Instance.CurrentDifficulty
                );

                OnGameCompleted?.Invoke();
            }
        }
        else
        {
            first.ResetCard();
            second.ResetCard();

            ScoreManager.Instance.ResetCombo();
        }
    }

    public bool IsGameCompleted()
    {
        int totalPairs = (GameSettings.Instance.Rows * GameSettings.Instance.Columns) / 2;
        return _matchedPairs >= totalPairs;
    }

    public void RestartGame()
    {
        _matchedPairs = 0;
        _firstSelectedCard = null;
        _secondSelectedCard = null;
        ScoreManager.Instance.ResetScore();
        ScoreManager.Instance.UpdateUI();
        InitializeGame();
    }

    public void QuitRound()
    {
        _matchedPairs = 0;
        _firstSelectedCard = null;
        _secondSelectedCard = null;
        ScoreManager.Instance.ResetScore();
        ScoreManager.Instance.UpdateUI();
    }
}