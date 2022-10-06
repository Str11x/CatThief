using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinShop : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _playerSkinSize = 2.5f;   
    [SerializeField] private int _rightRotateStep = -20;
    [SerializeField] private int _leftRotateStep = 20;

    private float _xPosition = -7;
    private float _yPosition = 5;
    private float _zPosition = -7;
    private Vector3 _shopPosition;
    private Vector3 _playerSkinScale;
    private Vector3 _lastPosition;
    private Vector3 _lastScale;
    private Vector3 _rightRotation;
    private Vector3 _leftRotation;
    private Vector3 _cameraOffset = new Vector3(-20, 18, -20);
    private Quaternion _lastRotation;

    private void Awake()
    {
        _rightRotation = new Vector3(0, _rightRotateStep, 0);
        _leftRotation = new Vector3(0, _leftRotateStep, 0);

        _shopPosition = new Vector3(_xPosition, _yPosition, _zPosition);
        _playerSkinScale = new Vector3(_playerSkinSize, _playerSkinSize, _playerSkinSize);
    }

    private void OnEnable()
    {
        _lastScale = _player.transform.localScale;
        _lastPosition = _player.transform.position;

        _player.transform.localScale = _playerSkinScale;
        _player.transform.position = _shopPosition;

        _player.transform.LookAt(Camera.main.transform.position + _cameraOffset);
    }

    private void OnDisable()
    {
        _player.transform.position = _lastPosition;
        _player.transform.localScale = _lastScale;
        _player.transform.rotation = _lastRotation;
    }

    public void RotateRight()
    {
        _player.transform.Rotate(_rightRotation);
    }

    public void RotateLeft()
    {
        _player.transform.Rotate(_leftRotation);
    }
}