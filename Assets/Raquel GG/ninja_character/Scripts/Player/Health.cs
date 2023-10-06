using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Health : MonoBehaviour
{
    private Animator _animator;

    [Header("Sound")]
    [SerializeField]  private SoundManager _soundManager;

    [Header("HP")]
    [SerializeField] private float _maxHealth = 100;
    private float _health;
    private float _inflictDamage = 0;
    private bool _react = false;
    private bool _die = false;
    private float _timeDie;
    [Tooltip ("Time in seconds to deactive the object when die")]
    [SerializeField] private float _durationDie = 10;

    [Header("HUD")]
    [SerializeField] private TMP_Text _hptext;

    void Start()
    {
        _health = _maxHealth;
        setTextHP();
        _animator = GetComponent<Animator>();
    }


    void Update()
    {
        resetValues();
        updateHealth();
        setAnimatorValues();
        deactiveObjectDeath();
    }

    private void resetValues()
    {
        if ((_animator && !_animator.GetBool("React"))||!_animator)
        {
            _react = false;
        }
        _die = false;

    }

    private void updateHealth()
    {
        if (!_react && _inflictDamage > 0 && _health > 0)
        {
            _health -= _inflictDamage;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
           
            _react = true;

            if (_health <= 0)
                _die = true;

            setTextHP();
            if (_soundManager) 
                _soundManager.playReact();
        }

        _inflictDamage = 0;
    }

    private void deactiveObjectDeath()
    {
        if (_health <= 0)
        {
            if (_animator)
            {
                _timeDie += Time.deltaTime;
                if (_timeDie >= _durationDie)
                    gameObject.SetActive(false);
            }
            else
                gameObject.SetActive(false);
        }
    }
    private void setAnimatorValues()
    {
        if (_animator)
        {
            _animator.SetBool("Die", _die);
            _animator.SetBool("React", _react);
        }
    }

    private void setTextHP()
    {
        if (_hptext)
        {
            float healthFloor = Mathf.Floor(_health);
            _hptext.SetText("HP: " + healthFloor.ToString() + "/" + _maxHealth.ToString());
        }
    }

    public void setDamage(float dam)
    {
        _inflictDamage = dam;
    }

    public bool getReact()
    {
        return _react;
    }

}
