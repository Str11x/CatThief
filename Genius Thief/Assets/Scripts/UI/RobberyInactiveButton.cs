using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobberyInactiveButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        PlayerMovementAgent.StartedRobbery += DisableButton;
        _button = GetComponent<Button>();
    }

    private void OnDestroy()
    {
        PlayerMovementAgent.StartedRobbery -= DisableButton;
    }

    private void DisableButton()
    {
        _button.enabled = false;
    }
}