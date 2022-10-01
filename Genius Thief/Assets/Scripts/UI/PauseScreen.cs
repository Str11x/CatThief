using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Image _screen;

    private Image _currentScreen;

    private void Awake()
    {
        FieldOfViewCalculate.GameIsLost += ChangePanelState;

        _currentScreen = _screen;
    }

    private void OnDisable()
    {
        FieldOfViewCalculate.GameIsLost -= ChangePanelState;
    }

    public void ChangePanelState()
    {
        if (_currentScreen.enabled == false)
            _currentScreen.enabled = true;
        else
            _currentScreen.enabled = false;        
    }
}