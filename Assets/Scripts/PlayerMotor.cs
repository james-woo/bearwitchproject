using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera _camera;

	private Vector3 _velocity = Vector3.zero;
	private Vector3 _rotate = Vector3.zero;
	private Vector3 _cameraRotate = Vector3.zero;
	private Rigidbody _rb;

	// Use this for initialization
	void Start () 
	{
		_rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() 
	{
		PerformMovement ();
		PerformRotation ();
	}

	void PerformMovement()
	{
		if (_velocity != Vector3.zero) 
		{
			_rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation()
	{
		_rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotate));
		if(_camera != null)
		{
			_camera.transform.Rotate(-_cameraRotate);
		}
	}

	public void Move(Vector3 velocity)
	{
		_velocity = velocity;
	}

	public void Rotate(Vector3 rotate)
	{
		_rotate = rotate;
	}

	public void RotateCamera(Vector3 rotate)
	{
		_cameraRotate = rotate;
	}
}
