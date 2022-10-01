using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeService : MonoBehaviour
{
    [SerializeField] private PauseScreen _pauseScreen;
    
    private WaitForSeconds _oneSecondDelay = new WaitForSeconds(1f);
    private Coroutine _pauseWithDelay;  
    private int _stopTime = 0;
    private int _startTime = 1;

    public bool IsInteractWithMenu { get; private set; }
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

    private IEnumerator PauseWithDelay(int delay)
    {
        int startTime = 0;

        while (startTime != delay)
        {
            startTime++;
            yield return _oneSecondDelay;
        }

        _pauseScreen.gameObject.GetComponent<Image>().enabled = true;
    }
}