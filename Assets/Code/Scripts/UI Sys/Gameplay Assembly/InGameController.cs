using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameController : MonoBehaviour
{
    #region Instance

    private static InGameController _instance;

    public static InGameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InGameController>();
            }

            return _instance;
        }
    }

    #endregion

    public GameObject Controller, Countdown;
    public Slider LevelProgress;

    public GameObject TimeOrientation, ChallengeOrientation;

    public TextMeshProUGUI LevelText, TimeDuration, LivesText;


    public delegate void Race(bool t);

    public static event Race _Race;

    public delegate void Brake(bool t);

    public static event Brake _Brake;

    public delegate void Jump(bool t);

    public static event Jump _Jump;

    public delegate void BackFlip(bool t);

    public static event BackFlip _BackFlip;

    public delegate void FrontFlip(bool t);

    public static event FrontFlip _FrontFlip;


    private bool _startlvlProgress;

    public Rigidbody _playerTransform;

    private GameController gameController;


    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] public int playerlives = 3;
    public void Start()
    {
        gameController = GameController.instance;
        LevelText.text = ScriptLocalization.LevelTitle + (ApplicationController.SelectedLevel + 1);
        LivesText.text = "Lives: " + (playerlives);

        if (GameController.gameMode == Mode.TIMEATTACK)
        {
            TimeOrientation.SetActive(true);
        }
        else
        {
            TimeOrientation.SetActive(false);
        }

        if (GameController.gameMode == Mode.CHALLENGE)
        {
            ChallengeOrientation.SetActive(true);
        }

        Countdown.SetActive(true);
        Controller.SetActive(false);
    }

    private void OnEnable()
    {
        GameController.gamePaused += pauseClicked;
        GameController.gameResumed += resumeClicked;
        GameController.gameStarted += StartLevelProgress;
        GameController.gameEnded += endedClicked;
        GameController.gameRespawned += RespawnClicked;
    }

    private void OnDisable()
    {
        GameController.gamePaused -= pauseClicked;
        GameController.gameResumed -= resumeClicked;
        GameController.gameStarted -= StartLevelProgress;
        GameController.gameEnded -= endedClicked;
        GameController.gameRespawned -= RespawnClicked;
    }


    public void StartLevelProgress()
    {
        _playerTransform = gameController._playerTransform;
        LevelProgress.maxValue = gameController.currentLevel.Finishpoint.position.x;
        LevelProgress.minValue = _playerTransform.worldCenterOfMass.x;
        _startlvlProgress = true;

        if (GameController.gameMode == Mode.TIMEATTACK)
        {
            currCountdownValue = gameController._levelManager.Time[ApplicationController.SelectedLevel];
            StartCoroutine(StartCountdown(currCountdownValue));
        }

        startTime = Time.time;
        Countdown.SetActive(false);
        Controller.SetActive(true);
    }

    float currCountdownValue;

    public IEnumerator StartCountdown(float countdownValue)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            int minutes = Mathf.FloorToInt(currCountdownValue / 60F);
            int seconds = Mathf.FloorToInt(currCountdownValue - minutes * 60);
            TimeDuration.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    private float startTime;
    public float endTime = 0f;
    public TMP_Text TimeText;

    public void Update()
    {
        if (Crash.instance)
        {
            LivesText.text = "Lives: " + (playerlives);
        }

        if (GameController.gameStatus != GameStatus.INGAME)
            return;
        endTime = Time.time - startTime;
        float minutes = Mathf.FloorToInt(endTime / 60);
        float seconds = Mathf.FloorToInt(endTime % 60);
        if (minutes < 10 && seconds < 10)
        {
            TimeText.text = $"0{minutes} : 0{seconds}";
        }
        else if (minutes < 10)
        {
            TimeText.text = $"0{minutes} : {seconds}";
        }

        else if (seconds < 10)
        {
            TimeText.text = $"{minutes} : 0{seconds}";
        }
        else
        {
            TimeText.text = $"{minutes} : {seconds}";
        }

        if (_startlvlProgress && _playerTransform)
        {
            LevelProgress.value = _playerTransform.worldCenterOfMass.x;
        }

        if (GameController.gameMode == Mode.TIMEATTACK)
        {
        }

        if (BikeController.Instance)
        {
            if (isNitroActive && BikeController.Instance.speed > 50)
            {
                nitroTimeLeft -= Time.deltaTime;
                if (nitroTimeLeft <= 0f)
                {
                    endNitro();
                }
            }
        }
       

        NitroFill.fillAmount = nitroTimeLeft * 2 / 10;
    }

    public Image NitroFill;
    public static float nitroTimeLeft = 5f;
    public static bool isNitroActive;

    public void nitroPressed()
    {
        if (isNitroActive)
        {
            return;
        }

        if (nitroTimeLeft <= 0f)
        {
            GetNitro();
        }

        isNitroActive = true;
        BikeController.Instance.NitroEffect.SetActive(true);
        BikeController.Instance.speed += 100;
    }

    public bool NitroRwd;

    public void GetNitro()
    {
        NitroRwd = true;
      //  AdsController.Instance.ShowRewarded();
    }

    public void endNitro()
    {
        if (!isNitroActive)
        {
            return;
        }

        isNitroActive = false;
        BikeController.Instance.speed -= 50;
        BikeController.Instance.NitroEffect.SetActive(false);
    }

    public void pauseClicked()
    {
    }

    public void RespawnClicked()
    {
    }

    public void resumeClicked()
    {
    }

    public void startedClicked()
    {
    }

    public void endedClicked()
    {
    }

    public void BrakeClicked(bool t)
    {
        _Brake?.Invoke(t);
    }

    public void RaceClicked(bool t)
    {
        _Race?.Invoke(t);
    }

    public void FrontFlipClicked(bool t)
    {
        _FrontFlip?.Invoke(t);
    }

    public void BackFlipClicked(bool t)
    {
        _BackFlip?.Invoke(t);
    }

    public void JumpClicked(bool t)
    {
        _Jump?.Invoke(t);
    }
}