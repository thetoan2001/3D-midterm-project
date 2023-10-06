using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class aiController : MonoBehaviour
{
    private Animator _animator;
    [Header("Sound")]
    [SerializeField] private SoundManager _soundManager;
    [Header("Weapon")]
    [SerializeField] private WeaponManager _weaponManager;

    [Header("AI Movement")]
    [Tooltip ("Time in seconds to change direction")]
    [SerializeField] private float _timeChangeDirection = 5f;
    private float _timeMovement = 0f;
    

    [Header("Physics")]
    private Physics _phy;
    [Tooltip("Vector to indicate the direction of the movement")]
    [SerializeField] private Vector2 _directionVector;
    [Tooltip("Aceleration of the character speed")]
    [SerializeField] private float _enemyAcceleration = 5f;
    [Tooltip ("If the character are running")]
    [SerializeField] private bool _run;

    [Header("Attack")]
    [Tooltip("Time in seconds to attack")]
    [SerializeField] private float _timeToAttack = 10f;
    [Tooltip("Every how many attacks does the special attack")]
    [SerializeField] private int _specialAttack = 5;
    private int _contSpecialAttack = 0;
    private float _timeAttack = 0f;


    void Start()
    {
        _animator = GetComponent<Animator>();
        //initialize physics
        _phy = new Physics(_run, _enemyAcceleration);
    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();
        updateAttack();
        setAnimatorValues();
    }

    private void updateMovement()
    {
        changeDirection();
        _phy.updateMovement(_directionVector);
        _soundManager.playMove(_phy.getSpeed());
    }

    private void changeDirection()
    {
        _timeMovement += Time.deltaTime;
        if (_timeMovement >= _timeChangeDirection)
        {
            _directionVector.x *= -1;
            _directionVector.y *= -1;
            _timeMovement = 0;
        }
    }

    private void updateAttack()
    {
        bool attack = false;
        bool specialAttack = false;

        _timeAttack += Time.deltaTime;

        if (_timeAttack >= _timeToAttack)
        {
            _timeAttack = 0;
            _contSpecialAttack++;
            if (_contSpecialAttack >= _specialAttack)
            {
                specialAttack = true;
                _contSpecialAttack = 0;
            }
            else
                attack = true;

        }
            
        _weaponManager.getCurrentWeapon().setInputValues(attack, specialAttack);
    }

    private void setAnimatorValues()
    {
        _animator.SetFloat("Speed X", _phy.getVelocity().x);
        _animator.SetFloat("Speed Z", _phy.getVelocity().y);
        _animator.SetFloat("Speed", _phy.getSpeed());
    }
}
