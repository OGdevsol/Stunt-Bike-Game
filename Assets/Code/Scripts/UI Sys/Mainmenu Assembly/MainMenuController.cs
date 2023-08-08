using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    
    #region Instance
 
    private static MainMenuController _instance;

    public static MainMenuController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainMenuController>();
            }

            return _instance;
        }
    }
 
    #endregion

    public Canvas[] panels;
    public Stack<Canvas> panelsStack = new Stack<Canvas>();
    private int currentpanelIndex=-1;


     InventoryController _InventoryController;
     private MenuController _menuController;
     private ExitController _exitController;
     private SettingsController _settingsController;
     private ModeSelectController _modeSelectController;

    private void Awake()
    {
  
        _instance = this;
  
    }
    
    void Start()
    {
        _InventoryController = InventoryController.instance;
        _menuController= MenuController.instance;
        _exitController= ExitController.instance;
        _settingsController= SettingsController.instance;
        _modeSelectController= ModeSelectController.instance;

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].enabled = false;
        }
        
        AddPanelToStackAndLoad(0);
    }
    
    public void AddPanelToStackAndLoad(int panelIndex)
    {
        
        if (panelsStack.Contains(panels[0]))
        {
            panelsStack.Peek().enabled = false;
        }
        panelsStack.Push(panels[panelIndex]);
        panelsStack.Peek().enabled = true;
        currentpanelIndex = panelIndex;
        
        if (panelIndex==0)
        {
            _menuController.PlayAnimation();
        }
        DisplayAd();

    }
    
    
    public void RemoveLastPanelFormStack()
    {
            if (currentpanelIndex == 3)
            {
              _InventoryController.cancelInventoryItem();  
            }
            
            panelsStack.Peek().enabled = false;
            panelsStack?.Pop(); 
            panelsStack.Peek().enabled = true;
            
            if (panelsStack.Peek() == panels[0])
            {
                _menuController.PlayAnimation();
            }

            DisplayAd();
    }


    public void DisplayAd()
    {
        if (panelsStack.Peek() == panels[2])
        {
            _modeSelectController.ShowAds();
        }
        if (panelsStack.Peek() == panels[4])
        {
            _settingsController.ShowAds();
        }
        if (panelsStack.Peek() == panels[5])
        {
            _exitController.ShowAds();
        }
        
        if (panelsStack.Peek() == panels[0])
        {
            _settingsController.HideAds();
        }
    }
}
