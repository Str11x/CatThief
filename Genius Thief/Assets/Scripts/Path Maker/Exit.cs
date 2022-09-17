using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool IsPlayerPlannedExit { get; private set; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("спнбемэ гюбепьем");
            Invoke("LevelCompleted", 1);
        }
        else
        {
            IsPlayerPlannedExit = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        IsPlayerPlannedExit = false;
    }

    private void LevelCompleted()
    {
        Time.timeScale = 0;
    }
}