
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour

{
    #region Instance
 
    private static ApplicationController _instance;

    public static ApplicationController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ApplicationController>();
            }

            return _instance;
        }
    }
 
    #endregion
    
    private void Awake()
    {
        _instance = this;
  
    }

    public static int SelectedLevel;

    public static int SelectedGameMode
    {
        
        get { return DataController.lastSelectedGameMode; }
        set { DataController.lastSelectedGameMode = value ; }
    }
    
    public static int LastSelectedLevel
    {
        
        get { return DataController.lastSelectedLevel; }
        set { DataController.lastSelectedLevel = value; }
    }
    
    public static int SelectedInventoryItem
    {
        
        get { return DataController.lastSelectedInventoryItem; }
        set { DataController.lastSelectedInventoryItem = value ; }
    }
}
 