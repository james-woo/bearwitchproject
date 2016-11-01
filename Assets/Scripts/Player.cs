using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

    [SerializeField]
    private float _maxHealth = 100f;
    [SerializeField]
    private Behaviour[] _disableOnDeath;
    [SerializeField]
    private bool[] _wasEnabled;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    // Synchronize players current health to all clients
    [SyncVar]
    private float _currentHealth;

	public void Setup()
    {
        _wasEnabled = new bool[_disableOnDeath.Length];
        for (int i = 0; i < _wasEnabled.Length; i++)
        {
            _wasEnabled[i] = _disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        _isDead = false;
        _currentHealth = _maxHealth;
        for (int i = 0; i < _disableOnDeath.Length; i++)
        {
            _disableOnDeath[i].enabled = _wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    // For all clients connected
    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        if (_isDead) return;
        _currentHealth -= damage;
        Debug.Log(transform.name + " now has " + _currentHealth + " health");

        if (_currentHealth <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        _isDead = true;

        // Disable components
        for (int i = 0; i < _disableOnDeath.Length; i++)
        {
            _disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log(transform.name + " is dead");

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " respawned");
    }
}
