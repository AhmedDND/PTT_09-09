using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int _turnsTaken;
    private int _currentCombo;
    private int _totalScore;
    private int _baseScoreEarned = 100;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _turnsText;
    [SerializeField] private TMP_Text _highScoreText;

    public int TotalScore => _totalScore;
    public int TurnsTaken => _turnsTaken;
    public int CurrentCombo => _currentCombo;

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

    private void Start()
    {
        UpdateUI();
    }

    public void ResetScore()
    {
        _turnsTaken = 0;
        _currentCombo = 0;
        _totalScore = 0;
        UpdateUI();
    }

    public void RegisterTurn()
    {
        _turnsTaken++;
        UpdateUI();
    }

    public void ResetCombo()
    {
        _currentCombo = 0;
    }

    public void RegisterMatch(float roundTime)
    {
        _currentCombo++;

        // Combo multiplier
        float comboMultiplier = 1f + (_currentCombo - 1) * 0.5f;

        // Difficulty multiplier
        float difficultyMultiplier = 1f;
        switch (GameSettings.Instance.CurrentDifficulty)
        {
            case Difficulty.EASY: difficultyMultiplier = 1f; break;
            case Difficulty.NORMAL: difficultyMultiplier = 1.5f; break;
            case Difficulty.HARD: difficultyMultiplier = 2f; break;
        }

        int neededPairs = (GameSettings.Instance.Rows * GameSettings.Instance.Columns) / 2;
        int penaltyStep = Mathf.Max(1, neededPairs / 2);

        int steps = _turnsTaken / penaltyStep;

        float turnPenalty = 1f - (steps * 0.1f);

        // Cap at 50% minimum
        turnPenalty = Mathf.Max(turnPenalty, 0.5f);

        float timePenalty = 1f - (roundTime / 60f) * 0.1f;

        // Also cap time penalty at 50%
        timePenalty = Mathf.Max(timePenalty, 0.5f);

        int pointsEarned = Mathf.RoundToInt(_baseScoreEarned * comboMultiplier * difficultyMultiplier * timePenalty * turnPenalty);
        _totalScore += Mathf.Max(pointsEarned, 5);

        UpdateUI();
    }

    public void SaveHighScore(string category, int rows, int cols, Difficulty difficulty)
    {
        string key = $"{category}_{rows}x{cols}_{difficulty}";
        int bestScore = PlayerPrefs.GetInt(key, 0);
        int newHigh = Mathf.Max(bestScore, _totalScore);

        PlayerPrefs.SetInt(key, newHigh);
        PlayerPrefs.Save();

        Debug.Log($"Highscore saved! {key} = {newHigh}");
    }

    public int GetHighScore(string category, int rows, int cols, Difficulty difficulty, int currentScore)
    {
        string key = $"{category}_{rows}x{cols}_{difficulty}";

        // Check if we already have a high score saved
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            // If first time playing this round, save current score as the high score
            PlayerPrefs.SetInt(key, currentScore);
            PlayerPrefs.Save();
            return currentScore;
        }
    }

    public void SetScoreFromSaveFile(int totalScore, int turnsTaken, int currentCombo)
    {
        _totalScore = totalScore;
        _turnsTaken = turnsTaken;
        _currentCombo = currentCombo;
        UpdateUI();
    }

    private void UpdateHighScoreUI()
    {
        if (_highScoreText == null) return;

        string category = GameSettings.Instance.GetCategory().name;
        int rows = GameSettings.Instance.Rows;
        int cols = GameSettings.Instance.Columns;
        Difficulty difficulty = GameSettings.Instance.CurrentDifficulty;

        string key = $"{category}_{rows}x{cols}_{difficulty}";
        int savedHighScore = PlayerPrefs.GetInt(key, 0);

        int highScore = Mathf.Max(_totalScore, savedHighScore);

        _highScoreText.text = $"High Score: {highScore}";
    }

    public void UpdateUI()
    {
        if (_scoreText != null)
            _scoreText.text = $"Score: {_totalScore}";
        if (_turnsText != null)
            _turnsText.text = $"Turns: {_turnsTaken}";

        UpdateHighScoreUI();
    }
}
