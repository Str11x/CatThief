using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RobberyInactiveButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        PlayerMovementAgent.RobberyStarted += DisableButton;
        _button = GetComponent<Button>();
    }

    private void OnDestroy()
    {
        PlayerMovementAgent.RobberyStarted -= DisableButton;
    }

    private void DisableButton()
    {
        _button.enabled = false;
    }
}