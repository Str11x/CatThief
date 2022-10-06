using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        FieldOfViewCalculate.GameIsLost += StopAnimations;
        Exit.LevelCompleted += StopAnimations;
    }

    private void OnDisable()
    {
        FieldOfViewCalculate.GameIsLost -= StopAnimations;
        Exit.LevelCompleted -= StopAnimations;
    }

    private void StopAnimations()
    {
        _animator.speed = 0;
    }
}