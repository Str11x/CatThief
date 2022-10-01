using UnityEngine;
using UnityEngine.UI;

public class SkinShop : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _playerSkinSize = 2.5f;
    [SerializeField] private float _xPosition = -7;
    [SerializeField] private float _yPosition = 5;
    [SerializeField] private float _zPosition = -7;

    private Vector3 _shopPosition;
    private Vector3 _playerSkinScale;
    private Vector3 _lastPosition;
    private Vector3 _lastScale;

    private void Awake()
    {
        _shopPosition = new Vector3(_xPosition, _yPosition, _zPosition);
        _playerSkinScale = new Vector3(_playerSkinSize, _playerSkinSize, _playerSkinSize);
    }

    private void OnEnable()
    {
        _lastScale = _player.transform.localScale;
        _lastPosition = _player.transform.position;

        _player.transform.localScale = _playerSkinScale;       
        _player.transform.position = _shopPosition;
    }

    private void OnDisable()
    {
        _player.transform.position = _lastPosition;
        _player.transform.localScale = _lastScale;
    }
}