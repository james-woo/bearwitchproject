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
    [SerializeField]
    private Transform _localWeaponHolder;
    [SerializeField]
    private Transform _globalWeaponHolder;

    private PlayerWeapon _currentWeapon;
    private WeaponGraphics _currentLocalGraphics;
    private WeaponGraphics _currentGlobalGraphics;

    void Start () {
        EquipWeapon(_primaryWeapon);
	}
	
    void EquipWeapon(PlayerWeapon weapon)
    {
        _currentWeapon = weapon;
        GameObject weaponGlobalInst = (GameObject)Instantiate(weapon.globalGraphics, _globalWeaponHolder.position, weapon.globalGraphics.transform.rotation);
        weaponGlobalInst.transform.SetParent(_globalWeaponHolder);
        _currentGlobalGraphics = weaponGlobalInst.GetComponent<WeaponGraphics>();

        if (isLocalPlayer)
        {
            GameObject weaponInst = (GameObject)Instantiate(weapon.localGraphics, _localWeaponHolder.position, weapon.localGraphics.transform.rotation);
            weaponInst.transform.SetParent(_localWeaponHolder);
            _currentLocalGraphics = weaponInst.GetComponent<WeaponGraphics>();
            if (_currentLocalGraphics == null)
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

    public WeaponGraphics GetCurrentLocalGraphics()
    {
        return _currentLocalGraphics;
    }

    public WeaponGraphics GetCurrentGlobalGraphics()
    {
        return _currentGlobalGraphics;
    }
}
