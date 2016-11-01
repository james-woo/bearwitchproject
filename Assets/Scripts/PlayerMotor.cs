using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera _camera;
    [SerializeField]
    private float _cameraRotationLimit = 85.0f;

	private Vector3 _velocity = Vector3.zero;
	private Vector3 _rotate = Vector3.zero;
    private float _cameraRotateX = 0.0f;
    private float _currentCameraRotateX = 0.0f;
    private Vector3 _jumpForce = Vector3.zero;
    private bool _grounded = true;
	private Rigidbody _rb;
    private Animator _animator;

	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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

        if (!_grounded && _rb.velocity.y == 0) 
        {
            _grounded = true;
        }

        if (_jumpForce != Vector3.zero && _grounded == true)
        {
            _rb.AddForce(_jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            _grounded = false;
            _animator.SetTrigger("Jumping");
        }
	}

	void PerformRotation()
	{
		_rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotate));
		if (_camera != null)
		{
            // Set rotation and clamp it
            _currentCameraRotateX -= _cameraRotateX;
            _currentCameraRotateX = Mathf.Clamp(_currentCameraRotateX, -_cameraRotationLimit, _cameraRotationLimit);
            // Apply rotation to the transform of the camera
            _camera.transform.localEulerAngles = new Vector3(_currentCameraRotateX, 0f, 0f);
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

	public void RotateCamera(float rotate)
	{
		_cameraRotateX = rotate;
	}

    public void Jump(Vector3 jumpForce)
    {
        _jumpForce = jumpForce;
    }
}
