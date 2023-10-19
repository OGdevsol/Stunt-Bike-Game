using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] ReferencePanels;
    SceneMngmtController _sceneMngmtController;
    // Start is called before the first frame update
    private void Awake()
    {
        _sceneMngmtController = SceneMngmtController.instance;
    }

    void Start()
    {
        EnableReference(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableReference(int z )
    {
        for (int i = 0; i < z; i++)
        {
            ReferencePanels[i].SetActive(false);
        }
        ReferencePanels[z].SetActive(true);
    }

    public void LoadMainMenuScene()
    {
        _sceneMngmtController.LoadScene(Scenes.Mainmenu);
        // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        // {
        //     AdsController.Instance._showInterstitialGame();
        // }
    }
}
