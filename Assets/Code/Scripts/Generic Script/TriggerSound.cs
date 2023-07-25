using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{

   
    private GameplaySoundController soundController;

    
    public void Start()
    {
        soundController= GameplaySoundController.instance;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Body"))
        {
            if(soundController)
                soundController.playFromPool(AudioLibrary.Checkpoint);
            
        }
    }
}
