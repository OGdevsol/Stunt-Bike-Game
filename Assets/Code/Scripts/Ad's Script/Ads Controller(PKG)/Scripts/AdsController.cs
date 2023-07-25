using System;
using System.Globalization;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsController : MonoBehaviour  //, IUnityAdsInitializationListener, IUnityAdsLoadListener,
  //  IUnityAdsShowListener
{
    #region Instance

    private static AdsController _instance;

    public static AdsController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AdsController>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    #endregion

    #region Data Members / Veriables

    public Ad_Ids adIds;

    ///////////////////////////// Admob 

    private bool _isAdsInitialized;

    private BannerView bannerView1;

    private BannerView bannerView2;

    private BannerView largebannerView;

    private InterstitialAd interstitialMain;

    private InterstitialAd _interstitialGame;

    private RewardedInterstitialAd interstitialMainRewarded;

    private RewardedAd rewardBasedVideo;


    private AppOpenAd ad;

    public bool showDebugs;

    [HideInInspector] public bool gameplay, complete, skip, isInterstitialMainLoaded, isInterstitialGameLoaded;

    #endregion


    #region Initilization

    private void Start()
    {
        Time.timeScale = 1;
        _rewardSystem = RewardSystem.instance;
        if (Application.internetReachability == NetworkReachability.NotReachable) return;


        Invoke(nameof(InitializeAdsSdk), 0.5f);
    }


    private void InitializeAdsSdk()
    {
        if (adIds.chooseAdsNetwork == Ad_Ids.AdsNetwork.DoNotShowAds)
        {
            PlayerPrefs.SetInt("DoNotShowAds", 1);
        }

        if (adIds.chooseAdsNetwork == Ad_Ids.AdsNetwork.UnityOnly)
        {
            PlayerPrefs.SetInt("OnlyUnityADs", 1);
        //    UnityADsInitialization();
            return;
        }


        if (!PlayerPrefs.HasKey("DoNotShowAds"))
        {
            if (adIds.chooseAdsNetwork == Ad_Ids.AdsNetwork.AdmobeOnly)
            {
                PlayerPrefs.SetInt("AdmobAdsOnly", 1);
                AdmobADsInitialization();
                return;
            }

          //  UnityADsInitialization();
            AdmobADsInitialization();
        }
        else
        {
            IntializeAdmobRewardedOnly();
        }

        print("----------- " + PlayerPrefs.GetInt("DoNotShowAds"));
    }

    // #region Unity Ads Initialization
    //
    // public void UnityADsInitialization()
    // {
    //     try
    //     {
    //         Advertisement.Initialize(adIds.unityId, adIds.isUnityTestADs, this);
    //
    //         if (showDebugs)
    //             print("<color=green> Unity Ads Initializing </color>");
    //     }
    //     catch (Exception e)
    //     {
    //         if (showDebugs)
    //             print("<color=green> Unity Ads Initialization Fail </color>" + e.Message);
    //     }
    // }
    //
    // #endregion

    #region Admob Ads Initialization

    public void AdmobADsInitialization()
    {
        if (_isAdsInitialized)
        {
            return;
        }

        _isAdsInitialized = true;

        try
        {
            MobileAds.Initialize(initStatus =>
            {
                if (showDebugs)
                    print("<color=green> Admob Ads Initialized </color>");
            });


            if (adIds.AdmobOpenAppAD)
                LoadOpenAppAd();

            //ShowOpenAppAdIfAvailable();
            //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

            CreateInterstitialMain();

         //   RequestLargeBanner();

            if (adIds.AdmobOpenAppAD)
                Invoke(nameof(ShowOpenAppAdIfAvailable), 4.5f);


       //     CreateInterstitialGame();

      //      InitializeAdMobRewardedVideo();

         //   RequestInterstitialMainRewarded();


            /*if (adIds.isSecondSmallBannerRequired)
            {
                RequestSmallBanner2_Low();
                RequestSmallBanner2_Medium();
                RequestSmallBanner2_High();
            }*/
        }
        catch (Exception e)
        {
            if (showDebugs)
                print("<color=green> Admobe Ads Initialization Fail </color>" + e.Message);
        }
    }


    // Only Rewarded Ads Initialization
    private void IntializeAdmobRewardedOnly()
    {
        if (_isAdsInitialized)
        {
            return;
        }

        _isAdsInitialized = true;
        try
        {
            MobileAds.Initialize(initStatus =>
            {
                if (showDebugs)
                {
                    print("<color=green> Admob Ads Initialized </color>");
                }
            });


            InitializeAdMobRewardedVideo();

            RequestInterstitialMainRewarded();
        }
        catch (Exception e)
        {
            if (showDebugs)
                print("<color=green> Admobe Ads Initialization Fail </color>" + e.Message);
        }
    }

    #endregion

    #endregion


    // #region Unity ADs
    //
    // public void OnInitializationComplete()
    // {
    //     LoadAd();
    //
    //     if (showDebugs)
    //         print("<color=green> Unity Ads Initialized </color>");
    // }
    //
    // public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    // {
    //     if (showDebugs)
    //         print("<color=green> Unity ADs Fail to Initialize </color>");
    // }
    //
    // // Load content to the Ad Unit:
    // public void LoadAd()
    // {
    //     // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
    //
    //     try
    //     {
    //         // Advertisement.Load(adIds.unityBannerString, this);
    //         Advertisement.Load(adIds.unityInterstitialVideoString, this);
    //         Advertisement.Load(adIds.unityRewardedString, this);
    //     }
    //     catch (Exception e)
    //     {
    //         if (showDebugs)
    //             print("<color=green> ERROR </color>" + e.Message);
    //     }
    // }
    //
    // public void OnUnityAdsAdLoaded(string placementId)
    // {
    //     if (showDebugs)
    //         print("<color=green> Unity Ads Loaded </color>" + placementId);
    // }
    //
    // public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    // {
    //     if (showDebugs)
    //         print("<color=green> Unity Ads Fail to Load </color>" + placementId);
    // }
    //
    // public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    // {
    //     if (showDebugs)
    //         print("<color=green> Unity Ads Fail To Show </color>" + placementId);
    // }
    //
    // public void OnUnityAdsShowStart(string placementId)
    // {
    //     if (showDebugs)
    //         print("<color=green> Unity Ad Open </color>" + placementId);
    // }
    //
    // public void OnUnityAdsShowClick(string placementId)
    // {
    //     if (showDebugs)
    //         print("<color=green> Unity Ads Click </color>" + placementId);
    // }
    //
    // public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    // {
    //     if (placementId.Equals(adIds.unityRewardedString) &&
    //         showCompletionState == UnityAdsShowCompletionState.COMPLETED)
    //     {
    //         complete = true;
    //     }
    //
    //     if (placementId.Equals(adIds.unityInterstitialVideoString))
    //     {
    //         Advertisement.Load(adIds.unityInterstitialVideoString, this);
    //     }
    //
    //     if (showDebugs)
    //         print("<color=green> Unity Ads Completed </color>" + placementId);
    // }
    //
    // #endregion

    #region Admob ADs

    public AdRequest NewRequest()
    {
        return new AdRequest.Builder().Build();
    }


    #region Small Banner 1 Implementation / Show / Hide Functions

    private bool isSmallBanner1Loaded;

    public void RequestSmallBanner1()
    {
        /*if (PlayerPrefs.HasKey("DoNotShowAds") || PlayerPrefs.HasKey("OnlyUnityADs"))
        {
            return;
        }*/

        try
        {
            if (bannerView1 != null)
            {
                bannerView1.Destroy();
            }

            this.bannerView1 = new BannerView(adIds.admobBannerId, AdSize.SmartBanner, adIds.smallBannerPosition1);

            bannerView1.OnAdLoaded += this.SmallBanner1HandleAdLoaded;
            bannerView1.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            bannerView1.OnAdOpening += this.HandleAdOpened;
            bannerView1.OnAdClosed += this.HandleAdClosed;

            bannerView1.LoadAd(NewRequest());
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public void SmallBanner1HandleAdLoaded(object sender, EventArgs args)
    {
        isSmallBanner1Loaded = true;

        if (showDebugs)
            print("<color=green>Calling :- Small Banner Ad Loaded </color>");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isSmallBanner1Loaded = false;
        if (showDebugs)
            print("<color=green>Calling :- Small Banner Ad Load Fail </color>");
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        isSmallBanner1Loaded = false;
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        isSmallBanner1Loaded = false;
    }


    // private AdPosition _positionSmallBanner1;

    /*public void RequestSmallBanner1()
    {
        if (PlayerPrefs.HasKey("DoNotShowAds") || PlayerPrefs.HasKey("OnlyUnityADs"))
        {
            return;
        }

        if (adIds.banner1Position == Ad_Ids.SmallBanner.TopLeft)
        {
            _positionSmallBanner1 = AdPosition.TopLeft;
            CreateSmallBanner1();
        }

        if (adIds.banner1Position == Ad_Ids.SmallBanner.TopRight)
        {
            _positionSmallBanner1 = AdPosition.TopRight;
            CreateSmallBanner1();
        }

        if (adIds.banner1Position == Ad_Ids.SmallBanner.Top)
        {
            _positionSmallBanner1 = AdPosition.Top;
            CreateSmallBanner1();
        }

        if (adIds.banner1Position == Ad_Ids.SmallBanner.Bottom)
        {
            _positionSmallBanner1 = AdPosition.Bottom;
            CreateSmallBanner1();
        }

        if (adIds.banner1Position == Ad_Ids.SmallBanner.BottomLeft)
        {
            _positionSmallBanner1 = AdPosition.BottomLeft;
            CreateSmallBanner1();
        }

        if (adIds.banner1Position == Ad_Ids.SmallBanner.BottomRight)
        {
            _positionSmallBanner1 = AdPosition.BottomRight;
            CreateSmallBanner1();
        }
        

        if (showDebugs)
            Debug.Log("<color=green>Calling :- Create Banner1 Request </color>" + adIds.banner1Position.ToString());
    }*/


    public void _showsmallBanner()
    {
        /*if (PlayerPrefs.HasKey("OnlyUnityADs"))
        {
            ShowUnityBanner();
            return;
        }*/


        RequestSmallBanner1();

        if (showDebugs)
            print("<color=green>Calling :- Show Small Banner 1 </color>");
    }


    public void _hidesmallBanner()
    {
        try
        {
            /*if (PlayerPrefs.HasKey("OnlyUnityADs"))
            {
                HideUnityBanner();
                return;
            }*/

            bannerView1?.Hide();

            if (showDebugs)
                print("<color=green>Calling :- Hide Small Banner 1 </color>");
        }
        catch (Exception)
        {
            // ignored
        }
    }

    #endregion


    #region LargeBanner Implementation callback handlers / Show / Hide

    private bool isLargeBannerLoaded;


    #region Large Banner

    private void RequestLargeBanner()
    {
        try
        {
            if (largebannerView != null)
            {
                largebannerView.Destroy();
            }

            largebannerView = new BannerView(adIds.admobLargeBannerId, AdSize.MediumRectangle,
                adIds.largeBannerPosition);

            largebannerView.OnAdLoaded += this.LargeBannerHandleLargeAdLoaded;
            largebannerView.OnAdFailedToLoad += this.LargeBannerHandleLargeAdFailedToLoad;
            largebannerView.OnAdOpening += this.LargeBannerHandleLargeAdOpened;
            largebannerView.OnAdClosed += this.LargeBannerHandleLargeAdClosed;

            AdRequest request = new AdRequest.Builder().Build();
            largebannerView.LoadAd(request);
            largebannerView.Hide();
        }
        catch (Exception)
        {
            // ignored
        }
    }


    private void RequestShowCustomeLargeBanner(AdPosition customPosition)
    {
        try
        {
            if (largebannerView != null)
            {
                largebannerView.Destroy();
            }

            largebannerView = new BannerView(adIds.admobLargeBannerId, AdSize.MediumRectangle,
                customPosition);

            largebannerView.OnAdLoaded += this.LargeBannerHandleLargeAdLoaded;
            largebannerView.OnAdFailedToLoad += this.LargeBannerHandleLargeAdFailedToLoad;
            largebannerView.OnAdOpening += this.LargeBannerHandleLargeAdOpened;
            largebannerView.OnAdClosed += this.LargeBannerHandleLargeAdClosed;

            AdRequest request = new AdRequest.Builder().Build();
            largebannerView.LoadAd(request);
        }
        catch (Exception)
        {
            // ignored
        }
    }


    public void LargeBannerHandleLargeAdLoaded(object sender, EventArgs args)
    {
        isLargeBannerLoaded = true;

        if (showDebugs)
            print("<color=green>Calling :- Large Banner Ad Loaded </color>");
    }

    public void LargeBannerHandleLargeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isLargeBannerLoaded = false;

        if (showDebugs)
            print("<color=green>Calling :- Large Banner Ad Load Fail </color>");
    }

    public void LargeBannerHandleLargeAdOpened(object sender, EventArgs args)
    {
        isLargeBannerLoaded = false;
    }

    public void LargeBannerHandleLargeAdClosed(object sender, EventArgs args)
    {
        isLargeBannerLoaded = false;
    }

    #endregion


    public void _showlargeBanner()
    {
        // if (PlayerPrefs.HasKey("OnlyUnityADs"))
        // {
        //     return;
        // }
        //
        // if (largebannerView == null)
        // {
        //     AdmobADsInitialization();
        //     return;
        // }
        //
        // largebannerView?.Show();
        //
        // if (showDebugs)
        //     print("<color=green>Calling :- Show Large Banner </color>");
    }

    public void _hidelargeBanner()
    {
        try
        {
            if (PlayerPrefs.HasKey("OnlyUnityADs"))
            {
                return;
            }

            if (largebannerView == null)
            {
                AdmobADsInitialization();
                return;
            }

            largebannerView?.Hide();

            if (showDebugs)
                print("<color=green>Calling :- HideAdMobBannerLarge </color>");
        }
        catch (Exception)
        {
            // ignored
        }
    }

    #endregion


    #region Interstitial Main callback handlers

    public void CreateInterstitialMain()
    {
        try
        {
            this.interstitialMain = new InterstitialAd(adIds.interstitialMainId);
            this.interstitialMain.OnAdLoaded += this.HandleInterstitialMainLoaded;
            this.interstitialMain.OnAdFailedToLoad += this.HandleInterstitialMainFailedToLoad;
            this.interstitialMain.OnAdOpening += this.HandleInterstitialMainOpened;
            this.interstitialMain.OnAdClosed += this.HandleInterstitialMainClosed;

            RequestInterstitialMain();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public void RequestInterstitialMain()
    {
        try
        {
            if (!interstitialMain.IsLoaded())
            {
                interstitialMain.LoadAd(NewRequest());

                if (showDebugs)
                    print("<color=green>Calling :- Request interstitial  </color>");
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public void HandleInterstitialMainLoaded(object sender, EventArgs args)
    {
        isInterstitialMainLoaded = true;

        if (showDebugs)
            print("<color=green>Calling :- Loaded interstitial Main </color>");
    }

    public void HandleInterstitialMainFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isInterstitialMainLoaded = false;

        if (showDebugs)
            print("<color=green>Calling :- FAILED interstitial Main </color>");
    }

    public void HandleInterstitialMainOpened(object sender, EventArgs args)
    {
        isInterstitialMainLoaded = false;
    }

    public void HandleInterstitialMainClosed(object sender, EventArgs args)
    {
        isInterstitialMainLoaded = false;
    }

    #endregion


    #region Interstitial Game callback handlers

    public void CreateInterstitialGame()
    {
        try
        {
            if (this._interstitialGame != null)
            {
                this._interstitialGame.Destroy();
            }

            this._interstitialGame = new InterstitialAd(adIds.interstitialGameId);

            this._interstitialGame.OnAdLoaded += this.HandleInterstitialGameLoaded;
            this._interstitialGame.OnAdFailedToLoad += this.HandleInterstitialGameFailedToLoad;
            this._interstitialGame.OnAdOpening += this.HandleInterstitialGameOpened;
            this._interstitialGame.OnAdClosed += this.HandleInterstitialGameClosed;
            RequestInterstitialGame();
        }

        catch (Exception)
        {
            // ignored
        }
    }

    public void RequestInterstitialGame()
    {
        try
        {
            _interstitialGame.LoadAd(NewRequest());
        }

        catch (Exception)
        {
            // ignored
        }
    }

    public void HandleInterstitialGameLoaded(object sender, EventArgs args)
    {
        isInterstitialGameLoaded = true;

        if (showDebugs)
            print("<color=green>Calling :- Loaded interstitial Game </color>");
    }

    public void HandleInterstitialGameFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        isInterstitialGameLoaded = false;

        if (showDebugs)
            print("<color=green>Calling :- Fail To Load interstitial Game </color>");
    }

    public void HandleInterstitialGameOpened(object sender, EventArgs args)
    {
    }

    public void HandleInterstitialGameClosed(object sender, EventArgs args)
    {
    }

    #endregion


    #region Interstitial Main Rewarded callback handlers

    public void RequestInterstitialMainRewarded()
    {
        try
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the rewarded ad High with the request.

            interstitialMainRewarded = null;

            if (interstitialMainRewarded == null)
            {
                RewardedInterstitialAd.LoadAd(adIds.interstitialRewardedId, request, AdLoadCallback);

                if (showDebugs)
                    print("<color=green>Calling :-  Interstitial Rewarded Load Request   </color>");
            }
        }

        catch (Exception)
        {
            // ignored
        }
    }

    private void AdLoadCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs eventArgs)
    {
        interstitialMainRewarded = ad;

        if (showDebugs)
            print("<color=green>Calling :-  Interstitial Rewarded Loaded  </color>" + ad.GetRewardItem());
    }


    private void UserEarnedRewardCallback(Reward reward)
    {
        // On Complete Ads Give Reward

        complete = true;

        if (showDebugs)
            print("<color=green>Calling :-  Interstitial Rewarded Complete   </color>");
    }

    #endregion


    #region RewardBasedVideo callback handlers

    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: " + args);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        skip = true;
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        complete = true;

        Debug.Log("HandleRewardBasedVideoRewarded event received for " + amount.ToString(CultureInfo.InvariantCulture) +
                  " " + type);
    }

    #endregion

    public void InitializeAdMobRewardedVideo()
    {
        // Create rewarded Ad High

        this.rewardBasedVideo = new RewardedAd(adIds.admobRewardedVideoId);
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        // this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnUserEarnedReward += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;


        RequestRewardBasedVideo();
    }

    public void RequestRewardBasedVideo()
    {
        try
        {
            AdRequest request = new AdRequest.Builder().Build();
            rewardBasedVideo.LoadAd(request);
            if (showDebugs)
            {
                print("<color=green> Request Admob Rewarded </color>");
            }
        }
        catch (Exception e)
        {
            if (showDebugs)
            {
                print("<color=green> Request Admob Rewarded Exception </color>" + e.Message);
            }
        }
    }

    #endregion


    #region OpenAppAd

    private bool IsAdAvailable
    {
        get { return ad != null; }
    }

    private bool isShowingAd = false;

    public void LoadOpenAppAd()
    {
        AdRequest request = new AdRequest.Builder().Build();

        // Load an app open ad for portrait orientation
        AppOpenAd.LoadAd(adIds.admobAppOpenId, ScreenOrientation.AutoRotation, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                return;
            }

            //// App open ad is loaded.
            ad = appOpenAd;
            print(ad + "adddddddddddd");
        }));
    }

    public void ShowOpenAppAdIfAvailable()
    {
        print(IsAdAvailable + " IsAdAvailable");
        if (!IsAdAvailable || isShowingAd)
        {
            return;
        }

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;

        ad.Show();
        print("showingggg");
    }
    //private void OnAppStateChanged(AppState state)
    //{
    //    // Display the app open ad when the app is foregrounded.
    //    UnityEngine.Debug.Log("App State is " + state);
    //    if (state == AppState.Foreground)
    //    {
    //        ShowOpenAppAdIfAvailable();
    //    }
    //}


    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        if (showDebugs)
            Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadOpenAppAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        if (showDebugs)
            Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadOpenAppAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        if (showDebugs)
            Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        if (showDebugs)
            Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        if (showDebugs)
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.AdValue.CurrencyCode, args.AdValue.Value);
    }

    #endregion

    #endregion


    #region Methods To Call/Show Ads

    // #region Unity Ads Call Methods
    //
    // public void ShowUnityVideo()
    // {
    //     if (PlayerPrefs.HasKey("AdmobAdsOnly")) return;
    //
    //     try
    //     {
    //         if (Advertisement.isInitialized)
    //         {
    //             Advertisement.Show(adIds.unityInterstitialVideoString, this);
    //
    //             Advertisement.Load(adIds.unityInterstitialVideoString, this);
    //         }
    //         else
    //         {
    //             Advertisement.Initialize(adIds.unityId, adIds.isUnityTestADs, this);
    //         }
    //
    //
    //         if (showDebugs)
    //             print("<color=green> ShowUnityVideo </color>");
    //     }
    //     catch (Exception)
    //     {
    //         // ignored
    //     }
    // }
    //
    //
    // public void ShowUnityBanner()
    // {
    //     if (PlayerPrefs.HasKey("DoNotShowAds")) return;
    //
    //     try
    //     {
    //         if (Advertisement.isInitialized)
    //         {
    //             Advertisement.Banner.SetPosition(adIds.unitySmallBannerPosition);
    //             Advertisement.Banner.Show(adIds.unityBannerString);
    //         }
    //         else
    //         {
    //             Advertisement.Initialize(adIds.unityId, adIds.isUnityTestADs, this);
    //         }
    //
    //         if (showDebugs)
    //             print("<color=green> ShowUnity Banner </color>");
    //     }
    //     catch (Exception)
    //     {
    //         // ignored
    //     }
    // }
    //
    //
    // public void HideUnityBanner()
    // {
    //     if (PlayerPrefs.HasKey("DoNotShowAds")) return;
    //
    //
    //     try
    //     {
    //         Advertisement.Banner.Hide();
    //
    //         if (showDebugs)
    //             print("<color=green> ShowUnity Banner </color>");
    //     }
    //     catch (Exception)
    //     {
    //         // ignored
    //     }
    // }
    //
    //
    // public void ShowUnityRewarded()
    // {
    //     try
    //     {
    //         if (Advertisement.isInitialized)
    //         {
    //             Advertisement.Show(adIds.unityRewardedString, this);
    //
    //             Advertisement.Load(adIds.unityRewardedString, this);
    //         }
    //         else
    //         {
    //             Advertisement.Initialize(adIds.unityId, adIds.isUnityTestADs, this);
    //         }
    //
    //         if (showDebugs)
    //             Debug.Log("<color=green>Calling :- UnityRewardedVideo </color>");
    //     }
    //     catch (Exception)
    //     {
    //         // ignored
    //     }
    // }
    //
    // #endregion


    #region Admobe Ads call Methods

    public void ShowRewardedInterstitialAd()
    {
        if (interstitialMainRewarded != null)
        {
            interstitialMainRewarded.Show(UserEarnedRewardCallback);
            if (showDebugs)
                print("<color=green>Calling :- Show AdMob Rewarded Interstitial </color>");
        }
    }


    public void ShowAdmobRewardedVideo()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }

        try
        {
            if (this.rewardBasedVideo.IsLoaded())
            {
                this.rewardBasedVideo.Show();
                if (showDebugs)
                    print("<color=green>Calling :- Show AdMob Rewarded </color>");
            }

            RequestRewardBasedVideo();
        }

        catch (Exception)
        {
            // ignored
        }
    }


    public void Show_Admob_InterstitialMain()
    {
        try
        {
            // if (PlayerPrefs.HasKey("OnlyUnityADs"))
            // {
            //     ShowUnityVideo();
            //     return;
            // }


            if (isInterstitialMainLoaded)
            {
                this.interstitialMain.Show();

                isInterstitialMainLoaded = false;

                if (showDebugs)
                    print("<color=green>Calling :- Show AdMob Interstitial </color>");
            }

            RequestInterstitialMain();
        }
        catch (Exception)
        {
            // ignored
        }
    }


    public void Show_Admob_InterstitialGame()
    {
        try
        {
            // if (PlayerPrefs.HasKey("OnlyUnityADs"))
            // {
            //     ShowUnityVideo();
            //     return;
            // }

            if (this._interstitialGame.IsLoaded())
            {
                this._interstitialGame.Show();
            }

            RequestInterstitialGame();

            if (showDebugs)
                print("<color=green>Calling :- Show AdMob Interstitial </color>");
        }
        catch (Exception)
        {
            // ignored
        }
    }

    #endregion


    public void ShowRewarded()
    {
        try
        {
            // if (PlayerPrefs.HasKey("OnlyUnityADs"))
            // {
            //     ShowUnityRewarded();
            //     return;
            // }
            //
            // if (PlayerPrefs.HasKey("AdmobAdsOnly"))
            // {
            //     if (rewardBasedVideo == null)
            //     {
            //         AdmobADsInitialization();
            //         return;
            //     }
            //
            //     if (this.rewardBasedVideo.IsLoaded())
            //     {
            //         this.rewardBasedVideo.Show();
            //         if (showDebugs)
            //             print("<color=green>Calling :- Show AdMob Rewarded </color>");
            //     }
            //     else if (interstitialMainRewarded != null)
            //     {
            //         interstitialMainRewarded.Show(UserEarnedRewardCallback);
            //         RequestInterstitialMainRewarded();
            //
            //         if (showDebugs)
            //             print("<color=green>Calling :- Show AdMob Rewarded Interstitial  </color>");
            //     }
            //
            //     RequestRewardBasedVideo();
            //
            //     if (showDebugs)
            //         print("<color=green>Calling :- Rewarded Admob Only   </color>");
            //     return;
            // }


            if (rewardBasedVideo == null)
            {
                AdmobADsInitialization();
            }


            if (this.rewardBasedVideo.IsLoaded())
            {
                this.rewardBasedVideo.Show();

                RequestRewardBasedVideo();
                if (showDebugs)
                    print("<color=green>Calling :- Show AdMob Rewarded  </color>");
            }
            else if (interstitialMainRewarded != null)
            {
                interstitialMainRewarded.Show(UserEarnedRewardCallback);
                RequestInterstitialMainRewarded();
                if (showDebugs)
                    print("<color=green>Calling :- Show AdMob Rewarded Interstitial </color>");
            }
            else
            {
              //  ShowUnityRewarded();

                RequestRewardBasedVideo();

                if (showDebugs)
                    Debug.Log("<color=green>Calling :- Unity Rewarded Video </color>");
            }


            if (showDebugs)
                print("<color=green>Calling :-  Unity + Admob Rewarded  </color>");
        }
        catch (Exception)
        {
            // ignored
        }
    }


    #region GameFail + Complete + Pause Ad-Calls Function

    private int _y = 0;

    public void _showInterstitialGame()
    {
        // if (PlayerPrefs.HasKey("OnlyUnityADs"))
        // {
        //     ShowUnityVideo();
        //     return;
        // }
        //
        //
        // if (PlayerPrefs.HasKey("AdmobAdsOnly"))
        // {
        //     if (interstitialMain == null || _interstitialGame == null)
        //     {
        //         AdmobADsInitialization();
        //         return;
        //     }
        //
        //     if (isInterstitialGameLoaded)
        //     {
        //         _interstitialGame.Show();
        //         isInterstitialGameLoaded = false;
        //         RequestInterstitialGame();
        //     }
        //     else
        //     {
        //         Show_Admob_InterstitialMain();
        //
        //         RequestInterstitialMain();
        //     }
        //
        //     if (showDebugs)
        //         print("<color=green>Calling :- Show AdMob Interstitial Only </color>");
        //
        //     return;
        // }

        if (interstitialMain == null || _interstitialGame == null)
        {
            AdmobADsInitialization();
        }


        if (isInterstitialGameLoaded)
        {
            _interstitialGame.Show();
            isInterstitialGameLoaded = false;
            RequestInterstitialGame();
        }
        else if (isInterstitialMainLoaded)
        {
            interstitialMain.Show();
            isInterstitialMainLoaded = false;
            RequestInterstitialMain();
        }
        else
        {
           // ShowUnityVideo();

            RequestInterstitialMain();
        }

        if (showDebugs)
            print("<color=green>Calling :- Show Interstitial Shuffle </color>");
    }

    #endregion

    #endregion


    #region OnComplete Reward

    private Text text;
    private RewardSystem _rewardSystem;

    private void OnCompleteRewarded()
    {
        complete = false;
        try
        {
            if (InGameController.instance)
            {
                if (InGameController.instance.NitroRwd)
                {
                    InGameController.nitroTimeLeft = 5f;
                    InGameController.instance.NitroRwd = false;
                //    InGameController.instance.nitroPressed();
                }
            }
            
            
            if (DataController.instance.getRewardPref(ScriptLocalization.AddCoin))
            {
                _rewardSystem.AddCoin(500);
                DataController.instance.setRewardPref(ScriptLocalization.AddCoin,0);
            }

            if (DataController.instance.getRewardPref(ScriptLocalization.UpgradeBike))
            {
                _rewardSystem.UpgradeBike();
                DataController.instance.setRewardPref(ScriptLocalization.UpgradeBike,0);
            }
        
            if (DataController.instance.getRewardPref(ScriptLocalization.UnlockAllLevel))
            {
                _rewardSystem.UnlockAllLevel();
                DataController.instance.setRewardPref(ScriptLocalization.UnlockAllLevel,0);
            }
        
            if (DataController.instance.getRewardPref(ScriptLocalization.UnlockFullGame))
            {
                _rewardSystem.UnlockAllGame();
                DataController.instance.setRewardPref(ScriptLocalization.UnlockFullGame,0);
            }
        
            if (DataController.instance.getRewardPref(ScriptLocalization.BikeRewarded))
            {
                _rewardSystem.UnlockBike();
                DataController.instance.setRewardPref(ScriptLocalization.BikeRewarded,0);
            }
            if (DataController.instance.getRewardPref(ScriptLocalization.RemoveAdRewarded))
            {
                _rewardSystem.UnlockRemoveAd();
                DataController.instance.setRewardPref(ScriptLocalization.RemoveAdRewarded,0);
            }
            complete = false;
        }
        catch (Exception)
        {
            complete = false;
        }
    }

    #endregion


    #region OnSkip Reward

    public void Onskip()
    {
        skip = false;

        try
        {
            if (DataController.instance.getRewardPref(ScriptLocalization.AddCoin))
            {
                DataController.instance.setRewardPref(ScriptLocalization.AddCoin,0);
            }

            if (DataController.instance.getRewardPref(ScriptLocalization.UpgradeBike))
            {
                DataController.instance.setRewardPref(ScriptLocalization.UpgradeBike,0);
            }
            if (DataController.instance.getRewardPref(ScriptLocalization.UnlockAllLevel))
            {
                DataController.instance.setRewardPref(ScriptLocalization.UnlockAllLevel,0);
            }
            if (DataController.instance.getRewardPref(ScriptLocalization.UnlockFullGame))
            {
                DataController.instance.setRewardPref(ScriptLocalization.UnlockFullGame,0);
            }
            if (DataController.instance.getRewardPref(ScriptLocalization.BikeRewarded))
            {
                DataController.instance.setRewardPref(ScriptLocalization.BikeRewarded,0);
            }
            skip = false;
        }
        catch (Exception)
        {
            skip = false;
        }
        skip = false;
    }

    #endregion

    public void Update()
    {
        if (complete)
        {
            OnCompleteRewarded();
            complete = false;
            skip = false;
        }

        if (skip)
        {
            Onskip();
            complete = false;
            skip = false;
        }
    }

  
}

