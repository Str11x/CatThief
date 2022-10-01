using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteAll();
    }
}
