using System;
using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    #region Instance

    private static GameOverController _instance;

    public static GameOverController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameOverController>();
            }

            return _instance;
        }
    }

    #endregion

    public TMP_Text Money, Time;

    private void OnEnable()
    {
        GameController.gameEnded += endedClicked;
    }

    private void OnDisable()
    {
        GameController.gameEnded -= endedClicked;
    }

    private void Awake()
    {
        _instance = this;
        //  FailAnimator.enabled = false;
    }

    public void endedClicked()
    {
    }

    public void display()
    {
        containerGO.enabled = true;
        Money.text = 100.ToString();
        float minutes = Mathf.FloorToInt(InGameController.instance.endTime / 60);
        float seconds = Mathf.FloorToInt(InGameController.instance.endTime % 60);
        if (minutes < 10 && seconds < 10)
        {
            Time.text = $"0{minutes} : 0{seconds}";
        }
        else if (minutes < 10)
        {
            Time.text = $"0{minutes} : {seconds}";
        }

        else if (seconds < 10)
        {
            Time.text = $"{minutes} : 0{seconds}";
        }
        else
        {
            Time.text = $"{minutes} : {seconds}";
        }


        DataController.instance.AddCoins(100);
    }

    public void hide()
    {
        containerGO.enabled = false;
    }

    [Header("Panel Container")] public Canvas containerGO;
    // public Animator FailAnimator;
}