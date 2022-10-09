using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private const string LevelComplete = "LevelComplete";
    private const string ShowWinScreen = "ShowWinnerScreen";
    private const string ShowZeroLootScreen = "ShowNoLootScreen";

    [SerializeField] private Player _player;
    [SerializeField] private TimeService _timeService;
    [SerializeField] private LevelComplete _levelCompleteText;
    [SerializeField] private GameOverScreen _gameOverText;
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _gameOverDescription;
    [SerializeField] private TMP_Text _noLootDescription;

    private int _timeDelay = 1;
    private int _levelComplete;
    private float _screenDelay = 0.75f;
    
    public int SceneIndex { get; private set; }

    public event Action LevelFailed;
    public event Action LevelDone;

    private void Awake()
    {
        Exit.LevelCompleted += LevelCompleted;
        FieldOfViewCalculate.GameIsLost += ShowGameOverScreen;

        if (SceneManager.GetActiveScene().buildIndex == 0)
            PlayerPrefs.DeleteAll();

        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        _levelComplete = PlayerPrefs.GetInt(LevelComplete);
    }

    private void OnDisable()
    {
        Exit.LevelCompleted -= LevelCompleted;
        FieldOfViewCalculate.GameIsLost -= ShowGameOverScreen;
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt(LevelComplete, SceneIndex);                      

        if (_player.GetBalance() == 0)
            Invoke(ShowZeroLootScreen, _screenDelay);
        else 
            Invoke(ShowWinScreen, _screenDelay);
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