
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectController : MonoBehaviour
{
    #region Instance
 
    private static ModeSelectController _instance;

    public static ModeSelectController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ModeSelectController>();
            }

            return _instance;
        }
    }
 
    #endregion

    private MainMenuController _mainMenuController;
    private LevelMenuController _levelMenuController;
    private RewardSystem _rewardSystem;
//    private AdsController _AdsController;

    
    public Image[] ModeImages;
    public GameObject AllGame,AllGameMainmenu;
    public TextMeshProUGUI UnlockAllGameRemainingVideos,UnlockAllGameRemainingVideosMainmenu;
    public int UnlockAllGameMaxVideos;

    private void Awake()
    {
  
        _instance = this;
  
    }


    private void Start()
    {
        _mainMenuController = MainMenuController.instance;
        _levelMenuController = LevelMenuController.instance;
        _rewardSystem=RewardSystem.instance;
     //   _AdsController=AdsController.Instance;

        // if (DataController.instance.getPrefBool(ScriptLocalization.AllGame))
        // {
        //     AllGame.SetActive(false);
        //     AllGameMainmenu.SetActive(false);
        // }
        // else
        // {
        //     UnlockAllGameRemainingVideosMainmenu.text=UnlockAllGameRemainingVideos.text = DataController.instance.getPref(ScriptLocalization.AllGameRemainingVideo)+"/"+ UnlockAllGameMaxVideos;
        // }
    }
 
    public void PlayBtnClick()
    {
        _mainMenuController.AddPanelToStackAndLoad(2);
        _levelMenuController.ExecuteLevel();
        
    }
    
    public void ModeSelectOnBtnClick(int n)
    {
        ApplicationController.SelectedGameMode = n;
         PlayBtnClick();
         // if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
         // {
         //     AdsController.Instance._showInterstitialGame();
         // }
    }


    public void SelectedMode()
    {
        ModeImages[ApplicationController.SelectedGameMode].GetComponent<Selectable>().Select();
    }
    
    public void UnlockAllGame()
    {
        _rewardSystem.ShowRewarded(ScriptLocalization.UnlockFullGame);
    }

    public void UnlockAllGameReward()
    {
        if (DataController.instance.getPref(ScriptLocalization.AllGameRemainingVideo) >= UnlockAllGameMaxVideos-1)
        {
             InventoryController.instance.UnlockAllInventoryItem();
           DataController.instance.setRewardPref(ScriptLocalization.AllGame,1);
           DataController.instance.setPref(ScriptLocalization.AllLevelsRemainingVideo,_levelMenuController.AllLevelUnlockMaxVideos);
            _levelMenuController.UnlockAllLevelReward();

           // AllGame.SetActive(false);
           // AllGameMainmenu.SetActive(false);
        }
        else
        {
            DataController.instance.setPref(ScriptLocalization.AllGameRemainingVideo,DataController.instance.getPref(ScriptLocalization.AllGameRemainingVideo)+1);
            UnlockAllGameRemainingVideosMainmenu.text=   UnlockAllGameRemainingVideos.text = DataController.instance.getPref(ScriptLocalization.AllGameRemainingVideo)+"/"+ UnlockAllGameMaxVideos;

        }
    }
    public void ShowAds()
    {
     //   _AdsController._showInterstitialGame();
    }
}


