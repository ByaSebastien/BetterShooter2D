using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameState _gameState;

    private int _currentScore;

    private event Action<GameState> GameStateChanged;

    public GameState GameState
    {
        get => _gameState;
        private set
        {
            _gameState = value;
            GameStateChanged?.Invoke(_gameState);
        }
    }

    public int CurrentScore
    {
        get => _currentScore;
        private set
        {
            _currentScore = value;
            if (UiManager.Instance) UiManager.Instance.UpdateScoreText(_currentScore);
        }
    }

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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            GameState = GameState.MainMenu;
        }
        else if (scene.name == "Game")
        {
            CurrentScore = 0;
            GameState = GameState.Playing;
            UiManager.Instance.UpdateScoreText(CurrentScore);
            //Todo Start wave
        }
    }

    public void StartGame()
    {
        if (GameState is GameState.GameOver)
        {
            Time.timeScale = 1f;
        }

        SceneManager.LoadScene("Game");
    }

    public void PauseGame()
    {
        if (GameState == GameState.Playing)
        {
            Time.timeScale = 0f;
            GameState = GameState.Paused;
        }
    }

    public void ResumeGame()
    {
        if (GameState == GameState.Paused)
        {
            Time.timeScale = 1f;
            GameState = GameState.Playing;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        GameState = GameState.GameOver;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        bool isNewHighScore = CurrentScore > highScore;
        
        if (UiManager.Instance) UiManager.Instance.ShowGameOverScreen(CurrentScore, isNewHighScore);
        
        if (isNewHighScore)
        {
            PlayerPrefs.SetInt("HighScore", CurrentScore);
            PlayerPrefs.Save();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                                    Application.Quit();
        #endif
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
    }

    public void AddStateListener(Action<GameState> action)
    {
        GameStateChanged += action;
    }

    public void RemoveStateListener(Action<GameState> action)
    {
        GameStateChanged -= action;
    }
}