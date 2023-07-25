
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{

 #region Instance
 
 private static MenuController _instance;

 public static MenuController instance
 {
  get
  {
   if (_instance == null)
   {
    _instance = FindObjectOfType<MenuController>();
   }

   return _instance;
  }
 }
 
 #endregion


 public Animator Animator;
 public Image ItemSprite;

 private MainMenuController _mainMenuController;
 private InventoryController _inventoryController;
 private ModeSelectController modeSelectController;
 private RewardSystem _rewardSystem;


 public TextMeshProUGUI RemoveAdAd;
    
 public int maxRemoveAd;
    
 public GameObject RemoveAdBtn;

 private void Awake()
 {
  
  _instance = this;
  
 }


 private void Start()
 {
  _mainMenuController = MainMenuController.instance;
  _inventoryController = InventoryController.instance;
  modeSelectController = ModeSelectController.instance;
  _rewardSystem=RewardSystem.instance;    

  ItemSprite.sprite =_inventoryController.Items[ApplicationController.SelectedInventoryItem];
  
  /*if (AdsController.Instance)
  {
   AdsController.Instance._showsmallBanner();
   AdsController.Instance._hidelargeBanner();
  }*/

        
        
  if (PlayerPrefs.GetString("DoNotShowAds") == "Unlocked")
  {
   RemoveAdBtn.SetActive(false);
   RemoveAdAd.gameObject.SetActive(false);
  }
  else
  {
   RemoveAdAd.text = DataController.instance.getPref(RewardTriggers.RemoveAd.ToString()) + "/" + maxRemoveAd; 
  }
  
 }
 
 public void PlayBtnClick()
 {
   _mainMenuController.AddPanelToStackAndLoad(1);
   modeSelectController.SelectedMode();
 }

 public void SettingBtnClick()
 {
  _mainMenuController.AddPanelToStackAndLoad(4);
  if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
  {
 //  AdsController.Instance._showlargeBanner();
  // AdsController.Instance._showsmallBanner();
 //  AdsController.Instance._showInterstitialGame();
  }
 }
 public void InventoryBtnClick()
 {
  _inventoryController.Display();
 }
 public void ShopBtnClick()
 {
  _mainMenuController.AddPanelToStackAndLoad(6);
 }
 
 public void BackBtnClick()
 {
  _mainMenuController.AddPanelToStackAndLoad(5);
  if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
  {
 //  AdsController.Instance._showlargeBanner();
 //  AdsController.Instance._showsmallBanner();
   // AdsController.Instance._showInterstitialGame();
  }
 }

 public void PlayAnimation()
 {
   Animator.Play("ItemShow");
 }

  public void UnlockAllAssets()
    {
//        LevelMenuController.instance.UnlockAllLevels();
     //   InventoryController.instance.UnlockAllItems();
        //
        // if(AdsController.Instance)
        // {
        //     AdsController.Instance._hidesmallBanner();
        // }
    }

    public void RemoveAds()
    {
     //   InAppHandler.Instance.BuyRemoveAds();
     _rewardSystem.ShowRewarded(ScriptLocalization.RemoveAdRewarded);
    }

    
    public void RemoveAdWatched()
    {
        if (DataController.instance.getPref(RewardTriggers.RemoveAd.ToString()) <
            maxRemoveAd-1)
        {
            DataController.instance.setPref(RewardTriggers.RemoveAd.ToString(),
                DataController.instance.getPref(RewardTriggers.RemoveAd.ToString()) + 1);
            RemoveAdAd.text = DataController.instance.getPref(RewardTriggers.RemoveAd.ToString()) + "/" + maxRemoveAd;
        }
        else
        {
            PlayerPrefs.SetString("DoNotShowAds", "Unlocked");
            // AdsController.Instance._hidesmallBanner();
            // AdsController.Instance._hidelargeBanner();
            if (PlayerPrefs.GetString("DoNotShowAds") == "Unlocked")
            {
                RemoveAdBtn.SetActive(false);
                RemoveAdAd.gameObject.SetActive(false);
            }
        }
    }

    public void Play()
    {
     ApplicationController.SelectedGameMode = 0;
     ApplicationController.SelectedLevel = 0;
     LoadingController.instance.display(Scenes.Gameplay);
    }

    public void PrivacyPolicy()
    {
     Application.OpenURL("https://lucidtecstudio.blogspot.com/2023/05/privacy-policy.html");
    } 
    public void RateUs()
    {
     Application.OpenURL("https://play.google.com/store/apps/details?id=com.ls.stunt.bike.racing.game.trial.tricks.master.stunt.master");
    }
}
