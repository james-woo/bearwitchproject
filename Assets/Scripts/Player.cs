using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    // Health
    [SerializeField]
    private float _maxHealth = 100f;
    [SerializeField]
    private float _healthRegen = 1f;
    // Synchronize players current health to all clients
    [SyncVar]
    private float _currentHealth;

    // Mana
    [SerializeField]
    private float _maxMana = 100f;
    [SerializeField]
    private float _manaRegen = 1f;
    // Synchronize players current mana to all clients
    [SyncVar]
    private float _currentMana;

    [SerializeField]
    private Behaviour[] _disableOnDeath;
    [SerializeField]
    private GameObject[] _disableGameObjectsOnDeath;
    [SerializeField]
    private bool[] _wasEnabled;
    
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    private Animator _animator;

	public void Setup()
    {
        _animator = GetComponent<Animator>();
        _wasEnabled = new bool[_disableOnDeath.Length];
        for (int i = 0; i < _wasEnabled.Length; i++)
        {
            _wasEnabled[i] = _disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    void Start()
    {
        InvokeRepeating("RegenerateHealth", 0.0f, 1.0f);
        InvokeRepeating("RegenerateMana", 0.0f, 1.0f);
    }

    void Update()
    {
        
    }

    public void SetDefaults()
    {
        _isDead = false;
        _currentHealth = _maxHealth;
        _currentMana = _maxMana;

        // Enable components
        for (int i = 0; i < _disableOnDeath.Length; i++)
        {
            _disableOnDeath[i].enabled = _wasEnabled[i];
        }

        // Enable gameobjects
        for (int i = 0; i < _disableGameObjectsOnDeath.Length; i++)
        {
            _disableGameObjectsOnDeath[i].SetActive(true);
        }

        // Enable collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
    }

    void RegenerateHealth()
    {
        if (_currentHealth < _maxHealth)
            _currentHealth += _healthRegen;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    void RegenerateMana()
    {
        if (_currentMana < _maxMana)
            _currentMana += _manaRegen;
    }

    public float GetCurrentMana()
    {
        return _currentMana;
    }

    public float GetMaxMana()
    {
        return _maxMana;
    }

    [ClientRpc]
    public void RpcSpendMana(float amount)
    {
        _currentMana -= amount;
        Debug.Log(transform.name + " now has " + _currentMana + " mana");
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

        for (int i = 0; i < _disableGameObjectsOnDeath.Length; i++)
        {
            _disableGameObjectsOnDeath[i].SetActive(false);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is dead");
        _animator.SetTrigger("Dead");

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        SetDefaults();

        Debug.Log(transform.name + " respawned");
        _animator.SetTrigger("Respawn");
    }
}
