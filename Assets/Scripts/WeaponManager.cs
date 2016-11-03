using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {
    [SerializeField]
    private string _dontDrawLayerName = "DontDraw";

    [SerializeField]
    private string _weaponLayerName = "Weapon";
    [SerializeField]
    private PlayerWeapon _primaryWeapon;

    // The player has a local and a global weapon, they look different but are the same weapon
    [SerializeField]
    private Transform _localWeaponHolder;           // What the local user sees
    [SerializeField]
    private Transform _globalWeaponHolder;          // What the local user sees for other users

    private PlayerWeapon _currentWeapon;
    private WeaponGraphics _currentGraphics;

    void Start () {
        EquipWeapon(_primaryWeapon);
	}
	
    void EquipWeapon(PlayerWeapon weapon)
    {
        _currentWeapon = weapon;
        GameObject weaponGlobalInst = (GameObject)Instantiate(weapon.graphics, _globalWeaponHolder.position, weapon.graphics.transform.rotation);
        weaponGlobalInst.transform.SetParent(_globalWeaponHolder);
        _currentGraphics = weaponGlobalInst.GetComponent<WeaponGraphics>();

        if (isLocalPlayer)
        {
            GameObject weaponInst = (GameObject)Instantiate(weapon.graphics, _localWeaponHolder.position, weapon.graphics.transform.rotation);
            weaponInst.transform.SetParent(_localWeaponHolder);

            // Change the look locally
            weaponInst.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            weaponInst.transform.localPosition = new Vector3(0f, 0f, 0f);

            _currentGraphics = weaponInst.GetComponent<WeaponGraphics>();
            if (_currentGraphics == null)
            {
                Debug.LogError("No graphics effects on weapon " + weaponInst.name);
            }
            weaponGlobalInst.layer = LayerMask.NameToLayer(_dontDrawLayerName);
            Util.SetLayerRecursively(weaponInst, LayerMask.NameToLayer(_weaponLayerName));
        }

    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return _currentGraphics;
    }
}
