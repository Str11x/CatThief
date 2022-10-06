using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TimeService _timeService;
    [SerializeField] private LevelComplete _levelCompleteText;
    [SerializeField] private GameOverScreen _gameOverText;
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _gameOverDescription;
    [SerializeField] private TMP_Text _noLootDescription;

    private int _timeDelay = 1;
    private int _levelComplete;
    private int _lastLevelIndex = 4;
    private int _titlesScene = 5;
    private float _screenDelay = 0.75f;
    private string _levelDone = "LevelComplete";
    private string _showWinnerScreen = "ShowWinnerScreen";
    private string _showNoLootScreen = "ShowNoLootScreen";

    public int SceneIndex { get; private set; }

    public event Action LevelFailed;
    public event Action LevelDone;

    private void Awake()
    {
        Exit.LevelCompleted += LevelCompleted;
        FieldOfViewCalculate.GameIsLost += ShowGameOverScreen;

        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        _levelComplete = PlayerPrefs.GetInt(_levelDone);
    }

    private void OnDisable()
    {
        Exit.LevelCompleted -= LevelCompleted;
        FieldOfViewCalculate.GameIsLost -= ShowGameOverScreen;
    }

    private void EndGame()
    {
        if (SceneIndex == _lastLevelIndex)
            SceneManager.LoadScene(_titlesScene);             
        else if (_levelComplete < SceneIndex)
            PlayerPrefs.SetInt(_levelDone, SceneIndex);

        if (_player.GetBalance() == 0)
            Invoke(_showNoLootScreen, _screenDelay);
        else 
            Invoke(_showWinnerScreen, _screenDelay);
    }

    public void ShowWinnerScreen()
    {
        LevelDone?.Invoke();
        _menuButton.gameObject.SetActive(false);
        _levelCompleteText.gameObject.SetActive(true);
    }

    public void LoadTo(int level)
    {
        _timeService.ChangePauseTime();
        SceneManager.LoadScene(level);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneIndex + 1);
    }

    private void LevelCompleted()
    {
        _timeService.ChangePauseTime(_timeDelay);
        EndGame();
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneIndex);
    }

    private void ShowGameOverScreen()
    {
        _gameOverDescription.enabled = true;
        _menuButton.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(true);
    }

    private void ShowNoLootScreen()
    {
        LevelFailed?.Invoke();
        _noLootDescription.enabled = true;
        _menuButton.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(true);
    }
}