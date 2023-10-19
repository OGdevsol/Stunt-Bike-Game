using System;
using System.Collections;
using UnityEngine;

public class SplashLoadingController : MonoBehaviour

{
    #region Instance

    private static SplashLoadingController _instance;

    public static SplashLoadingController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SplashLoadingController>();
            }

            return _instance;
        }
    }

    #endregion


    #region Initialization

    SceneMngmtController _sceneMngmtController;

    public int TimeToLoad;

    #endregion


    private void Awake()
    {
        _instance = this;
        try
        {
            string input = Console.ReadLine();
            // process input
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            // handle the error
        }
        _sceneMngmtController = SceneMngmtController.instance;

        
        if (!PlayerPrefs.HasKey("GameLaunch"))
        {
          PlayerPrefs.SetInt("GameLaunch",1);
          StartCoroutine(nameof(LoadTutorialTimer));

        }
        
    }

    public IEnumerator LoadTutorialTimer()
    {
        yield return new WaitForSecondsRealtime(TimeToLoad);
        LoadTutorial();
    }


    private void Start()
    {
       
        Invoke(nameof(LoadScene), TimeToLoad);
        // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        // {
        //     AdsController.Instance._showlargeBanner();
        //     AdsController.Instance._showsmallBanner();
        // }
    }

    public void LoadScene()
    {
        _sceneMngmtController.LoadScene(Scenes.Mainmenu);
        // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        // {
        //     AdsController.Instance._showInterstitialGame();
        // }
    }
    public void LoadTutorial()
    {
        Debug.Log("Loading Tutorial");
        _sceneMngmtController.LoadScene(Scenes.Tutorial);
        // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        // {
        //     AdsController.Instance._showInterstitialGame();
        // }
    }
}