using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {
    
    [SerializeField]
    private Behaviour[] _componentsToDisable;

    private Camera _sceneCamera;

    void Start()
    {
        if(!isLocalPlayer)
        {
            foreach(var c in _componentsToDisable)
            {
                c.enabled = false;
            }
        }
        else
        {
            _sceneCamera = Camera.main;
            if(_sceneCamera != null)
            {
                _sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    void OnDisable()
    {
        if(_sceneCamera != null)
        {
            _sceneCamera.gameObject.SetActive(true);
        }
    }
}
