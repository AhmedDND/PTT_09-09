using UnityEngine;
using UnityEngine.UI;

public class CardGrid : LayoutGroup
{
    [SerializeField] private int _rows = 2;
    [SerializeField] private int _columns = 2;

    [SerializeField] private Vector2 _spacing;
    [SerializeField] private int _topPadding;

    private Vector2 _cardSize;
    public Vector2 CardSize => _cardSize;

    // Added validation to prevent invalid values and division by zero
    protected override void OnValidate()
    {
        base.OnValidate();
        _rows = Mathf.Max(2, _rows);
        _columns = Mathf.Max(2, _columns);
    }

    public override void CalculateLayoutInputVertical()
    {
        // Handle division by 0 case
        if (_rows == 0 || _columns == 0)
        {
            // Set to default difficulty
            Debug.LogWarning("Rows and columns must be positive values. Using defaults.");
            _rows = 2;
            _columns = 2;
        }

        // Calculate the card size relative to the rectTransform
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cardHeight = (parentHeight - 2 * _topPadding - _spacing.y * (_rows - 1)) / _rows;
        float cardWidth = cardHeight;

        // Handle different screen Aspect Ratios
        if (cardWidth * _columns + _spacing.x * (_columns - 1) > parentWidth)
        {
            cardWidth = (parentWidth - 2 * _topPadding - (_columns - 1) * _spacing.x) / _columns;
            cardHeight = cardWidth;
        }

        _cardSize = new Vector2(cardWidth, cardHeight);

        padding.left = Mathf.FloorToInt((parentWidth - _columns * cardWidth - _spacing.x * (_columns - 1)) / 2);
        padding.top = Mathf.FloorToInt((parentHeight - _rows * cardHeight - _spacing.y * (_rows - 1)) / 2);
        padding.bottom = padding.top;

        // Place card in the correct position
        for (int i = 0; i < rectChildren.Count; i++)
        {
            int rowCount = i / _columns;
            int columnCount = i % _columns;
            
            RectTransform item = rectChildren[i];

            float xPosition = padding.left + _cardSize.x * columnCount + _spacing.x * (columnCount);
            float yPosition = padding.top + _cardSize.y * rowCount + _spacing.y * (rowCount);

            SetChildAlongAxis(item, 0, xPosition, _cardSize.x);
            SetChildAlongAxis(item, 1, yPosition, _cardSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {
        return;
    }

    public override void SetLayoutVertical()
    {
        return;
    }
}
