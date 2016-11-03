using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint _roomSize = 10;
    private string _roomName;
    private string _roomPassword;
    private NetworkManager _networkManager;

    void Start()
    {
        _networkManager = NetworkManager.singleton;
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string name)
    {
        _roomName = name;
    }

    public void SetRoomPassword(string password)
    {
        if (password == null)
        {
            _roomPassword = "";
            return;
        }
        _roomPassword = password;
    }
    public void CreateRoom()
    {
        if (_roomName != "" && _roomName != null)
        {
            Debug.Log("Creating room " + _roomName + " with room for " + _roomSize);
            // Create the room
            
            _networkManager.matchMaker.CreateMatch(_roomName, _roomSize, true, _roomPassword, "", "", 0, 0, _networkManager.OnMatchCreate);
        }
    }
}
