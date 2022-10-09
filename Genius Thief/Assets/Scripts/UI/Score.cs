using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _result;

    private int _initialValue = 0;

    private void Start()
    {
        FieldOfViewCalculate.GameIsLost += DisableRenderer;
        Exit.LevelCompleted += DisableRenderer;

        _wallet.AddedCoin += UpdateScore;
        _result.text = _initialValue.ToString();
    }

    private void OnDisable()
    {
        FieldOfViewCalculate.GameIsLost -= DisableRenderer;
        Exit.LevelCompleted -= DisableRenderer;
        _wallet.AddedCoin -= UpdateScore;
    }

    private void UpdateScore()
    {
        _result.text = _wallet.PointsCount.ToString();
    }

    private void DisableRenderer()
    {
        gameObject.SetActive(false);
    }
}