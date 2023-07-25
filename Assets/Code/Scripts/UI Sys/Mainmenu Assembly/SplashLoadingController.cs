using System;
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
    }


    private void Start()
    {
        _sceneMngmtController = SceneMngmtController.instance;
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
}