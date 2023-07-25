using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BikeSetup : EditorWindow
{
    public Object  Bike;
    public Object EngineSound;
    public Object Player;
    public Object RearWheelPhysicsMaterial;
    public Object ExhaustParticle;
    public Object DirtParticle;

    private GameObject BikeParent,BikeCOM,RearSuspension,FrontSuspension,RearWheel,FrontWheel;
    private BikeController bikeController;
    private AudioSource bikesource;
    private GameObject dp,ep;
    [MenuItem("Just Beginning/Bike Setup")]
    public static void ShowWindow()
    {
        GetWindow<BikeSetup>("Bike Setup");
    }

    void OnGUI()
    {
      
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Bike Mesh :");
        Bike = EditorGUILayout.ObjectField(Bike, typeof(GameObject), true);
        EditorGUILayout.LabelField("Engine Sound :");
        EngineSound = EditorGUILayout.ObjectField(EngineSound, typeof(AudioClip), true);
        EditorGUILayout.LabelField("Player Prefab :");
        Player = EditorGUILayout.ObjectField(Player, typeof(GameObject), true);
        EditorGUILayout.LabelField("Rear Wheel Physics Material :");
        RearWheelPhysicsMaterial = EditorGUILayout.ObjectField(RearWheelPhysicsMaterial, typeof(PhysicMaterial), true);
        EditorGUILayout.LabelField("Exhaust Particle :");
        ExhaustParticle = EditorGUILayout.ObjectField(ExhaustParticle, typeof(GameObject), true);
        EditorGUILayout.LabelField("Dirt Particle :");
        DirtParticle = EditorGUILayout.ObjectField(DirtParticle, typeof(GameObject), true);


        if (GUILayout.Button("Create"))
        {
            CreateBike();
        }

        EditorGUILayout.EndVertical();
        
    }

    public void CreateBike()
    {
    // Create Parent  
        BikeParent= new GameObject("Bike Prefab");
        
    // Instantiate Bike    
        GameObject BikePrefab =  Instantiate((GameObject)Bike,BikeParent.transform);
        
      
        
        //Bike Parenting  & Adding Components
        BikeParent.gameObject.tag = "Motorcycle";
        bikeController = BikeParent.AddComponent<BikeController>();
        bikeController.speed = 170;
        bikeController.flipValueGrounded = 225;
        bikeController.flipValue = 75;

        bikesource = BikeParent.AddComponent<AudioSource>();
        if (EngineSound)
        {
            bikesource.clip = (AudioClip)EngineSound;
        }
        bikeController._engine = bikesource;
        
        //Adding Components to Bike Prefab 
        BikePrefab.gameObject.tag = "Body";
        BikePrefab.AddComponent<BoxCollider>();
        Rigidbody rbbp = BikePrefab.AddComponent<Rigidbody>();
        rbbp.mass = 12;
        rbbp.angularDrag = 0.07f;
        rbbp.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
      //  rbbp.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

       
        BikePrefab.AddComponent<Ghost>();
        Ghost g = BikePrefab.GetComponent<Ghost>();
        g.Fwheel = BikePrefab.transform.Find("Front_Wheel");
        g.Bwheel = BikePrefab.transform.Find("Rear_Wheel");
        g.FrontBumper = BikePrefab.transform.Find("Front_Bumper");
        g.FrontSuspension = BikePrefab.transform.Find("Front_Suspention");
        g.BackSuspension = BikePrefab.transform.Find("Rear_Suspention");

        //Adjust Body Parts
        BikePrefab.transform.Find("Front_Wheel").SetParent(BikePrefab.transform.GetChild(0));
        BikePrefab.transform.GetChild(0).Find("Front_Wheel").gameObject.AddComponent<WheelRotation>();
         Transform fwheel = BikePrefab.transform.GetChild(0).Find("Front_Wheel");
        BikePrefab.transform.Find("Rear_Wheel").SetParent(BikePrefab.transform.GetChild(0));
        WheelRotation rw = BikePrefab.transform.GetChild(0).Find("Rear_Wheel").gameObject.AddComponent<WheelRotation>();
        rw.wheelType = WheelRotation.WheelType.Rear;
        Transform rwheel = BikePrefab.transform.GetChild(0).Find("Rear_Wheel");
        BikePrefab.transform.Find("Front_Bumper").SetParent(BikePrefab.transform.Find("Front_Suspention"));

        //Creating ik Points
        GameObject rht = new GameObject("RightHandTarget");
        GameObject rft = new GameObject("RightFootTarget");
        GameObject lht = new GameObject("LeftHandTarget");
        GameObject lft = new GameObject("LeftFootTarget");

        rht.transform.SetParent(BikePrefab.transform.GetChild(0));
        rft.transform.SetParent(BikePrefab.transform.GetChild(0));
        lht.transform.SetParent(BikePrefab.transform.GetChild(0));
        lft.transform.SetParent(BikePrefab.transform.GetChild(0));

        rht.transform.position= Vector3.zero;
        rht.transform.rotation= Quaternion.identity;
        
        rft.transform.position= Vector3.zero;
        rft.transform.rotation= Quaternion.identity;
        
        lht.transform.position= Vector3.zero;
        lht.transform.rotation= Quaternion.identity;
        
        lft.transform.position= Vector3.zero;
        lft.transform.rotation= Quaternion.identity;
        
        
        //Creating COM
        BikeCOM = new GameObject("COM");
        BikeCOM.transform.SetParent(BikePrefab.transform);
        BikeCOM.transform.position= Vector3.zero;
        BikeCOM.transform.rotation= Quaternion.identity;

        //Instantiate Player Character
        GameObject PlayerPrefab = Instantiate((GameObject)Player, Vector3.zero, Quaternion.identity,BikePrefab.transform);

       Crash c= PlayerPrefab.transform.GetChild(1).GetComponent<Crash>();
       c._rHandEffector = rht.transform;
       c._rlegEffector = rft.transform;
       c._lHandEffector = lht.transform;
       c._llegEffector = lft.transform;

       //Creating Rear Suspension 
        RearSuspension = new GameObject("RearSuspension");
        RearSuspension.gameObject.tag = "RearSuspension";
        RearSuspension.transform.SetParent(BikeParent.transform);
        RearSuspension.transform.position= Vector3.zero;
        RearSuspension.transform.rotation= Quaternion.identity;
        BikePrefab.transform.Find("Rear_Suspention").SetParent(RearSuspension.transform);

        //Adding Rigidbody to Rear Suspension
        Rigidbody rbrs = RearSuspension.AddComponent<Rigidbody>();
        rbrs.mass = 2f;
        rbrs.angularDrag = 2f;
        rbrs.constraints = RigidbodyConstraints.FreezePositionZ ;

        //Adding Hinje Joint to Rear Suspension
        HingeJoint hjrs = RearSuspension.AddComponent<HingeJoint>();
        hjrs.connectedBody = rbbp;
        hjrs.axis = new Vector3(0,0,-1);
        hjrs.useSpring = true;
        
        //Adding Joint Spring Value to Rear Suspension
        var hjrsjs = hjrs.spring;
        hjrsjs.spring = 900;
        hjrsjs.damper = 10;
        hjrsjs.targetPosition = 0.5f;
        hjrs.spring = hjrsjs;
        
        //Adding Limit Value to Rear Suspension
        hjrs.useLimits = true;
        var hjrsjl = hjrs.limits;
        hjrsjl.min = -20;
        hjrsjl.max = 20;
        hjrs.limits = hjrsjl;
        
        //Creating Front Suspension 
        FrontSuspension = new GameObject("FrontSuspension");
        FrontSuspension.gameObject.tag = "FrontSuspension";
        FrontSuspension.transform.SetParent(BikeParent.transform);
        FrontSuspension.transform.position= Vector3.zero;
        FrontSuspension.transform.rotation= Quaternion.identity;
        BikePrefab.transform.Find("Front_Suspention").SetParent(FrontSuspension.transform);

        //Adding Rigidbody to Front Suspension
        Rigidbody rbfs = FrontSuspension.AddComponent<Rigidbody>();
        rbfs.mass = 2f;
        rbfs.angularDrag = 2f;
        rbfs.constraints = RigidbodyConstraints.FreezePositionZ ;
      
        //Adding Configurable Joint to Front Suspension
        ConfigurableJoint rbfscj = FrontSuspension.AddComponent<ConfigurableJoint>();
        rbfscj.connectedBody = rbbp;
        rbfscj.xMotion = ConfigurableJointMotion.Locked;
        rbfscj.yMotion = ConfigurableJointMotion.Limited;
        rbfscj.zMotion = ConfigurableJointMotion.Locked;
        rbfscj.angularXMotion = ConfigurableJointMotion.Locked;
        rbfscj.angularYMotion = ConfigurableJointMotion.Locked;
        rbfscj.angularZMotion = ConfigurableJointMotion.Locked;

        //Adding Linear Limit Spring Value to Front Suspension
        var rbfscjlls = rbfscj.linearLimitSpring;
        rbfscjlls.spring = 1200;
        rbfscjlls.damper = 27;
        rbfscj.linearLimitSpring = rbfscjlls;
        
        //Adding Linear Limit Value to Front Suspension
        var rbfscjll = rbfscj.linearLimit;
        rbfscjll.limit = 0.01f;
        rbfscj.linearLimit = rbfscjll;
        
        //Creating Rear Wheel 
        RearWheel = new GameObject("RearWheel");
        RearWheel.gameObject.tag = "RearWheel";
        RearWheel.transform.SetParent(BikeParent.transform);
        RearWheel.transform.position= Vector3.zero;
        RearWheel.transform.rotation= Quaternion.identity;
        
        //Adding Rigidbody to Rear Wheel
        Rigidbody rbrw = RearWheel.AddComponent<Rigidbody>();
        rbrw.mass = 2f;
        rbrw.angularDrag = 3.6f;
        rbrw.constraints = RigidbodyConstraints.FreezePositionZ;
        
        //Adding Sphere Collider to Rear Wheel
        SphereCollider sprw = RearWheel.AddComponent<SphereCollider>();
        sprw.radius = 0.43f;  //0.34
        sprw.material = (PhysicMaterial)RearWheelPhysicsMaterial; 
        
        
        //Adding Configurable Joint to Rear Wheel
        ConfigurableJoint rwcj = RearWheel.AddComponent<ConfigurableJoint>();
        rwcj.connectedBody = rbrs;
        rwcj.secondaryAxis=new Vector3(0,-1,0);
        rwcj.xMotion = ConfigurableJointMotion.Locked;
        rwcj.yMotion = ConfigurableJointMotion.Locked;
        rwcj.zMotion = ConfigurableJointMotion.Locked;
        rwcj.angularXMotion = ConfigurableJointMotion.Free;
        rwcj.angularYMotion = ConfigurableJointMotion.Locked;
        rwcj.angularZMotion = ConfigurableJointMotion.Locked;
        
        //Adding CheckIsGrounded Script to Rear Wheel
        RearWheel.AddComponent<CheckIsGrounded>();
        
        //Creating Front Wheel 
        FrontWheel = new GameObject("FrontWheel");
        FrontWheel.gameObject.tag = "FrontWheel";
        FrontWheel.transform.SetParent(BikeParent.transform);
        FrontWheel.transform.position= Vector3.zero;
        FrontWheel.transform.rotation= Quaternion.identity;
        
        //Adding Rigidbody to Front Wheel
        Rigidbody rbfw = FrontWheel.AddComponent<Rigidbody>();
        rbfw.mass = 2f;
        rbfw.angularDrag = 3.6f;
        rbfw.constraints = RigidbodyConstraints.FreezePositionZ;
        
        //Adding Sphere Collider to Front Wheel
        SphereCollider spfw = FrontWheel.AddComponent<SphereCollider>();
        spfw.radius = 0.43f;//0.34
            
        //Adding Configurable Joint to Front Wheel
        ConfigurableJoint fwcj = FrontWheel.AddComponent<ConfigurableJoint>();
        fwcj.connectedBody = rbfs;
        fwcj.secondaryAxis=new Vector3(0,-1,0);
        fwcj.xMotion = ConfigurableJointMotion.Locked;
        fwcj.yMotion = ConfigurableJointMotion.Locked;
        fwcj.zMotion = ConfigurableJointMotion.Locked;
        fwcj.angularXMotion = ConfigurableJointMotion.Free;
        fwcj.angularYMotion = ConfigurableJointMotion.Locked;
        fwcj.angularZMotion = ConfigurableJointMotion.Locked;
        fwcj.projectionDistance = 0.1f;

        //Adding CheckIsGrounded Script to Front Wheel
        FrontWheel.AddComponent<CheckIsGrounded>();
        
        //Create Dirt & Exhaust Particle
        if (DirtParticle != null)
             dp = Instantiate((GameObject)DirtParticle, Vector3.zero, Quaternion.identity,BikePrefab.transform);
        
        if (ExhaustParticle != null)
             ep = Instantiate((GameObject)ExhaustParticle, Vector3.zero, Quaternion.identity,BikePrefab.transform);
        
    
        
        //Adding Reference to Bike Controller
        bikeController.COM = BikeCOM.transform;
        bikeController.body = rbbp;
        bikeController.frontFork = rbfs;
        bikeController.rearFork = rbrs;
        bikeController.frontWheel = rbfw;
        bikeController.rearWheel = rbrw;
        bikeController.characterAnimator = PlayerPrefab.GetComponent<Animator>();
        
        if (dp)
            bikeController.dirt = dp.GetComponent<ParticleSystem>();
        if(ep)
            bikeController.currentExhaust = ep.GetComponent<ParticleSystem>();
        
        

    }


}
