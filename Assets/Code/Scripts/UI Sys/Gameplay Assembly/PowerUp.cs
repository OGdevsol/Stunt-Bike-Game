using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region Instance
 
    private static PowerUp _instance;

    public static PowerUp instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PowerUp>();
            }

            return _instance;
        }
    }
 
    #endregion
    //public Transform playerporesp;
    bool isCollisionDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isCollisionDetected && other.gameObject.CompareTag("Body"))
        {
            //BikeController.Instance.speed += 100;
            //print("here");
            GameController.instance.RespawnplayerPos = gameObject.transform;
            print("Collision Detected");
            isCollisionDetected = true;
            InGameController.nitroTimeLeft = 5f;
        }
    }

}