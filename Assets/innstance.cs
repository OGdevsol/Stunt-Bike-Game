using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class innstance : MonoBehaviour
{
    #region Instance
 
    private static innstance _instance;

    public static innstance instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<innstance>();
            }

            return _instance;
        }
    }
 
    #endregion
    // Start is called before the first frame update
    // void Start()
    // {
    //   
    // }

    // Assuming you have a method that handles collisions, you can modify it like this:

    // public void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Body"))
    //     {
    //         GameController.RespawnplayerPos = gameObject.transform;
    //         print("Collision Detected");
    //         
    //     }
    //     // Assign the new GameObject to RespawnplayerPos
    // }


    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
