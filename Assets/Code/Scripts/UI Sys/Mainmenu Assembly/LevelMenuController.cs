
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuController : MonoBehaviour
{
    #region Instance

    private static LevelMenuController _instance;

    public static LevelMenuController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelMenuController>();
            }

            return _instance;
        }
    }

    #endregion

    public Button[] levelButtons;
    public GameObject[] lockButtons;
    public Sprite lockBorder;
    public Sprite unLockBorder;
    public GameObject[] TextImages;
    public Scrollbar _levelSlider;
    
    
    public GameObject AllLevels;
    public TextMeshProUGUI AllLevelUnlockRemaingVideos;
    public int AllLevelUnlockMaxVideos;
    public int customMode,customLevelUnlock;
    int i,lastSelectedLevel;
    int levelButtonsLength;
    
    private DataController _dataController;
    private InventoryController _inventoryController;
    private RewardSystem _rewardSystem;

    private void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        _inventoryController = InventoryController.instance;
        _dataController = DataController.instance;
        _rewardSystem=RewardSystem.instance;    
        
        if (DataController.instance.getPrefBool(ScriptLocalization.AllLevels))
        {
            AllLevels.SetActive(false);
        }
        else
        {
            AllLevelUnlockRemaingVideos.text = DataController.instance.getPref(ScriptLocalization.AllLevelsRemainingVideo)+"/"+AllLevelUnlockMaxVideos;
        }
    }

    public void ExecuteLevel()
    {
        UnlockingLevel();
    }

    public void OnClickSelectLevel(int index)
    {
        ApplicationController.SelectedLevel = index;
        _inventoryController.Display();

    }

    public void Play()
    {
        OnClickSelectLevel(ApplicationController.SelectedLevel);
        
    }

    public void UnlockingLevel()
    {
        levelButtonsLength = levelButtons.Length;

        if (levelButtons.Length <= 0) return;

        for (i = 0; i < levelButtonsLength; i++)
        {
            if (_dataController.GetUnlockLevel(i))
            {
                levelButtons[i].interactable = true;
                lockButtons[i].SetActive(false);
                levelButtons[i].GetComponent<Image>().sprite = unLockBorder;

               TextImages[i].SetActive(true);
                lastSelectedLevel = i;
            }
            else
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().sprite = lockBorder;
                lockButtons[i].SetActive(true);
                TextImages[i].SetActive(false);

            }
        }
        
       

        ApplicationController.LastSelectedLevel = lastSelectedLevel;
        ApplicationController.SelectedLevel = ApplicationController.LastSelectedLevel;
      
        levelButtons[ApplicationController.LastSelectedLevel].GetComponent<Selectable>().Select();
        //setSlider();
    }

    public void UnlockAllLevel()
    {
        _rewardSystem.ShowRewarded(ScriptLocalization.UnlockAllLevel);
    }

    public void UnlockAllLevelReward()
    {
        if (DataController.instance.getPref(ScriptLocalization.AllLevelsRemainingVideo) >= AllLevelUnlockMaxVideos-1)
        {
            ApplicationController.SelectedGameMode = 0;
            for (i = 0; i < levelButtons.Length; i++)
            {
                DataController.instance.SetUnlockLevel(i);
            }

            ApplicationController.SelectedGameMode = 1;
            for (i = 0; i < levelButtons.Length; i++)
            {
                DataController.instance.SetUnlockLevel(i);
            }

            ApplicationController.SelectedGameMode = 2;
            for (i = 0; i < levelButtons.Length; i++)
            {
                DataController.instance.SetUnlockLevel(i);
            }

            ExecuteLevel();

            DataController.instance.setRewardPref(ScriptLocalization.AllLevels, 1);
            AllLevels.SetActive(false);

        }
        else
        {
            DataController.instance.setPref(ScriptLocalization.AllLevelsRemainingVideo,DataController.instance.getPref(ScriptLocalization.AllLevelsRemainingVideo)+1);
            AllLevelUnlockRemaingVideos.text = DataController.instance.getPref(ScriptLocalization.AllLevelsRemainingVideo)+"/"+AllLevelUnlockMaxVideos;

        }
    }

    public float tempSlidervalue;
    public void setSlider()
    {
    
        tempSlidervalue=(lastSelectedLevel) / 10f;
        _levelSlider.value = tempSlidervalue;
    }



    [ContextMenu("Unlock Level")]
    public void CustomLevelUnlocking()
    {
        ApplicationController.SelectedGameMode = customMode;

        for (int j = 0; j < customLevelUnlock; j++)
        {
            DataController.instance.SetUnlockLevel(j);
        }
    }
}
