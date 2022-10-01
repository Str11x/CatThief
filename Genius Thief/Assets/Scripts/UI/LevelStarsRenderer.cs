using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelStarsRenderer : MonoBehaviour
{
    [SerializeField] private RecordSaver _recordSaver;
    [SerializeField] private Button[] _scenes;

    private List<string> _levelsName = new List<string>();

    private void Start()
    {
        for(int sceneNumber = 0; sceneNumber < _scenes.Length; sceneNumber++)
        {
            _levelsName.Add(_scenes[sceneNumber].name);
        }

        for(int sceneNumber = 0; sceneNumber < _levelsName.Count; sceneNumber++)
        {
            Star[] star = _scenes[sceneNumber].GetComponentsInChildren<Star>(true);

            int score = _recordSaver.GetLevelScore(_levelsName[sceneNumber]);

            for (int starNumber = 0; starNumber < score; starNumber++)
                star[starNumber].gameObject.SetActive(true);
        }  
    }
}