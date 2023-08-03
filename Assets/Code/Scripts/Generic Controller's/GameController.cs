using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Instance

    private static GameController _instance;

    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }

    #endregion

    public GameOverController _gameOverController;

    public GameWinController _gameWinController;

    private LoadingController _loadingController;

    public delegate void GameStatusHandler();

    public static event GameStatusHandler gamePaused;

    public static event GameStatusHandler gameResumed;

    public static event GameStatusHandler gameRespawned;

    public static event GameStatusHandler gameStarted;

    public static event GameStatusHandler gameEnded;

    public static event GameStatusHandler gameRestarted;

    public delegate void Cinematic();

    public static event Cinematic cinematicEvent;

    public static GameStatus _gameStatus = GameStatus.INGAME;

    public static GameStatus gameStatus
    {
        get { return _gameStatus; }
        set { _gameStatus = value; }
    }


    public static Mode gameMode;

    [HideInInspector] public Rigidbody _playerTransform;
    [HideInInspector] public LevelController currentLevel;

    public LevelManager _levelManager;

//    AdsController _AdsController;

    private void Awake()
    {
        _instance = this;


        gameMode = (Mode) ApplicationController.SelectedGameMode;
        if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        {
            AdsController.Instance._showsmallBanner();
        }    

    }


    void Start()
    {
        _loadingController = LoadingController.instance;
        _levelManager = LevelManager.instance;
        Invoke(nameof(GameStart), 4f);
    }
   

    public void displayGameOver()
    {
        gameStatus = GameStatus.GAMEOVER;
        _gameOverController.display();
        gameEnded?.Invoke();
        if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        {
            AdsController.Instance._showInterstitialGame();
        }
    }

    public void displayGameWin()
    {
        gameStatus = GameStatus.GAMEOVER;
        _gameWinController.display();
        gameEnded?.Invoke();
        if (ApplicationController.SelectedLevel < 9)
        {
            DataController.instance.SetUnlockLevel(ApplicationController.SelectedLevel);
            DataController.instance.AddCoins(1000);
        }

        if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        {
            AdsController.Instance._showInterstitialGame();
        }
    }

    public void GameStart()
    {
        gameStatus = GameStatus.INGAME;

        if (gameStatus == GameStatus.PAUSED)
        {
            gameResumed?.Invoke();
        }
        else
        {
            gameStarted?.Invoke();
        }
    }
    //Stop Motion

    /* public void StopObjectMotion()
     {
         // Stop linear motion
         rigb.velocity = Vector3.zero;
 
         // Stop angular motion
         rigb.angularVelocity = Vector3.zero;
     }*/


    public void GameResumed()
    {
        if (gameStatus == GameStatus.PAUSED)
        {
            gameStatus = GameStatus.INGAME;
            gameResumed?.Invoke();
        }
    }

    public void GamePaused()
    {
        gameStatus = GameStatus.PAUSED;
        gamePaused?.Invoke();
        // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        // {
        //     AdsController.Instance._showInterstitialGame();
        // }
    }

    public void GameRestart()
    {
        gameEnded?.Invoke();
        gameRestarted?.Invoke();
        _loadingController.display(Scenes.Gameplay);
    }

    public void Home()
    {
        _loadingController.display(Scenes.Mainmenu);
    }

    public void Next()
    {
        if (ApplicationController.SelectedLevel < 9)
        {
            ApplicationController.SelectedLevel += 1;
            ApplicationController.LastSelectedLevel = ApplicationController.SelectedLevel;
            _loadingController.display(Scenes.Gameplay);
        }
        else
        {
            // ApplicationController.SelectedLevel = 1;
            // ApplicationController.LastSelectedLevel = ApplicationController.SelectedLevel;
            _loadingController.display(Scenes.Mainmenu);
        }
    }
    public Transform RespawnplayerPos;
}