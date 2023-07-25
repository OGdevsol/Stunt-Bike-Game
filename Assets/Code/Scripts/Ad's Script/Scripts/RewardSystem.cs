using System.Collections;
using System.Collections.Generic;
//using Ad_Plugin_Data.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class RewardSystem : MonoBehaviour
{
   
    
    #region Instance
 
    private static RewardSystem _instance;

    public static RewardSystem instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RewardSystem>();
            }

            return _instance;
        }
    }
 
    #endregion

  //  private AdsController AdsController;
    private DataController DataController;


    private void Awake()
    {
        _instance = this;
    }
    
    private void Start()
    {
      //  AdsController = AdsController.Instance;
        DataController = DataController.instance;
       
    }

    public void ShowRewarded(string rewardName)
    {
      //  AdsController.Instance.ShowRewarded();
        DataController.setRewardPref(rewardName,1);
    }
    
    public void AddCoin(int coins)
    {
        DataController.instance.AddCoins(coins);
    }
    
    public void UpgradeBike()
    {
        DataController.instance.setUpgradedLevel(ApplicationController.SelectedInventoryItem);
        InventoryController.instance.SelectInventoryItem(ApplicationController.SelectedInventoryItem,false);
    }

    public void UnlockAllLevel()
    {
        LevelMenuController.instance.UnlockAllLevelReward();
    }

    public void UnlockAllBike()
    {
        InventoryController.instance.UnlockAllInventoryItem();
    }
    
    public void UnlockAllGame()
    {
       // ModeSelectController.instance.UnlockAllGameReward();
    }
    
    public void UnlockBike()
    {
        InventoryController.instance.UnlockInventoryItem();
    }
    
    public void UnlockRemoveAd()
    {
        MenuController.instance.RemoveAdWatched();
    }
    public void CustomUnlockAllGame()
    {
        DataController.instance.setPref(ScriptLocalization.AllGameRemainingVideo, ModeSelectController.instance.UnlockAllGameMaxVideos);
     //   ModeSelectController.instance.UnlockAllGameReward();
    }
}
