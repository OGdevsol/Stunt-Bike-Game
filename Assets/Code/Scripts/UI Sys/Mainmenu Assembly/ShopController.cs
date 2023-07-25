
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    #region Instance
 
    private static ShopController _instance;

    public static ShopController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ShopController>();
            }

            return _instance;
        }
    }
 
    #endregion


    public Scrollbar _Shopslider;
    private SharedMenuController _sharedMenuController;
    
    private void Awake()
    {
        _instance = this;
  
    }

    public void Start()
    {
        _sharedMenuController= SharedMenuController.instance;
    }

    public void AddCoin(int amount)
    {
        _sharedMenuController.AddCoin(amount);
    }

    public void setShopSlider(float value)
    {

        _Shopslider.value = value;
    }
}
