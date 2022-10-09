using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeService : MonoBehaviour
{
    [SerializeField] private PauseScreen _pauseScreen;

    private Image _pauseImage;
    private WaitForSeconds _oneSecondDelay = new WaitForSeconds(1f);
    private Coroutine _pauseWithDelay;  
    private int _stopTime = 0;
    private int _startTime = 1;

    public bool IsInteractWithMenu { get; private set; }

    private void Awake()
    {
        _pauseImage = _pauseScreen.gameObject.GetComponent<Image>();

        if (SceneManager.GetActiveScene().buildIndex == 0 && Time.timeScale == _startTime)
            ChangePauseTime();
    }

    private IEnumerator PauseWithDelay(int delay)
    {
        int startTime = 0;

        while (startTime != delay)
        {
            startTime++;
            yield return _oneSecondDelay;
        }

        _pauseImage.enabled = true;
    }

    public void ChangePauseTime()
    {
        if (Time.timeScale == _stopTime)
        {
            Time.timeScale = _startTime;
            IsInteractWithMenu = false;
        }
        else
        {
            Time.timeScale = _stopTime;
            IsInteractWithMenu = true;
        }
    }

    public void ChangePauseTime(int delay)
    {
        if (_pauseWithDelay == null)
        {
            StartCoroutine(PauseWithDelay(delay));
        }
        else
        {
            StopCoroutine(_pauseWithDelay);
            StartCoroutine(PauseWithDelay(delay));
        }
    }
}