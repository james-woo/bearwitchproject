using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {
    
    [SerializeField]
    private Behaviour[] _componentsToDisable;
    [SerializeField]
    private string _remoteLayerName = "RemotePlayer";
    [SerializeField]
    private string _dontDrawLayerName = "DontDraw";
    [SerializeField]
    private GameObject _playerGraphics;
    [SerializeField]
    private GameObject _remoteGraphics;
    [SerializeField]
    private GameObject _playerUIPrefab;
    private GameObject _playerUIInstance;

    private Camera _sceneCamera;

    void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();   
            AssignRemoteLayer();
            // Disable player graphics for remote player
            SetLayerRecursively(_remoteGraphics, LayerMask.NameToLayer(_dontDrawLayerName));
        }
        else
        {
            _sceneCamera = Camera.main;
            if(_sceneCamera != null)
            {
                _sceneCamera.gameObject.SetActive(false);
            }

            // Disable player graphics for local player
            SetLayerRecursively(_playerGraphics, LayerMask.NameToLayer(_dontDrawLayerName));
            
            // Create player ui
            _playerUIInstance = Instantiate(_playerUIPrefab);
            _playerUIInstance.name = _playerUIPrefab.name;

            // Configure player ui
            PlayerUI ui = _playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component on PlayerUI prefab");
            }

            ui.SetPlayer(GetComponent<Player>());
        }

        GetComponent<Player>().Setup();
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netid = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netid, player);
    }

    void DisableComponents()
    {
        foreach(var c in _componentsToDisable)
        {
            c.enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(_remoteLayerName);
    }

    void OnDisable()
    {
        Destroy(_playerUIInstance);

        if(_sceneCamera != null)
        {
            _sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}
