using UnityEngine;
using UnityEngine.UI;

public class Suite : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool active)
    {
        gameObject.SetActive(active);
    }
}