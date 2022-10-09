using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PathRenderer _pathRenderer;
    [SerializeField] private RewardObjects _rewardObjects;
    [SerializeField] private LevelMenu _levelMenu;

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

    private void Start()
    {
        _player.TouchedMarker += PLayMarkerTouchSound;
        _levelMenu.LevelDone += PlayWinnerTheme;
        _levelMenu.LevelFailed += PlayGameOverSound;
        FieldOfViewCalculate.GameIsLost += PlayGameOverSound;
        _rewardObjects.PickedupLoot += PlayPickedupSound;
        _pathRenderer.AddedMarker += PlayMarkerInstantiateSound;
        _mainTheme.Play();
    }

    private void OnDisable()
    {
        _player.TouchedMarker -= PLayMarkerTouchSound;
        _levelMenu.LevelDone -= PlayWinnerTheme;
        _levelMenu.LevelFailed -= PlayGameOverSound;
        FieldOfViewCalculate.GameIsLost -= PlayGameOverSound;
        _pathRenderer.AddedMarker -= PlayMarkerInstantiateSound;
        _rewardObjects.PickedupLoot -= PlayPickedupSound;
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

    private void PLayMarkerTouchSound()
    {
        _markerTouch.Play();
    }

    public void PlayStartSound()
    {
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
}