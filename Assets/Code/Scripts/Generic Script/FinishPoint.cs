using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
 #region Instance
 
    private static FinishPoint _instance;

    public static FinishPoint instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FinishPoint>();
            }

            return _instance;
        }
    }
 
    #endregion


    private void Awake()
    {
        _instance = this;
  
    }
    
    private void OnEnable()
    {
        GameController.gameEnded += Destroy;
    }
    private void OnDisable()
    {
        GameController.gameEnded -= Destroy;

    }
  
    
    
    Rigidbody targetRb;
    BikeController controller;
    bool shouldSlowDown = false;
    
    private GameplaySoundController soundController;

    
    public void Start()
    {
        soundController= GameplaySoundController.instance;

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Body"))
        {
            if (soundController)
            {
                soundController.playFromPool(AudioLibrary.CrowdCheering);
                soundController.gamePaused();
            }

            targetRb = other.attachedRigidbody;
            controller = other.gameObject.transform.GetComponentInParent<BikeController>();
            controller.DisableInput();
            shouldSlowDown = true;
        }

    }

    private void FixedUpdate()
    {
        if(shouldSlowDown)
        {
            if(targetRb.drag < 10f)
            {
                targetRb.drag += 1f;
            }
            else
            {
                win();
                controller.BrakeCheck(true);
                shouldSlowDown = false;
            }
        }
    }


    public void win()
    {
        GameController.instance.displayGameWin();
    }
    
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
