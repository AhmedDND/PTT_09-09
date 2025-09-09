using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private Image _cardFrontFace;
    [SerializeField] private Image _cardBackFace;
    [SerializeField] private Button _cardButton;

    private CardSO _cardData;
    private bool _isFlipped = false;
    private bool _isMatched = false;

    public CardSO CardData => _cardData;
    public bool IsMatched => _isMatched;

    public void Initialize(CardSO cardData, Sprite cardBack)
    {
        _cardData = cardData;
        _cardFrontFace.sprite = cardData.CardFrontFace;

        // WIP: Fix so that it grabs the sprite from game difficulty
        _cardBackFace.sprite = cardBack;

        // Start with card face down
        _cardFrontFace.gameObject.SetActive(false);
        _cardBackFace.gameObject.SetActive(true);
    }

    public void FlipCard()
    {
        
    }

    public void MarkAsMatched()
    {
        
    }

    public void ResetCard()
    {
        
    }
}