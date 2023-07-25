using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    /*private static CoinScript _coinScript;
    public static CoinScript CoinScript
    {
        get
        {
            _coinScript = FindObjectOfType<CoinScript>();
            return _coinScript;
        }
    }*/

    #region Instance

    private static CoinScript _instance;

    public static CoinScript instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CoinScript>();
            }

            return _instance;
        }
    }

    #endregion


    private void Awake()
    {
        _instance = this;

    }

    
    public float rotationSpeed = 200f;
    private coinsound Coinsound;
   

  

        // Start is called before the first frame update
        void Start()
        {
            Coinsound=coinsound.instance;
            

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        }
        private void OnTriggerEnter(Collider Coin)
        {
            // Play the collision sound
            Coinsound.audioSource.PlayOneShot(Coinsound.collisionSound);
           
            // Destroy the coin object
            Destroy(gameObject);
        }
    }

