using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private Wallet _playerWallet;
    [SerializeField] private PathRenderer _pathRenderer;
    [SerializeField] private RewardObjects _rewardObjects;
    [SerializeField] private LevelMenu _levelMenu;
    [SerializeField] private TimeService _timeService;

    [SerializeField] private AudioSource _mainTheme;
    [SerializeField] private AudioSource _markerInstantiate;
    [SerializeField] private AudioSource _pickedupLoot;
    [SerializeField] private AudioSource _startPlayer;
    [SerializeField] private AudioSource _winnerTheme;
    [SerializeField] private AudioSource _backButton;
    [SerializeField] private AudioSource _click;
    [SerializeField] private AudioSource _toggleClick;
    [SerializeField] private AudioSource _gameOver;
    [SerializeField] private AudioSource _markerTouch;

    private bool _isInstructionDisabled = false;

    private void Start()
    {
        _playerWallet.TouchedMarker += PlayMarkerTouchSound;
        _levelMenu.LevelDone += PlayWinnerTheme;
        _levelMenu.LevelFailed += PlayGameOverSound;
        FieldOfViewCalculate.GameIsLost += PlayGameOverSound;
        _rewardObjects.PickedupLoot += PlayPickedupSound;
        _pathRenderer.AddedMarker += PlayMarkerInstantiateSound;
        _timeService.PauseEnabled += PauseSounds;
        _timeService.PauseDisabled += PlayStartSound;
        _mainTheme.Play();
    }

    private void OnDisable()
    {
        _playerWallet.TouchedMarker -= PlayMarkerTouchSound;
        _levelMenu.LevelDone -= PlayWinnerTheme;
        _levelMenu.LevelFailed -= PlayGameOverSound;
        FieldOfViewCalculate.GameIsLost -= PlayGameOverSound;
        _pathRenderer.AddedMarker -= PlayMarkerInstantiateSound;
        _rewardObjects.PickedupLoot -= PlayPickedupSound;
        _timeService.PauseEnabled -= PauseSounds;
        _timeService.PauseDisabled -= PlayStartSound;
    }

    private void PlayPickedupSound()
    {
        _pickedupLoot.Play();
    }
    
    private void PlayMarkerInstantiateSound()
    {
        _markerInstantiate.Play();
    }

    private void PlayGameOverSound()
    {
        _startPlayer.Stop();
        _mainTheme.Stop();
        _gameOver.Play();
    }

    private void PlayWinnerTheme()
    {
        _startPlayer.Stop();
        _mainTheme.Stop();
        _winnerTheme.Play();
    }

    private void PlayMarkerTouchSound()
    {
        _markerTouch.Play();
    }

    private void PauseSounds()
    {
        _startPlayer.Pause();
    }

    public void PlayStartSound()
    {   
        if(_isInstructionDisabled == true)
            _startPlayer.Play();
    }

    public void PlayBackButtonSound()
    {
        _backButton.Play();
    }

    public void PlayClickButton()
    {
        _click.Play();
    }

    public void PlayToggleClickButton()
    {
        _toggleClick.Play();
    }

    public void StartRobbery()
    {
        _isInstructionDisabled = true;
    }
}