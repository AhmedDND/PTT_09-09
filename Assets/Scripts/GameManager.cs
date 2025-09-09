using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameDataSO _gameData;
    [SerializeField] private CardsCollectionSO _cardCollection;
    [SerializeField] private GameObject _cardPrefab;

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

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        CreateCardGrid();
    }

    private void CreateCardGrid()
    {
        int totalCards = _gameData.Rows * _gameData.Columns;

        // Create card grid
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObject = Instantiate(_cardPrefab, transform);
            cardObject.transform.name = "Card (" + i.ToString() + "à)";
            CardController cardController = cardObject.GetComponent<CardController>();
            _cards.Add(cardController);
        }
    }
}