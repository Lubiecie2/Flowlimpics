using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _targetsText;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gamePanel;

    [Header("End Game UI")]
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private TextMeshProUGUI _endMessageText;
    [SerializeField] private TextMeshProUGUI _endTimeText;
    [SerializeField] private TextMeshProUGUI _endScoreText;

    [Header("Timer Settings")]
    [SerializeField] private bool _useTimer = true;

    private int _currentScore = 0;
    private float _elapsedTime = 0f;
    private bool _gameStarted = false;
    private int _targetsHit = 0;
    private int _totalTargets = 0;

    private void Start()
    {
        _totalTargets = FindObjectsOfType<TargetHit>().Length;
        Debug.Log($"Znaleziono {_totalTargets} tarcz na mapie");

        UpdateScoreUI();
        UpdateTimerUI();
        UpdateTargetsUI();

        if (_startPanel != null)
        {
            _startPanel.SetActive(true);
        }

        if (_gamePanel != null)
        {
            _gamePanel.SetActive(false);
        }

        if (_endGamePanel != null)
        {
            _endGamePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_gameStarted || !_useTimer) return;

        _elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    public void StartGame()
    {
        if (_gameStarted) return;

        _gameStarted = true;
        _elapsedTime = 0f;

        Debug.Log("GRA ROZPOCZĘTA!");

        if (_startPanel != null)
        {
            _startPanel.SetActive(false);
        }

        if (_gamePanel != null)
        {
            _gamePanel.SetActive(true);
        }
    }

    public void AddPoints(int points)
    {
        if (!_gameStarted) return;

        _currentScore += points;
        _targetsHit++;

        Debug.Log($"{points} pkt! Tarcze: {_targetsHit}/{_totalTargets}");

        UpdateScoreUI();
        UpdateTargetsUI();

        if (_targetsHit >= _totalTargets)
        {
            Victory();
        }
    }

    private void UpdateScoreUI()
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Punkty: {_currentScore}";
        }
    }

    private void UpdateTimerUI()
    {
        if (_timerText != null)
        {
            int minutes = Mathf.FloorToInt(_elapsedTime / 60);
            int seconds = Mathf.FloorToInt(_elapsedTime % 60);
            _timerText.text = $"Czas: {minutes:00}:{seconds:00}";
        }
    }

    private void UpdateTargetsUI()
    {
        if (_targetsText != null)
        {
            _targetsText.text = $"Tarcze: {_targetsHit}/{_totalTargets}";
        }
    }

    private void Victory()
    {
        _gameStarted = false;

        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);

        Debug.Log($"WYGRANA! Wszystkie tarcze trafione! Czas: {minutes:00}:{seconds:00}");

        if (_scoreText != null)
        {
            _scoreText.gameObject.SetActive(false);
        }

        if (_timerText != null)
        {
            _timerText.gameObject.SetActive(false);
        }

        if (_targetsText != null)
        {
            _targetsText.gameObject.SetActive(false);
        }


        if (_gamePanel != null)
        {
            _gamePanel.SetActive(false);
        }

        if (_endGamePanel != null)
        {
            _endGamePanel.SetActive(true);

            if (_endMessageText != null)
            {
                _endMessageText.text = "Gratulacje!\nUkończyłeś grę!";
            }

            if (_endTimeText != null)
            {
                _endTimeText.text = $"Czas: {minutes:00}:{seconds:00}";
            }

            if (_endScoreText != null)
            {
                _endScoreText.text = $"Punkty: {_currentScore}";
            }
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public bool IsGameStarted()
    {
        return _gameStarted;
    }
}