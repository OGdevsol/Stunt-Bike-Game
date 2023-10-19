
using System.Collections;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    #region Instance
 
    private static LoadingController _instance;

    public static LoadingController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingController>();
            }

            return _instance;
        }
    }
 
    #endregion
    
    [Header("Panel Container")]
    public Canvas containerGO;

    public Animator LoadingAnimator;
    
    public int TimeToLoad;
    
    private SceneMngmtController _sceneMngmtController;
//    private AdsController _AdsController;

    private void Awake()
    {
        _instance = this;

    }

    public void Start()
    {
    _sceneMngmtController=SceneMngmtController.instance;
//   _AdsController=AdsController.Instance;

    LoadingAnimator.enabled = false;

    }
    

    public void display(Scenes s)
    {
        containerGO.enabled = true;
        StartCoroutine(LoadScene(s));
      //  _AdsController._showsmallBanner();
      //  _AdsController._showlargeBanner();
    }

    public IEnumerator LoadScene(Scenes s)
    {
        LoadingAnimator.enabled = true;
        yield return new WaitForSeconds(TimeToLoad);
        _sceneMngmtController.LoadScene(s);
    }

    public void hide()
    {
        containerGO.enabled = false;

    }
 
 

}
public enum Scenes
{
    Splash,
    Mainmenu,
    Gameplay,
    Tutorial
    
}