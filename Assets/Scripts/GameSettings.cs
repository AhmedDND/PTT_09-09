using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public CardsCollectionSO SelectedCategory { get; private set; }

    public Difficulty CurrentDifficulty { get; private set; } = Difficulty.EASY;

    [Min(2)] public int Rows = 2;
    [Min(2)] public int Columns = 2;

    [Header("Card Backgrounds per Difficulty")]
    public Sprite EasyCardBackground;
    public Sprite NormalCardBackground;
    public Sprite HardCardBackground;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCategory(CardsCollectionSO category)
    {
        SelectedCategory = category;
        Debug.Log($"Category set to: {category.name}");
    }

    public CardsCollectionSO GetCategory()
    {
        return SelectedCategory;
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        Debug.Log("Difficulty set to: " + difficulty);
    }

    public Sprite GetCardBackground()
    {
        switch (CurrentDifficulty)
        {
            case Difficulty.EASY:
                return EasyCardBackground;
            case Difficulty.NORMAL:
                return NormalCardBackground;
            case Difficulty.HARD:
                return HardCardBackground;
            default:
                return EasyCardBackground;
        }
    }
}