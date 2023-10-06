using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Input : MonoBehaviour
{
    private PlayerController _player;
    [Header ("Weapon")]
    [SerializeField] private WeaponManager _weaponManager;
    [Header("Camera")]
    [SerializeField] private cameraManager _camManager;

    private Vector2 _inputVector;
    private bool _doRun, _doJump, _doDodge, _doAttack, _doSpecialAttack;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GetComponent<PlayerController>();
    }


    void Update()
    {
        if ((_weaponManager && _weaponManager.getCurrentWeapon().getAttacking())||_doAttack||_doSpecialAttack) //if i'm attacking, I can't jump or dodge
        {
            _doJump = _doDodge = false;
        }
        if(_player)
            _player.setInputValues(_doRun, _doJump, _doDodge, _inputVector);
        if(_weaponManager)
            _weaponManager.getCurrentWeapon().setInputValues(_doAttack,_doSpecialAttack);
        reestartInputValues();
    }

    private void reestartInputValues()
    {
        _doRun = _doJump = _doDodge = _doAttack = _doSpecialAttack = false;
    }


    //Input methods
    private void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }

    private void OnAttack(InputValue value)
    {
        _doAttack = true;
    }

    private void OnSpecialAttack(InputValue value)
    {
        _doSpecialAttack = true;
    }

    private void OnRun(InputValue value)
    {
        _doRun = true;
    }

    private void OnJump(InputValue value)
    {
        _doJump = true;
    }

    private void OnDodge(InputValue value)
    {
        _doDodge = true;
    }

    private void OnCamera(InputValue value)
    {
        if(_camManager)
            _camManager.ChangeCam();
    }

    private void OnWeapon(InputValue value)
    {
        if(_weaponManager)
            _weaponManager.changeWeapon();
    }
}
