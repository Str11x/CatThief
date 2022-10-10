using UnityEngine;

public class PlayerSuite : MonoBehaviour
{
    [SerializeField] Suite _defaultSuite;

    private Suite [] _allSuites;
    private string _isSkinSaved;
    private string _currentSkin = "CurrentSkin";

    private int _skinSaved = 1;
    private int _skinEmpty = 0;

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

    public void UpdateCurrentSkin(string name)
    {
        PlayerPrefs.SetString(_currentSkin, name);
        PlayerPrefs.SetInt(_isSkinSaved, _skinSaved);
    }
}