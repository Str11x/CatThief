using TMPro;
using UnityEngine;

public class ShowRewardText : MonoBehaviour
{
    [SerializeField] private RewardObjects _lootService;

    private float _timeToTurnOff = 0.6f;
    private Vector3 _offsetMessagePosition = new Vector3 (-2, 2, -2);

    private TMP_Text[] _messages;
    private int _currentRandomMessage;

    private void Awake()
    {
        _lootService.LootPickedup += EnableRandomTextMessage;

        _messages = GetComponentsInChildren<TMP_Text>(true);
    }

    private void OnDisable()
    {
        _lootService.LootPickedup -= EnableRandomTextMessage;
    }

    private void EnableRandomTextMessage()
    {
        _currentRandomMessage = Random.Range(0, _messages.Length);
        _messages[_currentRandomMessage].transform.position = _lootService.LastLootPosition + _offsetMessagePosition;

        _messages[_currentRandomMessage].gameObject.SetActive(true);
        Invoke(nameof(DisableLastMessage), _timeToTurnOff);
    }

    private void DisableLastMessage()
    {
        _messages[_currentRandomMessage].gameObject.SetActive(false);
    }
}