// WheelRotation
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
	public enum WheelType
	{
		Front,
		Rear
	}

	public WheelType wheelType;

	private Transform thisTransform;

	private BikeController bikeController;

	private float speed = 1.5f;

	private Rigidbody rearRigidWheel;

	private Transform frontTransWheel;

	public enum  Axis
	{
		X,Z
	}

	public Axis axis;
	private bool axisX;
	private void Start()
	{
		thisTransform = base.transform;
		if ((Object)bikeController == (Object)null)
		{
			bikeController = Object.FindObjectOfType<BikeController>();
		}
        rearRigidWheel = GameObject.FindGameObjectWithTag("RearWheel").GetComponent<Rigidbody>();
        frontTransWheel = wheelType == WheelType.Front ?
            GameObject.FindGameObjectWithTag("FrontWheel").GetComponent<Transform>() : GameObject.FindGameObjectWithTag("RearWheel").GetComponent<Transform>();

        if (axis == Axis.X)
        {
	        axisX = true;
        }
	}

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			thisTransform.position = frontTransWheel.gameObject.transform.position;
			if (!bikeController.inAir)
			{
				if (axisX)
				{
					if (rearRigidWheel.velocity.x > 0)
					{
						thisTransform.Rotate(rearRigidWheel.velocity.magnitude * speed,0f,0f);
					}
					else
					{
						thisTransform.Rotate(rearRigidWheel.velocity.magnitude * (0f -speed),0f,0f);
					}
				}
				else
				{
				if (rearRigidWheel.velocity.z > 0)
                {
                    thisTransform.Rotate(0f,0f,rearRigidWheel.velocity.magnitude * speed);
                }
                else
                {
                    thisTransform.Rotate(0f,0f,rearRigidWheel.velocity.magnitude * (0f -speed));
                }
					
				}
            }
		}
	}
}
