using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Instance
 
    private static LevelManager _instance;

    public static LevelManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }

            return _instance;
        }
    }
 
    #endregion
    public GameObject[] Levels;
    public LevelController currentLevel;
    public int[] Time;
    private void Awake()
    {
        _instance = this;
  
        GameController.gameMode = (Mode)ApplicationController.SelectedGameMode ;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(Levels.Length>0)
            Levels[ApplicationController.SelectedLevel].SetActive(true);

        currentLevel = Levels[ApplicationController.SelectedLevel].GetComponent<LevelController>();
    }

 
}
