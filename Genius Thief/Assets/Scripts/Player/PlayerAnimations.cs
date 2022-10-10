using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private PlayerAnimationState _playerState;

    private string _changeRobberyMovement = "ChangeRobberyMovement";

    private void Awake()
    {
        _playerState = GetComponentInParent<PlayerAnimationState>();              
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Exit.LevelCompleted += ChangeAnimation;
        _playerState.StartedMovement += ChangeAnimation;
        FieldOfViewCalculate.GameIsLost += ChangeAnimation;

        if (_playerState.IsRun == true)
            ChangeAnimation();
    }

    private void OnDisable()
    {
        Exit.LevelCompleted -= ChangeAnimation;
        _playerState.StartedMovement -= ChangeAnimation;
        FieldOfViewCalculate.GameIsLost -= ChangeAnimation;
    }

    private void ChangeAnimation()
    {
        if(_animator.GetBool(_changeRobberyMovement) == false)
        {
            _animator.SetBool(_changeRobberyMovement, true);
            _playerState.SavePlayerRun(true);
        }
        else
        {
            _animator.SetBool(_changeRobberyMovement, false);
            _playerState.SavePlayerRun(false);
        }           
    }
}