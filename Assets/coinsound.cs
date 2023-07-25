using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinsound : MonoBehaviour
{
    #region Instance

    private static coinsound _instance;

    public static coinsound instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<coinsound>();
            }

            return _instance;
        }
    }

    #endregion


    private void Awake()
    {
        _instance = this;

    }
    public AudioClip collisionSound; // Reference to the collision sound clip
    public AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the coin object
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
