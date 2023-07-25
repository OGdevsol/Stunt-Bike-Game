using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class Ad_Ids : ScriptableObject
{

 public enum AdsNetwork
 {
  BothAdmobUnity,
  AdmobeOnly,
  UnityOnly,
  DoNotShowAds
 };
 
 
 [Space(5)]
 [Header("AD IDs", order = -1)]
 [Space(5)]
 public AdsNetwork chooseAdsNetwork;
 

 /*public bool admobAdsOnly;
 public bool unityAdsOnly;
 public bool dontShowAds;*/
 
 [Header("* Admob IDs *")]
 [Space(3)]
 
 
 [Header("---Admob App Id")]
 [Space(5)]public string admobAppId;
 
 [Header("----Admob App open Ad Id")]
 [Space(3)]public string admobAppOpenId;
 public bool AdmobOpenAppAD;
 

 [Header("---Admob Small Banner Ad Id")] 
 [Space(3)]
 public string admobBannerId;

 [Space(3)]
 
 [Header("---Admob large Banner Ad Id")] 
 [Space(3)]
 public string admobLargeBannerId;

 [Header("---Admob Interstitial Ad Id")] [Space(3)]
 public string interstitialMainId;
 
 [Space(3)]public string interstitialGameId;
 
 [Header("---Admob Rewarded Interstitial Ad Id")]
 [Space(3)]public string interstitialRewardedId;
 
 [Header("---Admob Rewarded Ad Id")]
 [Space(3)]public string admobRewardedVideoId;
 
 [Space(3)]public string admobAppOpenId_Medium;
 [Space(3)]public string admobAppOpenId_High;
 
 
  [Header("---Admob Banner Positions")] 
  [Space(3)]
 
  [Space(3)]public AdPosition smallBannerPosition1;
  [Space(3)]public AdPosition largeBannerPosition;
 
  [Space(3)]
  public bool isSecondSmallBannerRequired;
  public AdPosition smallBannerPosition2;
 
 
  // [Space(5)] [Header("*- Unity ID / Ads Strings *")] [Space(5)]
  // [Space(3)]public bool isUnityTestADs;
  // [Space(3)]public string unityId;
  //
  // [Space(5)]public string unityBannerString;
  // [Space(3)]public BannerPosition unitySmallBannerPosition;
  // [Space(3)]public string unityInterstitialVideoString;
  // [Space(3)]public string unityRewardedString;
 
 public  enum SmallBanner
 {
  TopLeft,
  TopRight,
  Top,
  BottomLeft,
  BottomRight,
  Bottom
 }


 public enum LargeBanner
 {
  TopLeft,
  TopRight,
  Top,
  BottomLeft,
  BottomRight
 }

 
 
 [ContextMenu("CopyData")]
 public void CopyDataTestAds()
 {
   admobBannerId = "ca-app-pub-3940256099942544/6300978111";
   admobLargeBannerId = "ca-app-pub-3940256099942544/6300978111";
   interstitialMainId = interstitialGameId = interstitialRewardedId= "ca-app-pub-3940256099942544/1033173712";
   admobRewardedVideoId = "ca-app-pub-3940256099942544/5224354917";
 }
 
}
