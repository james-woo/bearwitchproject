using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
	[SerializeField]
	private float _speed = 5f;
	[SerializeField]
	private float _mouseSensitivity = 3f;
    [SerializeField]
    private float _jumpForce = 300f;

    [Header("ConfigurableJoint Options:")]
    [SerializeField]
    private float _jointSpring = 15;
    [SerializeField]
    private float _jointMaxForce = 40;

	private PlayerMotor _motor;
	private Animator _animator;
    private ConfigurableJoint _joint;

	// Use this for initialization
	void Start() 
	{
		_motor = GetComponentInChildren<PlayerMotor>();
		_animator = GetComponentInChildren<Animator>();
        _joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(_jointSpring);
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
		var cameraRotationX = xRot * _mouseSensitivity;
		_motor.Rotate(rotation);
		_motor.RotateCamera(cameraRotationX);

        // Jumping
        Vector3 jumpForce = Vector3.zero;
        if (Input.GetButtonDown("Jump"))
        {
            jumpForce = Vector3.up * _jumpForce;
            SetJointSettings(0.0f);
        } 
        else
        {
            SetJointSettings(_jointSpring);
        }
        _motor.Jump(jumpForce);
	}

    void SetJointSettings(float jointSpring)
    {
        _joint.yDrive = new JointDrive
        {
            positionSpring = jointSpring,
            maximumForce = _jointMaxForce
        };
    }
}
