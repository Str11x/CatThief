using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordSaver : MonoBehaviour
{
    [SerializeField] private WinnerScreenScore _screenScore;
    [SerializeField] private LevelMenu _levelMenu;

    private string _sceneName;

    private void Awake()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        _screenScore.CalculatedScoreResult += SaveNewLevelScore;
    }

    private void OnDestroy()
    {
        _screenScore.CalculatedScoreResult -= SaveNewLevelScore;
    }

    private void SaveNewLevelScore(int starsScore)
    {
        if(PlayerPrefs.GetInt(_sceneName) < starsScore)
            PlayerPrefs.SetInt(_sceneName, starsScore);
    }

    public int GetLevelScore(string sceneName)
    {
        return PlayerPrefs.GetInt(sceneName);
    }
}