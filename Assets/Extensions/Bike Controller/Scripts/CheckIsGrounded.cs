// CheckIsGrounded
using UnityEngine;

public class CheckIsGrounded : MonoBehaviour
{
	public bool IsGrounded = true;

	private void OnCollisionEnter(Collision col)
	{
		IsGrounded = true;
	}

	private void OnCollisionExit(Collision col)
	{
		IsGrounded = false;
	}

	private void OnCollisionStay(Collision col)
	{
		IsGrounded = true;
	}
}
