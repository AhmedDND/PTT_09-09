using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Card Prototype/Game Data")]
public class GameDataSO : ScriptableObject
{
    [Header("Round Settings")]
    public Difficulty Difficulty;
    public int Rows;
    public int Columns;

    [Header("Card Background Based on Difficulty")]
    public Sprite CardBackground;

    [Header("Grid Layout Variables")]
    public int TopBottomPadding;
    public Vector2 Spacing;
}
