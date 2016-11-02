using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private RectTransform _healthForeground;
    [SerializeField]
    private RectTransform _manaForeground;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private Text _manaText;

    private PlayerController _controller;
    private Player _player;

    private float _maxHealth;
    private float _maxMana;

    void Update()
    {
        SetHealth(_player.GetCurrentHealth());
        SetMana(_player.GetCurrentMana());
    }
    
    public void SetPlayer(Player player)
    {
        _player = player;
        _maxHealth = _player.GetMaxHealth();
        _maxMana = _player.GetMaxMana();
    }

    void SetHealth(float amount)
    {
        amount = Mathf.Clamp(amount, 0, _maxHealth);
        _healthForeground.localScale = new Vector3(amount/_maxHealth, 1f, 1f);
        _healthText.text = amount.ToString() + "/" + _maxHealth.ToString();
    }

    void SetMana(float amount)
    {
        amount = Mathf.Clamp(amount, 0, _maxMana);
        _manaForeground.localScale = new Vector3(amount/_maxMana, 1f, 1f);
        _manaText.text = amount.ToString() + "/" + _maxMana.ToString();
    }
}
