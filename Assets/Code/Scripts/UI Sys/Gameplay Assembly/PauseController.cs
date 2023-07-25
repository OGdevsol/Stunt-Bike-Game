
using UnityEngine;

public class PauseController : MonoBehaviour
{
    #region Instance
 
    private static PauseController _instance;

    public static PauseController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PauseController>();
            }

            return _instance;
        }
    }
 
    #endregion
 
    private void Awake()
    {
        _instance = this;
  
    }
    private void OnEnable()
    {
        GameController.gamePaused += pauseClicked;
        GameController.gameResumed += resumeClicked;

    }

    private void OnDisable()
    {
        GameController.gamePaused -= pauseClicked;
        GameController.gameResumed -= resumeClicked;

    }

    public void pauseClicked()
    {
        display();
    }
    public void resumeClicked()
    {
        hide();
    }
    
    public void display()
    {
        containerGO.enabled = true;
        
    }
 
    public void hide()
    {
        containerGO.enabled = false;

    }
 
    [Header("Panel Container")]
    public Canvas containerGO; 
}
