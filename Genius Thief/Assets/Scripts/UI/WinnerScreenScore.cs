using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WinnerScreenScore : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _finalScore;
    [SerializeField] private Star _firstStar;
    [SerializeField] private Star _secondStar;
    [SerializeField] private Star _completelyStar;
    [SerializeField] private RewardObjects _allObjects;
    [SerializeField] private TMP_Text _fullCompleteText;

    private WaitForSeconds _waitTime = new WaitForSeconds(0.1f);
    private float almostCompleteMultiply = 0.75f;
    private Loot[] _allLoot;
    private int _starsScore;

    private Coroutine _renderScore;

    public event Action <int> CalculatedScoreResult;

    private void Awake()
    {
        _allLoot = _allObjects.GetComponentsInChildren<Loot>();
    }
    private void OnEnable()
    {
        if (_renderScore != null)
        {
            StopCoroutine(_renderScore);
            _renderScore = StartCoroutine(Render());
        }
        else
        {
            _renderScore = StartCoroutine(Render());
        }
    }

    private IEnumerator Render()
    {
        float finalCount = _wallet.PointsCount;      
        int newScore = 0;

        for (int i = 0; i <= finalCount; i++)
        {
            _finalScore.text = i.ToString();

            yield return _waitTime;

            newScore = i;
        }

        ShowStars(newScore);

        if (_completelyStar.gameObject.activeSelf == true)
            _fullCompleteText.gameObject.SetActive(true);

        CalculatedScoreResult?.Invoke(_starsScore);
    }

    private void ShowStars(int newScore)
    {
        float amountOfLoot = _allLoot.Length;
        int lowThresholdLoot = 0;

        if (newScore > lowThresholdLoot)
        {
            _starsScore++;
            _firstStar.gameObject.SetActive(true);         
        }
            
        if (newScore > amountOfLoot * almostCompleteMultiply)
        {
            _starsScore++;
            _secondStar.gameObject.SetActive(true);
        }
            
        if (newScore == amountOfLoot)
        {
            _starsScore++;
            _completelyStar.gameObject.SetActive(true);
        }
    }
}