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
    private bool _isMatched = false;

    public CardSO CardData => _cardData;
    public bool IsMatched => _isMatched;

    public void Initialize(CardSO cardData, Sprite cardBack)
    {
        _cardData = cardData;
        _cardFrontFace.sprite = cardData.CardFrontFace;

        // WIP: Fix so that it grabs the sprite from game difficulty
        _cardBackFace.sprite = cardBack;

    }

    public void FlipCard()
    {
        if (_isMatched || GameManager.Instance.IsProcessing) return;
        if (_isFlipped) return;

        _isFlipped = true;
        // Lock card interaction during flip
        _cardButton.interactable = false;

        StartCoroutine(RotateCard(transform, true));
        GameManager.Instance.CardFlipped(this);
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

        StartCoroutine(RotateCard(transform, false));
    }

    public IEnumerator RotateCard(Transform target, bool flipped, float duration = 0.25f)
    {
        float time = 0f;

        Quaternion startRot = target.rotation;
        Quaternion endRot = Quaternion.Euler(0f, flipped ? 0f : 180f, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            target.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        target.rotation = endRot;

        // Enable button only when the rotation is finished
        if (!_isMatched)
        {
            _cardButton.interactable = true;
        }
    }
}