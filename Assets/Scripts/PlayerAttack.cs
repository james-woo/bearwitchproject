using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : NetworkBehaviour {

    [SerializeField]
    private PlayerWeapon _weapon;
    [SerializeField]
    private GameObject _weaponGFX;
    [SerializeField]
    private string _weaponLayerName = "Weapon";

	[SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask mask;

    private const string PLAYER_TAG = "Player";

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_camera == null)
        {
            Debug.LogError("PlayerAttack: No camera referenced");
            this.enabled = false;
        }

        _weaponGFX.layer = LayerMask.NameToLayer(_weaponLayerName);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("Attacking");
            Attack();
        }
    }

    // Only called on client
    [Client]
    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(_camera.transform.position, 
                            _camera.transform.forward, 
                            out hit,
                            _weapon.range,
                            mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerAttacked(hit.collider.name, _weapon.damage);
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
