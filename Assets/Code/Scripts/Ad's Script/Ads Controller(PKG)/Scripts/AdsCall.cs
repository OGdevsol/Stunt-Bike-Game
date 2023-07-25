/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsCall : MonoBehaviour
{
    #region Instance

    private static AdsCall _instance;

    public static AdsCall instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdsCall>();
            }

            return _instance;
        }
    }

    #endregion
    
    #region  Banner Calls

    public void ShowBanner2()
    {
        //AdsController.Instance.banne();
    }
    
    
    public void ShowBanner1()
    {
        if (PlayerPrefs.HasKey("DoNotShowAds")) return;
        
        AdsController.Instance.ShowSmallBanner1();
    }
    
    
    public void HideBanner1()
    {
       AdsController.Instance.HideSmallBanner1();
    }

    public void HideBanner2()
    {
        //AdsController.Instance.HideSmallBanner2();
    }
    
    public void ShowLargeBanner()
    {
        if (PlayerPrefs.HasKey("DoNotShowAds")) return;
        
        AdsController.Instance.ShowLargeBanner();
    }
    
    public void HideLargeBanner()
    {
        AdsController.Instance.HideLargeBanner();
    }
    
    #endregion
    
    #region  Interstitial CallBacks
    
    public void InterstitialAdMainmenu()
    {
        if (PlayerPrefs.HasKey("DoNotShowAds")) return;
        
        AdsController.Instance.Show_Admob_InterstitialMain();
    }
    
    public void InterstitialAdGamePlay()
    {
        if (PlayerPrefs.HasKey("DoNotShowAds")) return;
        
        AdsController.Instance.Show_Admob_InterstitialGame();
    }
    
    public void ShuffleUnity_AdmobInterstitial()
    {
        if (PlayerPrefs.HasKey("DoNotShowAds")) return;
        
        AdsController.Instance.ShowUnityAdmobAlternateInterstitialAds();
    }

    
    #endregion
    
    #region  Rewarded CallBacks
    
    public void AdmobRewardedAd()
    {
        AdsController.Instance.ShowAdmobRewardedVideo();
    } 
    
    public void AdmobRewardedInterstitialAd()
    {
        AdsController.Instance.ShowRewardedInterstitialAd();
    } 
    
    public void UnityRewardedAd()
    {
        AdsController.Instance.ShowUnityRewarded();
    }
    
    public void UnityVideo()
    {
        AdsController.Instance.ShowUnityVideo();
    }

    public void ShowUnityAdmobRewarded()
    {
        AdsController.Instance.ShowUnityAdmobRewarded();
    }
    
    #endregion
}
*/