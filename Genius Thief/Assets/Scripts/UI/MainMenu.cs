using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ChangePanelState()
    {                
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);       
    }

    public void ExitAplication()
    {
        Application.Quit();
    }
}