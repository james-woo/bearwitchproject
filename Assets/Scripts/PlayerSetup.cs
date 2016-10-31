using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {
    
    [SerializeField]
    private Behaviour[] _componentsToDisable;
    [SerializeField]
    private string _remoteLayerName = "RemotePlayer";

    private Camera _sceneCamera;

    void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();   
            AssignRemoteLayer();
        }
        else
        {
            _sceneCamera = Camera.main;
            if(_sceneCamera != null)
            {
                _sceneCamera.gameObject.SetActive(false);
            }
        }

        RegisterPlayer();
    }

    void RegisterPlayer()
    {
        // Get reference to network identity
        string id = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = id;
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
        if(_sceneCamera != null)
        {
            _sceneCamera.gameObject.SetActive(true);
        }
    }
}
