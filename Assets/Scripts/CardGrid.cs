using UnityEngine;
using UnityEngine.UI;

public class CardGrid : LayoutGroup
{
    [SerializeField] private Vector2 _spacing;
    [SerializeField] private int _topPadding;

    private Vector2 _cardSize;
    public Vector2 CardSize => _cardSize;

    private GameSettings _gameSettings;

    protected override void Start()
    {
        _gameSettings = GameSettings.Instance.GetComponent<GameSettings>();
    }

    public override void CalculateLayoutInputVertical()
    {
        // Handle division by 0 case
        if (_gameSettings.Rows == 0 || _gameSettings.Columns == 0)
        {
            // Set to default difficulty
            Debug.LogWarning("Rows and columns must be positive values. Using defaults.");
            _gameSettings.Rows = 2;
            _gameSettings.Columns = 2;
        }

        // Calculate the card size relative to the rectTransform
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cardHeight = (parentHeight - 2 * _topPadding - _spacing.y * (_gameSettings.Rows - 1)) / _gameSettings.Rows;
        float cardWidth = cardHeight;

        // Handle different screen Aspect Ratios
        if (cardWidth * _gameSettings.Columns + _spacing.x * (_gameSettings.Columns - 1) > parentWidth)
        {
            cardWidth = (parentWidth - 2 * _topPadding - (_gameSettings.Columns - 1) * _spacing.x) / _gameSettings.Columns;
            cardHeight = cardWidth;
        }

        _cardSize = new Vector2(cardWidth, cardHeight);

        padding.left = Mathf.FloorToInt((parentWidth - _gameSettings.Columns * cardWidth - _spacing.x * (_gameSettings.Columns - 1)) / 2);
        padding.top = Mathf.FloorToInt((parentHeight - _gameSettings.Rows * cardHeight - _spacing.y * (_gameSettings.Rows - 1)) / 2);
        padding.bottom = padding.top;

        // Place card in the correct position
        for (int i = 0; i < rectChildren.Count; i++)
        {
            int rowCount = i / _gameSettings.Columns;
            int columnCount = i % _gameSettings.Columns;
            
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
