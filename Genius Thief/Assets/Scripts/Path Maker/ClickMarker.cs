using TMPro;
using UnityEngine;

public class ClickMarker : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI _stepRenderer;

    public void AddStep(int stepNumber)
    {
        _stepRenderer.text = stepNumber.ToString();
    }
}