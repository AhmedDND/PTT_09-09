using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public Difficulty CurrentDifficulty { get; private set; } = Difficulty.EASY;

    [Min(2)] public int Rows = 2;
    [Min(2)] public int Columns = 2;

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

    public void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        Debug.Log("Difficulty set to: " + difficulty);
    }
}