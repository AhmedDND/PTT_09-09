using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _gameOverText;

    private float _roundTime;
    private bool _roundInProgress;

    public float RoundTime => _roundTime;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (_roundInProgress)
        {
            _roundTime += Time.deltaTime;
            _timeText.text = GetTimeString();
        }
    }

    private string GetTimeString()
    {
        int min = Mathf.FloorToInt(_roundTime / 60.0f);
        int sec = Mathf.FloorToInt(_roundTime % 60);

        return $"{GetNumberWithLeadingZero(min)}:{GetNumberWithLeadingZero(sec)}";
    }

    private string GetNumberWithLeadingZero(int num)
    {
        return num < 10 ? "0" + num.ToString() : num.ToString();
    }

    public void SetEndGameText()
    {
        _gameOverText.text = $"You finished in {GetTimeString()}!";
    }

    public void StartTimer()
    {
        ResetTimer();
        _roundInProgress = true;
    }

    public void PauseTimer()
    {
        _roundInProgress = false;
    }

    public void ResumeTimer()
    {
        _roundInProgress = true;
    }

    public void ResetTimer()
    {
        _roundTime = 0f;
        _timeText.text = "00:00";
        _roundInProgress = false;
    }
}
