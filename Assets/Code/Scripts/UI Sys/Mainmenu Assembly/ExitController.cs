
using UnityEngine;

public class ExitController : MonoBehaviour
{
    #region Instance
 
    private static ExitController _instance;

    public static ExitController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ExitController>();
            }

            return _instance;
        }
    }
 
    #endregion
 
    private MainMenuController _mainMenuController;
   // private AdsController _AdsController;

    private void Awake()
    {
  
        _instance = this;
     //   _AdsController=AdsController.Instance;

    }


    private void Start()
    {
        _mainMenuController = MainMenuController.instance;
    }
    
    public void Yes()
    {
       Application.Quit();
        
    }
 
    public void No()
    {
        _mainMenuController.RemoveLastPanelFormStack();
        // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
        // {
        //     AdsController.Instance._showsmallBanner();
        //     AdsController.Instance._hidelargeBanner();
        // }
    }
 
    public void ShowAds()
    {
       // _AdsController._showlargeBanner();
    }
    public void HideAds()
    {
      //  _AdsController._hidelargeBanner();
    }
}
