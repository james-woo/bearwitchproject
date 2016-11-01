using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string _weaponLayerName = "Weapon";
    [SerializeField]
    private PlayerWeapon _primaryWeapon;
    [SerializeField]
    private Transform _weaponHolder;

    private PlayerWeapon _currentWeapon;

	void Start () {
        EquipWeapon(_primaryWeapon);
	}
	
    void EquipWeapon(PlayerWeapon weapon)
    {
        if (isLocalPlayer)
        {
            _currentWeapon = weapon;
            GameObject weaponInst = (GameObject)Instantiate(weapon.graphics, _weaponHolder.position, weapon.graphics.transform.rotation);
            weaponInst.transform.SetParent(_weaponHolder);
            weaponInst.layer = LayerMask.NameToLayer(_weaponLayerName);
        }
    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }
}
