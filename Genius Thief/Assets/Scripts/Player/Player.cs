using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Suite _defaultSuite;
    [SerializeField] Wallet _wallet;

    private Suite [] _allSuites;
    private string _isSkinSaved;
    private string _currentSkin = "CurrentSkin";

    private int _skinSaved = 1;
    private int _skinEmpty = 0;

    public int MarkersCount { get; private set; }

    public event Action StartedMovement;
    public event Action TouchedMarker;

    private void Start()
    {
        _allSuites = GetComponentsInChildren<Suite>();

        if(PlayerPrefs.GetInt(_isSkinSaved) == _skinEmpty)
        {
            foreach (Suite suite in _allSuites)
            {
                if (suite != _defaultSuite)
                    suite.gameObject.SetActive(false);                       
            }
        }
        else
        {
            string skinName = PlayerPrefs.GetString(_currentSkin);

            foreach (Suite suite in _allSuites)
            {
                if (suite.gameObject.name != skinName)
                    suite.gameObject.SetActive(false);
                else
                    suite.gameObject.SetActive(true);
            }
        }        
    }

    public void AddCount()
    {
        _wallet.AddPoints();
    }

    public int GetBalance()
    {
        return _wallet.PointsCount;
    }

    public void AddMarkerCount()
    {
        MarkersCount++;
        TouchedMarker?.Invoke();
    }

    public void StartAnimation()
    {
        StartedMovement?.Invoke();
    }

    public void UpdateCurrentSkin(string name)
    {
        PlayerPrefs.SetString(_currentSkin, name);
        PlayerPrefs.SetInt(_isSkinSaved, _skinSaved);
    }
}