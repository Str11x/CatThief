using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private Player _player;

    private int _changeRobberyAnimation = Animator.StringToHash("ChangeRobberyAnimation");

    private void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        _player.StartedMovement += ChangeAnimation;
        Exit.LevelCompleted += ChangeAnimation;
        FieldOfViewCalculate.GameIsLost += ChangeAnimation;
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        Exit.LevelCompleted -= ChangeAnimation;
        _player.StartedMovement -= ChangeAnimation;
        FieldOfViewCalculate.GameIsLost -= ChangeAnimation;
    }

    private void ChangeAnimation()
    {
        _animator.SetTrigger(_changeRobberyAnimation);
    }
}