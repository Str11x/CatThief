using UnityEngine;
using UnityEngine.UI;

public class Suite : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    
    private PlayerSuite _player;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener(OnToggleChanged);
        _player = GetComponentInParent<PlayerSuite>();
    }

    private void OnToggleChanged(bool active)
    {
        gameObject.SetActive(active);
        _player.UpdateCurrentSkin(gameObject.name);
    }
}