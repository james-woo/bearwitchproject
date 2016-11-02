using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject _sceneCamera;
     
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("GameManager: More than one GameManager in scene");
        }
        else
        {
            instance = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (_sceneCamera == null)
        {
            Debug.Log("GameManager: Scene camera is null");
            return;
        }
        _sceneCamera.SetActive(isActive);
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    // Key: ID Value: Player
    private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
	
    public static void RegisterPlayer(string netid, Player player)
    {
        string playerid = PLAYER_ID_PREFIX + netid;
        _players.Add(playerid, player);
        player.transform.name = playerid;
    }

    public static void UnRegisterPlayer(string playerid)
    {
        _players.Remove(playerid);
    }

    public static Player GetPlayer(string playerid)
    {
        return _players[playerid];
    }

    /*void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach(var pid in _players.Keys)
        {
            GUILayout.Label(pid + "  -  " + _players[pid].transform.name);
            Debug.Log(pid + "  -  " + _players[pid].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/

    #endregion
}
