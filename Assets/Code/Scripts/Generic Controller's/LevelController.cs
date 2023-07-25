using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelController : MonoBehaviour
{
 #region Instance
 
    private static LevelController _instance;

    public static LevelController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelController>();
            }

            return _instance;
        }
    }
 
    #endregion


    public Transform PlayerPos;
    public Transform Finishpoint;
    public Transform SpaceShip;

    private InventoryManager inventoryManager;
    private void Awake()
    {
        _instance = this;
  
    }
   
    // Start is called before the first frame update
    void Start()
    
    {
        inventoryManager = InventoryManager.instance;
        inventoryManager.currentPlayer.transform.SetParent(PlayerPos.parent);
        inventoryManager.currentPlayer.transform.localPosition =  PlayerPos.localPosition;
        GameController.instance.currentLevel = this;  
    }





}
