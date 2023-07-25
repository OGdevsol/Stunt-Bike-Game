using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Instance

    private static InventoryManager _instance;

    public static InventoryManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager>();
            }

            return _instance;
        }
    }

    #endregion

    public GameObject[] Player;

    public GameObject currentPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

        currentPlayer = Instantiate(Player[ApplicationController.SelectedInventoryItem]);
    }

    void Start()
    {
        GameController.instance._playerTransform = currentPlayer.transform.GetChild(0).GetComponent<Rigidbody>();
    }

    public void RespawnWait()
    {
        Invoke("Respawn",0.01f);
    }
    public void Respawn()
    {
        currentPlayer = Instantiate(Player[ApplicationController.SelectedInventoryItem]);
        currentPlayer.transform.SetParent(LevelController.instance.PlayerPos.parent);
        currentPlayer.transform.localPosition = GameController.instance.RespawnplayerPos.position;
        GameController.instance._playerTransform = currentPlayer.transform.GetChild(0).GetComponent<Rigidbody>();
       InGameController.instance._playerTransform =  GameController.instance._playerTransform;
    }
}