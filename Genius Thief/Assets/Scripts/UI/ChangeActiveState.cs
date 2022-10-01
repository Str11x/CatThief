using UnityEngine;

public class ChangeActiveState : MonoBehaviour
{
    public void ChangeState()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}