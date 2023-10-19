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
              
                StartCoroutine("win");
                controller.BrakeCheck(true);
                shouldSlowDown = false;
            }
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSecondsRealtime(3f);
        win();
    }

    private Vector3 offset = new Vector3(0, -250, 0);
    public IEnumerator win()
    {
        Instantiate(GameController.instance.winParticle,GameController.instance. _playerTransform.position,GameController.instance.gameObject.transform.rotation);
        yield return new WaitForSecondsRealtime(4f);
        GameController.instance.displayGameWin();
    }
    
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
