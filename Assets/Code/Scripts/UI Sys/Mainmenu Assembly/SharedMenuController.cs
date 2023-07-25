using TMPro;
using UnityEngine;

public class SharedMenuController : MonoBehaviour
{
    #region Instance

    private static SharedMenuController _instance;

    public static SharedMenuController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SharedMenuController>();
            }

            return _instance;
        }
    }

    #endregion

    public TextMeshProUGUI _CoinText;
    public Canvas containerGO;

    private DataController _dataController;
    private MainMenuController _mainMenuController;
    private RewardSystem _rewardSystem;

    public void OnEnable()
    {
        DataController.coinsChanged += showCoins;
    }

    public void OnDisable()
    {
        DataController.coinsChanged -= showCoins;
    }

    private void Awake()
    {
        _instance = this;
    }


    private void Start()
    {
        _dataController = DataController.instance;
        _mainMenuController = MainMenuController.instance;
        _rewardSystem = RewardSystem.instance;
        _CoinText.text = _dataController.coins.ToString();
    }

    public void ShopBtnClick()
    {
        if (containerGO.enabled)
            return;

        _mainMenuController.AddPanelToStackAndLoad(6);
    }


    public void AddCoin(int coins)
    {
        DataController.instance.AddCoins(coins);
        _CoinText.text = _dataController.coins.ToString();
    }


    public void showCoins()
    {
        _CoinText.text = _dataController.coins.ToString();
    }

    public void CoinRewarded()
    {
        _rewardSystem.ShowRewarded(ScriptLocalization.AddCoin);
    }
}