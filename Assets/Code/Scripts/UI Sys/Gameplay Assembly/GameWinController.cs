
using System;
using TMPro;
using UnityEngine;

public class GameWinController : MonoBehaviour
{
    #region Instance
 
    private static GameWinController _instance;

    public static GameWinController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameWinController>();
            }

            return _instance;
        }
    }
 
    #endregion

    public TMP_Text Money, Time;
    private void Awake()
    {
        _instance = this;
       // WinAnimator.enabled = false;

    }
  
    public void display()
    {
        containerGO.enabled = true;
        Money.text = 1000.ToString();
        float minutes = Mathf.FloorToInt(InGameController.instance.endTime / 60);
        float seconds = Mathf.FloorToInt(InGameController.instance.endTime  % 60);
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
        DataController.instance.AddCoins(1000);  

        // WinAnimator.enabled = true;
    }
 
    public void hide()
    {
        containerGO.enabled = false;

    }
 
    [Header("Panel Container")]
    public Canvas containerGO; 
    //public Animator WinAnimator;

}
