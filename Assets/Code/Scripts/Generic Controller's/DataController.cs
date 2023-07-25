
using UnityEngine;

public class DataController : MonoBehaviour
{
    
    #region Instance
 
    private static DataController _instance;

    public static DataController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataController>();
            }

            return _instance;
        }
    }
 
    #endregion
    
    private void Awake()
    {
        _instance = this;
  
    }

    #region Coins

    public int coins
    {
        get
        {
            if (_coins < 0)
            {
                _coins = PlayerPrefs.GetInt("coins", 0);
            }
            return _coins;
        }
    }
    
    public int AddCoins(int coin )
    {
        _coins = coins + coin;
        PlayerPrefs.SetInt("coins", _coins);
        coinsChanged?.Invoke();
        return _coins;
    }
    
    public int RemoveCoins(int coin)
    {
        _coins = coins - coin;
        PlayerPrefs.SetInt("coins", _coins);
        coinsChanged?.Invoke();
        return _coins;
    }
    
    #endregion
    
    #region Gold

    public int golds
    {
        get
        {
            if (_gold < 0)
            {
                _gold = PlayerPrefs.GetInt("gold", 0);
            }
            return _coins;
        }
    }
    
    public int AddGold(int gold )
    {
        _gold = golds + gold;
        PlayerPrefs.SetInt("gold", _gold);
        goldChanged?.Invoke();
        return _gold;
    }
    
    public int RemoveGold(int gold)
    {
        _gold = golds - gold;
        PlayerPrefs.SetInt("gold", _gold);
        goldChanged?.Invoke();
        return _gold;
    }
    
    #endregion

    #region Levels / Modes
    
    public void SetUnlockLevel(int levelID)
    {
        PlayerPrefs.SetInt(string.Concat("M"+ lastSelectedGameMode +"levelUnlocked", levelID + 1), 1);
    }
    public bool GetUnlockLevel(int levelID)
    {
        if (levelID == 0)
        {
            PlayerPrefs.SetInt(string.Concat("M"+ lastSelectedGameMode +"levelUnlocked", levelID), 1);
        }   
        return  PlayerPrefs.HasKey(string.Concat("M"+ lastSelectedGameMode +"levelUnlocked", levelID));
    }
    
    public static int lastSelectedLevel
    {
        get
        {
            return PlayerPrefs.GetInt("M" + lastSelectedGameMode + "lastSelectedLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("M" + lastSelectedGameMode + "lastSelectedLevel", value);
        }
    }

    public static int lastSelectedGameMode
    {
        get
        {
            return PlayerPrefs.GetInt("lastSelectedGameMode", 0);
        }
        set
        {
            PlayerPrefs.SetInt("lastSelectedGameMode", value);
        }
    }
    
    
    #endregion

    #region Inventory

    
 public static int lastSelectedInventoryItem
    {
        get
        {
            return PlayerPrefs.GetInt("lastSelectedInventoryItem", 0);
        }
        set
        {
            PlayerPrefs.SetInt("lastSelectedInventoryItem", value);
        }
    }
 
 public bool GetUnlockInventoryItem(int itemID)
 {
     if (itemID == 0)
     {
         PlayerPrefs.SetInt(string.Concat("InventoryItemUnlocked", itemID), 1);
     }

     return  PlayerPrefs.HasKey(string.Concat("InventoryItemUnlocked", itemID));
 }
 public void SetUnlockInventoryItem(int itemID)
 {
     PlayerPrefs.GetInt(string.Concat("InventoryItemUnlocked", itemID), 0);
     PlayerPrefs.SetInt(string.Concat("InventoryItemUnlocked", itemID), 1);
 }

 public int getUpgradedLevel(int itemID)
 {
    return  PlayerPrefs.GetInt("Item" +  itemID + "UpgradedLevel" , 0);
 }
 public void setUpgradedLevel(int itemID)
 {
     if(getUpgradedLevel(itemID)<InventoryController.instance.maxUpgrades)
       PlayerPrefs.SetInt("Item" +  itemID + "UpgradedLevel" , getUpgradedLevel(itemID) + 1 );
 }
 
 #endregion

 #region Reward

 public void setRewardPref(string rewardName , int value)
 {
     PlayerPrefs.SetInt(rewardName, value);
 }
 public bool getRewardPref(string rewardName)
 {
     if (PlayerPrefs.GetInt(rewardName) == 1)
     {
         return true;
     }
     else
     {
         return false;
     }
 }


 #endregion

 #region Generic

  public void setPref(string Name , int value)
  {
      PlayerPrefs.SetInt(Name, value);
  }
  public int getPref(string Name)
  {
     return PlayerPrefs.GetInt(Name);
  }
  public bool getPrefBool(string Name)
  {
      if (PlayerPrefs.GetInt(Name) == 1)
      {
          return true;
      }
      else
      {
          return false;
      }
  }

 #endregion
    #region Sound
    
    public float sfxVolume
    {
        get
        {
            if (_sfxVolume == -1f)
            {
                _sfxVolume = PlayerPrefs.GetFloat("sfx", 1f);
            }
            return _sfxVolume;
        }
        set
        {
            if (_sfxVolume != value)
            {
                _sfxVolume = value;
                PlayerPrefs.SetFloat("sfx", value);
                if (sfxVolumeChanged != null)
                {
                    sfxVolumeChanged(value);
                }
            }
        }
    }
    
    public float musicVolume
    {
        get
        {
            if (_musicVolume == -1f)
            {
                _musicVolume = PlayerPrefs.GetFloat("music", 1f);
            }
            return _musicVolume;
        }
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                PlayerPrefs.SetFloat("music", value);
                if (musicVolumeChanged != null)
                {
                    musicVolumeChanged(value);
                }
            }
        }
    }
    
    #endregion
    
    
    [Header("Coins")]
    private int _coins = -1;
    
    [Header("Gold")]
    private int _gold = -1;
    
    [Header("Sound")]
    private float _sfxVolume = -1f;
    
    [Header("Music")]
    private float _musicVolume = -1f;

    //        Delegates
    public delegate void CoinChangedDelegate();
    
    public delegate void GoldChangedDelegate();

    public delegate void sfxVolumeChangedDelegate(float newVolume);

    public delegate void musicVolumeChangedDelegate(float newVolume);
    
    public static event CoinChangedDelegate coinsChanged;
    
    public static event GoldChangedDelegate goldChanged;

    
    public static event sfxVolumeChangedDelegate sfxVolumeChanged;
	
    public static event musicVolumeChangedDelegate musicVolumeChanged;
}
