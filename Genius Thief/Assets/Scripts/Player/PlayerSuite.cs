using UnityEngine;

public class PlayerSuite : MonoBehaviour
{
    [SerializeField] Suite _defaultSuite;

    private Suite [] _Suites;
    private string _skinPresence;
    private string _currentSkin = "CurrentSkin";

    private int _saveSkin = 1;
    private int _skinEmpty = 0;

    private void Start()
    {
        _Suites = GetComponentsInChildren<Suite>();

        if(PlayerPrefs.GetInt(_skinPresence) == _skinEmpty)
        {
            foreach (Suite suite in _Suites)
            {
                if (suite != _defaultSuite)
                    suite.gameObject.SetActive(false);                       
            }
        }
        else
        {
            string skinName = PlayerPrefs.GetString(_currentSkin);

            foreach (Suite suite in _Suites)
            {
                if (suite.gameObject.name != skinName)
                    suite.gameObject.SetActive(false);
                else
                    suite.gameObject.SetActive(true);
            }
        }        
    }

    public void UpdateCurrentSkin(string name)
    {
        PlayerPrefs.SetString(_currentSkin, name);
        PlayerPrefs.SetInt(_skinPresence, _saveSkin);
    }
}