using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crash : MonoBehaviour
{
    #region Instance
 
    private static Crash _instance;

    public static Crash instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Crash>();
            }

            return _instance;
        }
    }
 
    #endregion
    
    [SerializeField] private BikeController controller;
    [SerializeField] private GameObject _ragDoll;
    [SerializeField] private Animator _charAnimator;
    [SerializeField] private List<CapsuleCollider> _colliders;
    [SerializeField] private List<Rigidbody> _rbs;
    [SerializeField] public ArmIK _rHandIK,_lHandIK;
    [SerializeField] public LegIK _rlegIK,_llegIK;
    [SerializeField] public Transform _rHandEffector,_lHandEffector;
    [SerializeField] public Transform _rlegEffector,_llegEffector;
    public bool isDead = false;
    private GameplaySoundController soundController;
    private void OnEnable()
    {
        controller=transform.root.GetComponent<BikeController>();
        ToggleRagdoll(false);
        _rHandIK.solver.arm.target=_rHandEffector; 
        _lHandIK.solver.arm.target=_lHandEffector; 
        _rlegIK.solver.leg.target=_rlegEffector; 
        _llegIK.solver.leg.target=_llegEffector; 

    }

    public void Start()
    {
        soundController= GameplaySoundController.instance;

    }
  
    private bool isCooldownActive = false;
    private float cooldownDuration = 3f;

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        isCooldownActive = false;
    }

    void CallRespawn()
    {
        //ToggleRagdoll(false);
        BikeController.Instance.Respawn();
       InGameController.instance. playerlives -= 1;
        isCooldownActive = true;
        StartCoroutine(StartCooldown());
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Water"))
        {
            print("lives condition checking");
            if (!isCooldownActive &&  InGameController.instance. playerlives > 0)
            {
                print("lives condition checked");
                ToggleRagdoll(false);
                //BikeController.Instance.Respawn();
                InGameController.instance.playerlives -= 1;
                InventoryManager.instance.RespawnWait();
                Destroy(InventoryManager.instance.currentPlayer);
              
                isCooldownActive = true;
                StartCoroutine(StartCooldown());
                
            }
            else if (!isCooldownActive)
            {
                if (soundController)
                {
                    soundController.playFromPool(AudioLibrary.PlayerHurt);
                    soundController.playFromPool(AudioLibrary.CrowdOh);
                    soundController.gamePaused();
                }
                
                isDead = true;
                controller.DisableInput();
                ToggleRagdoll(true);
                Invoke(nameof(GameOver), 2f);
            }
            
        }
    }

    private void GameOver()
    {
       GameController.instance.displayGameOver();
    }

    void ToggleRagdoll(bool ragdollOn)
    {
        _ragDoll.SetActive(ragdollOn);
        _charAnimator.enabled = !ragdollOn;
        
        _rlegIK.enabled=  _llegIK.enabled=  _lHandIK.enabled= _rHandIK.enabled = !ragdollOn;
        
        foreach(var collider in _colliders)
        {
            collider.enabled = ragdollOn;
        }

        foreach(var rb in _rbs)
        {
            rb.isKinematic = !ragdollOn;
        }

    }
    
}
