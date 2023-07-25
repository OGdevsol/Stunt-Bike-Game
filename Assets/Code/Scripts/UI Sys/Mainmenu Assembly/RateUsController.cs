
using UnityEngine;

public class RateUsController : MonoBehaviour
{
    #region Instance
 
    private static RateUsController _instance;

    public static RateUsController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RateUsController>();
            }

            return _instance;
        }
    }
 
    #endregion
 
    private void Awake()
    {
        _instance = this;
  
    }


    private void Start()
    {

    }
 
    public void display()
    {
        containerGO.alpha = 1;
        
    }
 
    public void hide()
    {
        containerGO.alpha = 0;

    }
 
    [Header("Panel Container")]
    public CanvasGroup containerGO; 
}