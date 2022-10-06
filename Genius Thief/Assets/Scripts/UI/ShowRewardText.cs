using TMPro;
using UnityEngine;

public class ShowRewardText : MonoBehaviour
{
    private const string DisableMessage = "DisableLastMessage";

    [SerializeField] private RewardObjects _lootService;

    private int _timeToTurnOff = 1;
    private Vector3 _offsetMessagePosition = new Vector3 (-2, 2, -2);

    private TMP_Text[] _messages; 

    private void Awake()
    {
        _lootService.PickedupLoot += EnableRandomTextMessage;

        _messages = GetComponentsInChildren<TMP_Text>(true);
    }

    private void OnDisable()
    {
        _lootService.PickedupLoot -= EnableRandomTextMessage;
    }

    private void EnableRandomTextMessage()
    {
        int randomMessage = Random.Range(0, _messages.Length);
        _messages[randomMessage].transform.position = _lootService.LastLootPosition + _offsetMessagePosition;

        _messages[randomMessage].gameObject.SetActive(true);
        Invoke(DisableMessage, _timeToTurnOff);
    }

    private void DisableLastMessage(int lastMessageNumber)
    {
        _messages[lastMessageNumber].gameObject.SetActive(false);
    }
}