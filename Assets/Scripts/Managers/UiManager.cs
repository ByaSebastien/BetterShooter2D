using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-90)]
public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    private GameObject _mainMenuPanel;
    private GameObject _gamePanel;
    private GameObject _pausePanel;
    private GameObject _gameOverPanel;

    private Slider _playerHealthSlider;
    private TMP_Text _scoreText;
    private TMP_Text _waveText;

    private TextMeshProUGUI _finalScoreText;
    private TextMeshProUGUI _highScoreText;
    private TextMeshProUGUI _weaponText;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (GameManager.Instance)
        {
            GameManager.Instance.AddStateListener(UpdateUIForGameState);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (GameManager.Instance)
        {
            GameManager.Instance.RemoveStateListener(UpdateUIForGameState);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUiReferences();
        SetupAllButtons();
        UpdateUIForGameState(GameManager.Instance.GameState);
    }

    private void FindUiReferences()
    {
        _mainMenuPanel = GameObject.FindGameObjectWithTag("MainMenuPanel");
        _gamePanel = GameObject.FindGameObjectWithTag("GamePanel");
        _pausePanel = GameObject.FindGameObjectWithTag("PausePanel");
        _gameOverPanel = GameObject.FindGameObjectWithTag("GameOverPanel");

        GameObject healthSliderObj = GameObject.FindGameObjectWithTag("PlayerHealthSlider");
        if (healthSliderObj) _playerHealthSlider = healthSliderObj.GetComponent<Slider>();

        GameObject scoreTextObj = GameObject.FindGameObjectWithTag("ScoreText");
        if (scoreTextObj) _scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();

        GameObject waveTextObj = GameObject.FindGameObjectWithTag("WaveText");
        if (waveTextObj) _waveText = waveTextObj.GetComponent<TextMeshProUGUI>();

        GameObject finalScoreTextObj = GameObject.FindGameObjectWithTag("FinalScoreText");
        if (finalScoreTextObj) _finalScoreText = finalScoreTextObj.GetComponent<TextMeshProUGUI>();

        GameObject highScoreTextObj = GameObject.FindGameObjectWithTag("HighScoreText");
        if (highScoreTextObj) _highScoreText = highScoreTextObj.GetComponent<TextMeshProUGUI>();
        
        GameObject weaponTextObj = GameObject.FindGameObjectWithTag("WeaponText");
        if (weaponTextObj) _weaponText = weaponTextObj.GetComponent<TextMeshProUGUI>();
    }

    private void SetupAllButtons()
    {
        if (!GameManager.Instance) return;

        SetupButtonWithTag("PlayButton", GameManager.Instance.StartGame);
        SetupButtonWithTag("QuitButton", GameManager.Instance.QuitGame);
        SetupButtonWithTag("ResumeButton", GameManager.Instance.ResumeGame);
        SetupButtonWithTag("MainMenuButton", GameManager.Instance.ReturnToMainMenu);
        SetupButtonWithTag("RetryButton", GameManager.Instance.StartGame);
        SetupButtonWithTag("PauseButton", GameManager.Instance.PauseGame);
    }

    private void SetupButtonWithTag(string buttonTag, UnityEngine.Events.UnityAction action)
    {
        GameObject[] buttonObjs = GameObject.FindGameObjectsWithTag(buttonTag);
        foreach (GameObject buttonObj in buttonObjs)
        {
            if (!buttonObj) continue;
            Button button = buttonObj.GetComponent<Button>();
            if (!button) continue;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }


    private void UpdateUIForGameState(GameState state)
    {
        if (_mainMenuPanel) _mainMenuPanel.SetActive(false);
        if (_gamePanel) _gamePanel.SetActive(false);
        if (_pausePanel) _pausePanel.SetActive(false);
        if (_gameOverPanel) _gameOverPanel.SetActive(false);

        switch (state)
        {
            case GameState.MainMenu:
                if (_mainMenuPanel) _mainMenuPanel.SetActive(true);
                break;
            case GameState.Playing:
                if (_gamePanel) _gamePanel.SetActive(true);
                break;
            case GameState.Paused:
                if (_gamePanel) _gamePanel.SetActive(true);
                if (_pausePanel) _pausePanel.SetActive(true);
                break;
            case GameState.GameOver:
                if (_gamePanel) _gamePanel.SetActive(true);
                if (_gameOverPanel) _gameOverPanel.SetActive(true);
                break;
        }
    }

    public void UpdatePlayerHealth(float currentHealth, float maxHealth)
    {
        if (_playerHealthSlider)
        {
            _playerHealthSlider.value = currentHealth / maxHealth;
        }
    }

    public void UpdateScoreText(int score)
    {
        if (_scoreText) _scoreText.text = $"Score: {score}";
    }

    public void UpdateWeaponText(string weaponName)
    {
        if (_weaponText) _weaponText.text = weaponName;
    }

    public void UpdateWaveText(int waveNumber)
    {
        if (_waveText) _waveText.text = $"Wave: {waveNumber}";
    }

    public void ShowGameOverScreen(int finalScore, bool isNewHighScore)
    {
        if (_gameOverPanel)
        {
            if (_finalScoreText) _finalScoreText.text = $"{(isNewHighScore ? "New" : "Final")} Score: {finalScore}";
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (_highScoreText) _highScoreText.text = $"High Score: {highScore}";
        }
    }
}