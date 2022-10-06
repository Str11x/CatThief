using UnityEngine;

public class Star : MonoBehaviour
{
    private AudioSource _instantiateSound;

    private void Awake()
    {
        _instantiateSound = GetComponent<AudioSource>();
    }

    public void StartInstantiateStarSound()
    {
        _instantiateSound.Play();
    }
}