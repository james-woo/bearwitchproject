using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	[SerializeField]
	private float _speed = 5f;
	[SerializeField]
	private float _mouseSensitivity = 3f;

	private PlayerMotor _motor;
	private Animator _animator;

	// Use this for initialization
	void Start() 
	{
		_motor = GetComponent<PlayerMotor>();
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() 
	{
		var xMov = Input.GetAxisRaw("Horizontal");
		var zMov = Input.GetAxisRaw("Vertical");

		// Movement
		var movHorizontal = transform.right * xMov;
		var movVertical = transform.forward * zMov;
		var velocity = (movHorizontal + movVertical).normalized * _speed;
		_motor.Move (velocity);
		_animator.SetFloat("VSpeed", zMov);
		_animator.SetFloat("HSpeed", xMov);

		// Rotation
		var yRot = Input.GetAxisRaw("Mouse X");
		var xRot = Input.GetAxisRaw("Mouse Y");
		var rotation = new Vector3(0.0f, yRot, 0.0f) * _mouseSensitivity;
		var cameraRotation = new Vector3(xRot, 0.0f, 0.0f) * _mouseSensitivity;
		_motor.Rotate(rotation);
		_motor.RotateCamera(cameraRotation);

        // Attacking
        var attack = Input.GetButtonDown("Fire1");
        if(attack) 
        {
            _animator.SetTrigger("Attacking");
            Invoke("StopAttack", 0.01f);
        }
	}

    void StopAttack()
    {
        _animator.SetTrigger("StopAttacking");
    }
}
