using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScoreBag : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    private Animator _animator;
    private int _shakeBag = Animator.StringToHash("PickedUpLoot");

    private void Start()
    {
        _wallet.AddedCoin += ShakeBag;
        FieldOfViewCalculate.GameIsLost += DisableRenderer;
        Exit.LevelCompleted += DisableRenderer;

        _animator = GetComponent<Animator>();
    }

    private void ShakeBag()
    {
        _animator.SetTrigger(_shakeBag);
    }

    private void OnDisable()
    {
        _wallet.AddedCoin -= ShakeBag;
        FieldOfViewCalculate.GameIsLost -= DisableRenderer;
        Exit.LevelCompleted -= DisableRenderer;
    }

    private void DisableRenderer()
    {
        gameObject.SetActive(false);
    }
}