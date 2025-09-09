using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private Image _cardFrontFace;
    [SerializeField] private Image _cardBackFace;
    [SerializeField] private Button _cardButton;

    private CardSO _cardData;
    private bool _isFlipped = false;
    private bool _isFlipping = false;
    private bool _isMatched = false;

    public CardSO CardData => _cardData;
    public bool IsMatched => _isMatched;

    public void Initialize(CardSO cardData, Sprite cardBack)
    {
        _cardData = cardData;
        _cardFrontFace.sprite = cardData.CardFrontFace;

        // WIP: Fix so that it grabs the sprite from game difficulty
        _cardBackFace.sprite = cardBack;

        _cardFrontFace.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        _cardBackFace.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FlipCard()
    {
        if (_isMatched || GameManager.Instance.IsProcessing) return;
        if (!_isFlipping)
        {
            _isFlipped = !_isFlipped;
            // Lock card interaction during flip
            _cardButton.interactable = false;

            StartCoroutine(RotateCard(_isFlipped));
            GameManager.Instance.CardFlipped(this);

        }
    }
    public void MarkAsMatched()
    {
        _isMatched = true;
        _cardButton.interactable = false;
    }

    public void ResetCard()
    {
        _isFlipped = false;
        _isMatched = false;

        StartCoroutine(RotateCard(_isFlipped, true));
    }

    public IEnumerator RotateCard(bool flipped, bool isReset = false, float duration = 0.25f)
    {
        _isFlipping = true;
        float time = 0f;

        Quaternion frontStart = _cardFrontFace.transform.localRotation;
        Quaternion backStart = _cardBackFace.transform.localRotation;

        Quaternion frontEnd = frontStart * Quaternion.Euler(0f, 180f, 0f);
        Quaternion backEnd = backStart * Quaternion.Euler(0f, 180f, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            _cardFrontFace.transform.localRotation = Quaternion.Slerp(frontStart, frontEnd, t);
            _cardBackFace.transform.localRotation = Quaternion.Slerp(backStart, backEnd, t);

            yield return null;
        }

        _cardFrontFace.transform.localRotation = frontEnd;
        _cardBackFace.transform.localRotation = backEnd;

        _isFlipping = false;

        // Reactivate button only if card is not matched
        if (!_isMatched)
            _cardButton.interactable = true;
    }
}