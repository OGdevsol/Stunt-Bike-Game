using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static float maxcamfieldOfView = 50f;
    private Transform target;
    public float height = 9f;
    public float heightDamping;
    public float minFOV, maxFOV;

    public Crash _crash;
    private Rigidbody rigid;
    private float LerpHeight;
    private float CurrentHeight;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        target = GameObject.FindWithTag("Body").transform;
        rigid = target.gameObject.GetComponent<Rigidbody>();
        _crash = GameObject.Find("DeadZone").GetComponent<Crash>();
    }

    

    private void FixedUpdate()
    {
        if (!target)
        {
            if (GameObject.FindWithTag("Body"))
            {
                target = GameObject.FindWithTag("Body").transform;
                _crash = GameObject.Find("DeadZone").GetComponent<Crash>();
                return;
            }
        }

        if (_crash.isDead) return;
        if ((Object)rigid != (Object)null)
        {
            mainCamera.fieldOfView =
                Mathf.Clamp(
                    Mathf.Lerp(Camera.main.fieldOfView, rigid.velocity.magnitude * 1.5f + maxcamfieldOfView, 0.1f), minFOV,
                    maxFOV);
        }

        if (BikeController.Instance)
        {
            LerpHeight = target.position.y + height;
            CurrentHeight = target.position.y;
            CurrentHeight = Mathf.Lerp(CurrentHeight, LerpHeight, heightDamping * Time.deltaTime);
            transform.position = new Vector3(target.position.x-5f , CurrentHeight, target.position.z -10f); 
        }
    
    }
}
