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
    [HideInInspector]
    public GameObject playerUIInstance;

    void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();   
            AssignRemoteLayer();
            // Disable player graphics for remote player
            Util.SetLayerRecursively(_remoteGraphics, LayerMask.NameToLayer(_dontDrawLayerName));
        }
        else
        {
            // Disable player graphics for local player
            Util.SetLayerRecursively(_playerGraphics, LayerMask.NameToLayer(_dontDrawLayerName));
            // Create player ui
            playerUIInstance = Instantiate(_playerUIPrefab);
            playerUIInstance.name = _playerUIPrefab.name;

            // Configure player ui
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component on PlayerUI prefab");
            }

            ui.SetPlayer(GetComponent<Player>());
            GetComponent<Player>().SetupPlayer();
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
        Destroy(playerUIInstance);
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }
}
