using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameDataSO _gameData;
    [SerializeField] private CardsCollectionSO _cardCollection;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _gameGrid;

    private List<CardController> _cards = new List<CardController>();
    private CardController _firstSelectedCard;
    private CardController _secondSelectedCard;
    private int _matchedPairs = 0;

    public bool IsProcessing { get; private set; }

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
    }

    public void CardFlipped(CardController card)
    {
        if (_firstSelectedCard == null)
        {
            _firstSelectedCard = card;
        }
        else
        {
            _secondSelectedCard = card;
            StartCoroutine(CheckForMatch());
        }
    }

    private IEnumerator CheckForMatch()
    {
        IsProcessing = true;
        yield return new WaitForSeconds(1f); // Give player time to see the second card

        if (_firstSelectedCard.CardData.name == _secondSelectedCard.CardData.name)
        {
            // Match found
            _firstSelectedCard.MarkAsMatched();
            _secondSelectedCard.MarkAsMatched();
            _matchedPairs++;

            // Check for game completion
            if (_matchedPairs >= (_gameData.Rows * _gameData.Columns) / 2)
            {
                Debug.Log("Game Completed!");
            }
        }
        else
        {
            // No match, Flip cards back
            _firstSelectedCard.ResetCard();
            _secondSelectedCard.ResetCard();
        }

        _firstSelectedCard = null;
        _secondSelectedCard = null;
        IsProcessing = false;
    }

    public void RestartGame()
    {
        _matchedPairs = 0;
        _firstSelectedCard = null;
        _secondSelectedCard = null;
        InitializeGame();
    }
}