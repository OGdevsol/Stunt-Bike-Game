using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
  
    #region Instance
 
    private static InventoryController _instance;

    public static InventoryController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryController>();
            }

            return _instance;
        }
    }
 
    #endregion


    public InventoryItem[] InventoryItems;
    public Sprite[] Items;
    public TextMeshProUGUI _handlingText, _gripText, _speedText;
    public Slider _handlingSlider, _gripSlider, _speedSlider;
    public Slider _estimatedhandlingSlider, _estimatedgripSlider, _estimatedspeedSlider;

    public int upgradedMultiplier,maxUpgrades;
    public Image ItemSprite;
    public Button Buy,BuyRewarded,UpgradeButton;
    public TextMeshProUGUI PriceText;

    public Animator _itemAnimator;

    int i,tempSelectedItem;
    int inventoryItemButtonsLength;
    
    private MainMenuController _mainMenuController;
    private MenuController _MenuController;
    private DataController _dataController;
    private LoadingController _loadingController;
    private RewardSystem _rewardSystem;

    private void Awake()
    {
        _instance = this;
    }


    private void Start()
    {
        _mainMenuController = MainMenuController.instance;
        _MenuController = MenuController.instance;
        _dataController = DataController.instance;
        _loadingController = LoadingController.instance;
        _rewardSystem=RewardSystem.instance;    
        
        setStatsPref();
    }

  

    public void UnlockingInventoryItem()
    {
        inventoryItemButtonsLength = InventoryItems.Length;

        if (inventoryItemButtonsLength <= 0) return;

        for (i = 0; i < inventoryItemButtonsLength; i++)
        {
            if (_dataController.GetUnlockInventoryItem(i))
            {
                InventoryItems[i].inventoryItemlockButtons.SetActive(false);
            }
            else
            {
                InventoryItems[i].inventoryItemlockButtons.SetActive(true);
            }
        }
    }

    int j,itemsLength;

    public void InventoryItemButtonClick(int i)
    {
        SelectInventoryItem(i,true);
    }

    public void SelectInventoryItem(int i, bool t)
    {
        if (t)
        {
            if (tempSelectedItem == i)
                return;
        }

        itemsLength = Items.Length;
        
    //    --------------- For 3D Items -----------------
    
//        for (; j < itemsLength; j++)
//        {
//            Items[j].SetActive(false);
//        }
        tempSelectedItem = i;
     //   Items[tempSelectedItem].SetActive(true);
     
     for (i = 0; i < inventoryItemButtonsLength; i++)
     {
         InventoryItems[i].selectedImage.SetActive(false);
     }

        StartCoroutine(showCaseItem());
     
        

        InventoryItems[tempSelectedItem].selectedImage.SetActive(true);

        if (_dataController.GetUnlockInventoryItem(tempSelectedItem))
        {
            ApplicationController.SelectedInventoryItem = tempSelectedItem;
            _MenuController.ItemSprite.sprite = Items[tempSelectedItem];
                
                
                
            Buy.gameObject.SetActive(false);
            BuyRewarded.gameObject.SetActive(false);
        }
        else
        {
            Buy.gameObject.SetActive(true);
            BuyRewarded.gameObject.SetActive(true);

            PriceText.text=InventoryItems[tempSelectedItem].price.ToString();
        }
        
        setStats(tempSelectedItem);
        InventoryItems[tempSelectedItem].BgImage.GetComponent<Selectable>().Select();

    }

    public void cancelInventoryItem()
    {
        SelectInventoryItem(ApplicationController.SelectedInventoryItem,false);
    }

    public void UnlockInventoryItem()
    {
        _dataController.SetUnlockInventoryItem(tempSelectedItem);
        _dataController.RemoveCoins(InventoryItems[tempSelectedItem].price);
        InventoryItems[tempSelectedItem].inventoryItemlockButtons.SetActive(false);
        ApplicationController.SelectedInventoryItem = tempSelectedItem;
        Buy.gameObject.SetActive(false);
        BuyRewarded.gameObject.SetActive(false);
    }

    public void BuyInventoryItem()
    {
        if (_dataController.coins >= InventoryItems[tempSelectedItem].price)
        {
            UnlockInventoryItem();
        }
        else
        {
            _mainMenuController.AddPanelToStackAndLoad(8);
        }
    }

    public void BuyInventoryItemRewarded()
    {
        _rewardSystem.ShowRewarded(ScriptLocalization.BikeRewarded);
    }
    

    
    private int _handlingValue, _gripValue, _speedValue;
    public void setStats(int item)
    {
         _handlingValue = calculateUpgradeValue(InventoryItems[ item].Handling , _dataController.getUpgradedLevel( item));
         _gripValue = calculateUpgradeValue(InventoryItems[ item].Grip , _dataController.getUpgradedLevel( item));
         _speedValue = calculateUpgradeValue(InventoryItems[ item].Speed , _dataController.getUpgradedLevel( item));
         
         _handlingSlider.DOValue(_handlingValue,1f,false);
         _gripSlider.DOValue(_gripValue,1f,false);
         _speedSlider.DOValue( _speedValue,1f,false);
         
         _handlingText.text = _handlingValue.ToString();
         _gripText.text = _gripValue.ToString();
         _speedText.text = _speedValue.ToString();
         
        if (_dataController.getUpgradedLevel(item) < maxUpgrades)
        {
            _estimatedhandlingSlider.DOValue( calculateUpgradeValue(InventoryItems[ item].Handling , _dataController.getUpgradedLevel( item)+1),1f,false);
            _estimatedgripSlider.DOValue( calculateUpgradeValue(InventoryItems[ item].Grip , _dataController.getUpgradedLevel( item)+1),1f,false);
            _estimatedspeedSlider.DOValue(calculateUpgradeValue(InventoryItems[ item].Speed , _dataController.getUpgradedLevel( item)+1),1f,false);
        }
        else
        {
            _estimatedgripSlider.gameObject.SetActive(false);
            _estimatedhandlingSlider.gameObject.SetActive(false);
            _estimatedspeedSlider.gameObject.SetActive(false);
            UpgradeButton.interactable = false;
        }
    }

    public int calculateUpgradeValue(int upgradetype , int currentUpgradedLevel )
    {
        return upgradetype + (upgradedMultiplier * currentUpgradedLevel);
    }
    
    public void setStatsPref()
    {
        if(!_dataController.getPrefBool(ScriptLocalization.UpgradeMulitplier))
            _dataController.setPref(ScriptLocalization.UpgradeMulitplier,upgradedMultiplier);


        for (int k = 0; k < InventoryItems.Length; k++)
        {
            
            if(!_dataController.getPrefBool(String.Concat( ScriptLocalization.BikeHandling,k)))
                _dataController.setPref(String.Concat( ScriptLocalization.BikeHandling,k),InventoryItems[k].Handling); 
        
            if(!_dataController.getPrefBool(String.Concat(ScriptLocalization.BikeGrip,k)))
                _dataController.setPref(String.Concat(ScriptLocalization.BikeGrip,k),InventoryItems[k].Grip);
        
            if(!_dataController.getPrefBool(String.Concat(ScriptLocalization.BikeSpeed,k)))
                _dataController.setPref(String.Concat(ScriptLocalization.BikeSpeed,k),InventoryItems[k].Speed); 
        }
        
    }
    public void Display()
    {
        UnlockingInventoryItem();
        SelectInventoryItem(ApplicationController.SelectedInventoryItem,false);
        _mainMenuController.AddPanelToStackAndLoad(3);
       
    }

    public void Play()
    {
        _loadingController.display(Scenes.Gameplay);
        //if (Application.internetReachability != NetworkReachability.NotReachable && AdsController.Instance != null)
       // {
           // AdsController.Instance._showInterstitialGame();
      //  }
    }

    public IEnumerator showCaseItem()
    {
        _itemAnimator.Play("ItemHide");
            yield return new WaitForSeconds(.5f);
            ItemSprite.sprite= Items[tempSelectedItem]; 
            _itemAnimator.Play("ItemShow");
        
    }

    public void UpgradRewarded()
    {
        if (_dataController.getUpgradedLevel(tempSelectedItem) < maxUpgrades)
            _rewardSystem.ShowRewarded(ScriptLocalization.UpgradeBike);
    
}
    
    public void UnlockAllInventoryItem()
    {
        for (i = 0; i < InventoryItems.Length; i++)
        {
            _dataController.SetUnlockInventoryItem(i);
        }
        Buy.gameObject.SetActive(false);
        BuyRewarded.gameObject.SetActive(false);

        UnlockingInventoryItem();
        ApplicationController.SelectedInventoryItem = tempSelectedItem;
        DataController.instance.setRewardPref(ScriptLocalization.AllBikes,1);
    }
    
    
}

[Serializable]
public class InventoryItem
{
    public Image BgImage;
    public GameObject selectedImage;
    public GameObject inventoryItemlockButtons;
    public int price;
    public int Speed,Handling,Grip;
    
}