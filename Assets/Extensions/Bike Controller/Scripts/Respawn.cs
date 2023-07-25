using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            if (other.GetComponentInChildren<Crash>().isDead) return;
//            GameController.instance.displayGameOver();
        }
    }
}
