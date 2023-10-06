using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private SoundManager _soundManager;

    private GameObject _owner;
    private Animator _animator;
    private Health _health;

    [Header("Parent")]
    [Tooltip("Object to copy transform (hand position)")]
    [SerializeField] private GameObject _parentObject;

    [Header("Damage")]
    [SerializeField] private float _damage = 30;
    [SerializeField] private float _damageSpecialAttack = 60;
    [Tooltip("Tag of the object to attack")]
    [SerializeField] private string _tagEnemy = "Enemy";
    private Collider _enemy;

    [Header("Stamina")]
    [SerializeField] private float _staminaSpAttack = 10;
    private Stamina _stamina;

    private bool _attack = false;
    private bool _specialAttack = false;
    private bool _damageOneTime = false;

    //input values
    private bool _doAttack = false;
    private bool _doSpecialAttack = false;
    
    

    //Init object with the owner components
    public void initObjects()
    {
        if (_owner)
        {
            _animator = _owner.GetComponent<Animator>();
            _health = _owner.GetComponent<Health>();
            _stamina = _owner.GetComponent<Stamina>();
            if (_staminaSpAttack > _stamina.getMaxStamina())
                _staminaSpAttack = _stamina.getMaxStamina();

            transform.SetParent(_parentObject.transform);
            transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }
    }

    private void Update()
    {
        resetValues();
        updateAttack();       
        setAnimatorValues();
    }

    private void resetValues()
    {
        if (_animator && !_animator.GetBool("Attack"))
        {
            _attack = false;
            _specialAttack = false;
            _damageOneTime = false;
            
        }
    }

    
    private void updateAttack()
    {
        //If attack is false and the owner doesn't reacting or haven't health, then check attack
        if (!_attack && (!_health || _health && !_health.getReact()))
        {
            if (_doSpecialAttack)
            {
                //Check the stamina for the special attack
                if (!_stamina || (_stamina && _stamina.useStamina(_staminaSpAttack)))
                {
                    if(_soundManager)
                        _soundManager.playSpecialAttack();
                    _attack = true;
                    _specialAttack = true;
                    _damageOneTime = false;
                }
            }
            else if (_doAttack)
            {
                if (_soundManager)
                    _soundManager.playAttack();
                _attack = true;
                _specialAttack = false;
                _damageOneTime = false;
            }

            
        }

        inflictDamage();
    }


    private void inflictDamage()
    {
        //If have a enemy collision and the weapon doesn't inflict damage yet, can inflict damage
        if (_enemy && !_damageOneTime)
        {
            if (_specialAttack)
            {
                _enemy.gameObject.GetComponent<Health>().setDamage(_damageSpecialAttack);
                _damageOneTime = true;
                _soundManager.playAttackCollision();
            }
            else if (_attack)
            {
                _enemy.gameObject.GetComponent<Health>().setDamage(_damage);
                _damageOneTime = true;
                _soundManager.playAttackCollision();
            }
            
        }

        _enemy = null;
    }

    private void setAnimatorValues()
    {
        if (_animator)
        {
            _animator.SetBool("Attack", _attack);
            _animator.SetBool("SpecialAttack", _specialAttack);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Save the enemy object
        if (other.gameObject.tag.Equals(_tagEnemy))
        {
            _enemy = other;
        }
    }

    public void setInputValues(bool attack, bool spAttack)
    {
        _doAttack = attack;
        _doSpecialAttack = spAttack;
    }

    public bool getAttacking()
    {
        return _attack;
    }

    public void setOwner(GameObject owner)
    {
        _owner = owner;
        initObjects();
    }

}
