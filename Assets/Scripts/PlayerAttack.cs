using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(WeaponManager))]
public class PlayerAttack : NetworkBehaviour {

   	[SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask mask;

    private const string PLAYER_TAG = "Player";

    private Animator _animator;
    private WeaponManager _weaponManager;
    private PlayerWeapon _currentWeapon;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _weaponManager = GetComponent<WeaponManager>();

        if (_camera == null)
        {
            Debug.LogError("PlayerAttack: No camera referenced");
            this.enabled = false;
        }
    }

    void Update()
    {
        _currentWeapon = _weaponManager.GetCurrentWeapon();

        if (_currentWeapon.fireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Attack", 0f, 1f/_currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Attack");
            }
        }
    }

    // Only called on client
    [Client]
    void Attack()
    {
        _animator.SetTrigger("Attacking");
        RaycastHit hit;

        if (Physics.Raycast(_camera.transform.position, 
                            _camera.transform.forward, 
                            out hit,
                            _currentWeapon.range,
                            mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerAttacked(hit.collider.name, _currentWeapon.damage);
            }
        }
    }

    // Only called on server
    [Command]
    void CmdPlayerAttacked(string playerid, float damage)
    {
        Debug.Log(playerid + " has been attacked");
        Player player = GameManager.GetPlayer(playerid);
        player.RpcTakeDamage(damage);
    }
}
