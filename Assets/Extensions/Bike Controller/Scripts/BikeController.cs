using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class BikeController : MonoBehaviour
{

    public Transform COM;
    
    public Rigidbody body;
    public Rigidbody frontFork;
    public Rigidbody rearFork;
    public Rigidbody frontWheel;
    public Rigidbody rearWheel;

    [HideInInspector]
    public bool inAir,Isdead;
    public static bool crash;
    public static bool crashed;
    public static bool IsRespawn;
    
    public float speed = 48f;
    public int flipValueGrounded = 170;
    public int flipValue = 75;

    public Animator characterAnimator;
    
    public AudioSource _engine;
    
    public ParticleSystem currentExhaust;
    public ParticleSystem dirt;
  
   

    private CheckIsGrounded isGrounded_rearWheel;
    private CheckIsGrounded isGrounded_frontWheel;
    
    private bool jump,onGround,flip,accelerate,brake,left,right;
    private bool useFrontWheel = false;
    private bool canJump = false;
   
    private int backfC;
    private int BackFlipCount = 0;
    private int FrontFlipCount = 0;
    
    private float airTime;
    private float maxVel = 35f;
    private float maxAirTIme;
    private float pitch;
    private float cooldown = 0.7f;

    private int CurrentInventoryItem;
     Ghost _Ghost;
     private DataController DataController;
     private GameplaySoundController soundController;
    public static BikeController Instance;
     
    private void OnEnable()
    {
        InGameController._Brake += BrakeCheck;
        InGameController._Race += AcceleraterCheck;
        InGameController._BackFlip += BackFlip;
        InGameController._FrontFlip += FrontFlip;
        InGameController._Jump += JumpCheck;
        GameController.gameEnded += StopRecordingGhost;
    }

    private void OnDisable()
    {
        InGameController._Brake -= BrakeCheck;
        InGameController._Race -= AcceleraterCheck;
        InGameController._BackFlip -= BackFlip;
        InGameController._FrontFlip -= FrontFlip;
        InGameController._Jump -= JumpCheck;
        GameController.gameEnded -= StopRecordingGhost;

    }
    
    
    public void Awake()
    {
        Time.timeScale = 1;
        Instance = this;
    }

    private void Start()
    {
       DataController=DataController.instance;
       soundController= GameplaySoundController.instance;
       CurrentInventoryItem = ApplicationController.SelectedInventoryItem;
     
        IntializeStats();
        Initialization();
        setExhaust();
        setLayers();
        setSpeed();
        startBooleans();

        if (GameController.gameMode == Mode.CHALLENGE)
        {
            setupGhost();
            PlayRecordingGhost();
            StartRecordingGhost();
        }
    }
  

    private void SetMotion()
    {
        frontFork.isKinematic = false;
        rearFork.isKinematic =false;
        body.isKinematic = false;
    }
    public Transform RespawnplayerPos;
    public void Respawn()
    {
       
        body.isKinematic = true;
        frontFork.isKinematic = true;
        rearFork.isKinematic = true;
        frontWheel.isKinematic = true;
        rearWheel.isKinematic = true;
        if (body.isKinematic)
        {
            var position = RespawnplayerPos.position;
            NitroEffect.SetActive(false);
            InGameController.isNitroActive = false;
           // speed = 250;
            frontWheel.MovePosition(position);
            frontFork.MovePosition(position);
            frontFork.MoveRotation(Quaternion.Euler(0f,90,0));
            rearFork.MoveRotation(Quaternion.Euler(0f,90,0));
            rearWheel.MovePosition(position);
            rearFork.MovePosition(position);    
            body.MovePosition( position);
            body.MoveRotation(Quaternion.Euler(0f,90,0));
          
        }
        
        frontWheel.isKinematic =false;
        rearWheel.isKinematic = false;
        Invoke(nameof(SetMotion), 2f);
      
    }
    
    public void IntializeStats()
    {
        speed = calculateUpgradeValue(DataController.getPref(String.Concat( ScriptLocalization.BikeSpeed,CurrentInventoryItem)) , DataController.getUpgradedLevel( CurrentInventoryItem));
        flipValueGrounded = calculateUpgradeValue(DataController.getPref(String.Concat( ScriptLocalization.BikeHandling,CurrentInventoryItem)) , DataController.getUpgradedLevel( CurrentInventoryItem));
        flipValue = calculateUpgradeValue(DataController.getPref(String.Concat( ScriptLocalization.BikeGrip,CurrentInventoryItem)), DataController.getUpgradedLevel( CurrentInventoryItem));
    }
    public int calculateUpgradeValue(int upgradetype , int currentUpgradedLevel )
    {
        return upgradetype + (DataController.getPref( ScriptLocalization.UpgradeMulitplier) * currentUpgradedLevel);
    }
    public void Initialization()
    {
        isGrounded_rearWheel = GameObject.FindGameObjectWithTag("RearWheel").GetComponent<CheckIsGrounded>();
        isGrounded_frontWheel = GameObject.FindGameObjectWithTag("FrontWheel").GetComponent<CheckIsGrounded>();
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Rigidbody>();
        frontFork = GameObject.FindGameObjectWithTag("FrontSuspension").GetComponent<Rigidbody>();
        rearFork = GameObject.FindGameObjectWithTag("RearSuspension").gameObject.GetComponent<Rigidbody>();
        rearWheel = GameObject.FindGameObjectWithTag("RearWheel").GetComponent<Rigidbody>();
        frontWheel = GameObject.FindGameObjectWithTag("FrontWheel").GetComponent<Rigidbody>();
        
      
    }

    public void setExhaust()
    {
        if (currentExhaust)
        {
            currentExhaust.gameObject.SetActive(true);
            var emit = currentExhaust.emission;
            emit.enabled = false;
        }
    }

    public void setLayers()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer("Ragdoll"), true);
        Physics.IgnoreCollision(frontWheel.GetComponent<Collider>(), body.GetComponent<Collider>());
        Physics.IgnoreCollision(rearWheel.GetComponent<Collider>(), body.GetComponent<Collider>());
    }

    public void startBooleans()
    {
        Input.multiTouchEnabled = true;
        crash = false;
        crashed = false;
        IsRespawn = false; 
    }

    public void setSpeed()
    {
        rearWheel.GetComponent<Rigidbody>().maxAngularVelocity = speed;
        frontWheel.GetComponent<Rigidbody>().maxAngularVelocity = speed;
        if(soundController)
        soundController.playFromPool(AudioLibrary.BikeStart);
    }

    public GameObject NitroEffect;
    public void setupGhost()
    {
        _Ghost = transform.GetChild(0).GetComponent<Ghost>();
    }

    public void StartRecordingGhost()
    {
       
        _Ghost.StartRecordingGhost();
    }
    public void StopRecordingGhost()
    {
        if (GameController.gameMode == Mode.CHALLENGE)
        {
           // if(_Ghost)
            _Ghost.StopRecordingGhost();
        }
    }
    public void PlayRecordingGhost()
    {
        _Ghost.playGhostRecording(transform.parent);
    }
    private void Update()
    {
        if (!Isdead)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                BackFlip(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AcceleraterCheck(true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                FrontFlip(true);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                JumpCheck(true);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                BackFlip(false);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                AcceleraterCheck(false);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                FrontFlip(false);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                JumpCheck(false);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                brake = true;
                AcceleraterCheck(false);
            }

            CharacterAnimatorState();

            //if (accelerate)
            //{
            //    pitch = rearWheel.angularVelocity.sqrMagnitude / speed;
            //    pitch *= Time.deltaTime * 2f;
            //    pitch = Mathf.Clamp(pitch + 1f, 0.5f, 1.8f);
            //    _engine.pitch = pitch;
            //}
            //else
            //{
            //    pitch = Mathf.Clamp(pitch - Time.deltaTime * 2f, 0.5f, 1.8f);
            //    _engine.pitch = pitch;
            //}

            if (cooldown > 0 && !canJump)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                canJump = true;
                cooldown = 0.7f;
            }
        }
    }



    private void FixedUpdate()
    {

        if (!Isdead)
        {
            if (jump && !inAir && canJump)
            {
                canJump = false;
                body.AddForceAtPosition(new Vector3(0f, 3060f * Time.deltaTime, 0f), COM.position, ForceMode.Impulse);
            }

            if (accelerate && onGround)
            {
                if (dirt)
                    dirt.gameObject.SetActive(true);
                rearWheel.freezeRotation = false;

                if (useFrontWheel)
                {


                    frontWheel.AddTorque(new Vector3(0f, 0f, (0f - speed) * Time.deltaTime), ForceMode.Impulse);


                    if (dirt)
                        dirt.gameObject.SetActive(false);
                }
                else
                {

                    rearWheel.AddTorque(new Vector3(0f, 0f, (0f - speed) * Time.deltaTime), ForceMode.Impulse);

                }

                if (!left && !right && !useFrontWheel)
                {
                    frontWheel.AddForceAtPosition(new Vector3(0f, 0.5f * Time.deltaTime, 0f), frontWheel.transform.up,
                        ForceMode.Impulse);
                }
            }
            else
            {
                if (dirt)
                    dirt.gameObject.SetActive(false);
            }

            if (brake)
            {
                if (rearWheel.GetComponent<CheckIsGrounded>().IsGrounded &&
                    frontWheel.GetComponent<CheckIsGrounded>().IsGrounded)
                {
                    rearWheel.freezeRotation = true;
                    frontWheel.freezeRotation = true;
                }

            }
            else
            {
                rearWheel.freezeRotation = false;
                frontWheel.freezeRotation = false;
            }

            if (left)
            {
                if (!inAir)
                {
                    float leftValue = flipValueGrounded;

                    body.AddTorque(new Vector3(0f, 0f, 1f) * leftValue);


                }
                else
                {
                    float leftValueinAir = flipValue;

                    body.AddTorque(new Vector3(0f, 0f, 1f) * leftValueinAir);


                }
            }
            else if (right)
            {
                if (!inAir)
                {
                    float rightValue = -flipValueGrounded;

                    body.AddTorque(new Vector3(0f, 0f, 1f) * rightValue);

                }
                else
                {
                    float rightValueInAir = -1 * flipValue;

                    body.AddTorque(new Vector3(0f, 0f, 1f) * rightValueInAir);
                   

                }
            }

            if (!isGrounded_rearWheel.IsGrounded && isGrounded_frontWheel.IsGrounded)
            {
                useFrontWheel = true;
            }
            else
            {
                useFrontWheel = false;
            }

            if (isGrounded_rearWheel.IsGrounded || isGrounded_frontWheel.IsGrounded)
            {
                onGround = true;
                inAir = false;
            }
            else
            {
                onGround = false;
                inAir = true;

            }
        }
    }

    public void AcceleraterCheck(bool ac)
    {
        if (currentExhaust)
        {
            var emit = currentExhaust.emission;
            emit.enabled = ac;
        }

        accelerate = ac;
    }

    public void BrakeCheck(bool bc)
    {
        brake = bc;
        if (currentExhaust)
        {
            var emit = currentExhaust.emission;
            emit.enabled = false;
        }
    }

    public void BackFlip(bool bf)
    {
        left = bf;
    }

    public void FrontFlip(bool ff)
    {
        right = ff;
    }

    public void JumpCheck(bool jc)
    {
        jump = jc;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Rigidbody FindBodyReference()
    {
        return body;
    }

    public void DisableInput()
    {
        Isdead = true;
        accelerate = false;
        brake = false;
        left = false;
        right = false;
        jump = false;
        _engine.Stop();
        if(soundController)
            soundController.playFromPool(AudioLibrary.BikeEnd);
        
    }

    private void CharacterAnimatorState()
    {
        if (characterAnimator.gameObject.activeSelf && !((Object)characterAnimator.gameObject == (Object)null))
        {
            if (right)
            {
                characterAnimator.SetBool("LeanForward", true);
            }
            else
            {
                characterAnimator.SetBool("LeanForward", false);
            }


            if (left)
            {
                characterAnimator.SetBool("LeanBackward", true);
            }
            else
            {
                characterAnimator.SetBool("LeanBackward", false);
            }
            

            if (!right && !left)
            {
                characterAnimator.SetBool("Idle", true);
            }
            else
            {
                characterAnimator.SetBool("Idle", false);
            }

        }
    }

    

}
