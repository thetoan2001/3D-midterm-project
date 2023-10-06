using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    [Header ("Sound")]
    [SerializeField]  private SoundManager _soundManager;
    
    
    [Header("Physics")]
    [Tooltip("Acceleration of the character speed")]
    [SerializeField] private float _playerAcceleration = 5f;
    [Tooltip ("Speed to rotate the player when the cam rotate")]
    [SerializeField] private float _rotationSpeed = 5f;
    private Physics _phy;
    private Transform _playerCam;
    private bool _jump, _dodge;
    //input values
    private bool doJump, doDodge;
    private Vector2 _inputVector;

    [Header("Idle Animation")]
    [Tooltip("first idle animation clip in the idle tree node (animator)")]
    [SerializeField] private AnimationClip idle2;
    [Tooltip("last idle animation clip in the idle tree node (animator)")]
    [SerializeField] private AnimationClip idle3;
    [Tooltip("time to change de default idle animation to other")]
    [SerializeField] private float _defaultIdleDuration = 10f;
    private float _timeIdle = 10;
    private float _currentIdle = 0;
    private float _oldIdle = -1;
    private float _targetIdle = 0;

    void Start()
    {
        _phy = new Physics(_playerAcceleration);
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        if(Camera.main)
            _playerCam = Camera.main.transform;
        _timeIdle = _defaultIdleDuration;
    }

    void Update()
    {
        resetAnimatorValues();

        updateMovement();
        
        updateJumpAndDodge();
        
        setAnimatorValues();
    }

    private void resetAnimatorValues()
    {
        if (_jump&&!_animator.GetBool("Jump"))
        {
            _jump = false;
        }

        if (_dodge&&!_animator.GetBool("Dodge"))
            _dodge = false;
    }
    
    private void updateMovement()
    {
        _phy.updateMovement(_inputVector);
        rotatePlayer();
        updateIdle();
        if(_soundManager)
            _soundManager.playMove(_phy.getSpeed());
    }

    //Rotate de player with the camera
    private void rotatePlayer()
    {
        if (_playerCam)
        {
            Quaternion targetRotation = Quaternion.Euler(0, _playerCam.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    //Change idle animation when time passed
    private void updateIdle()
    {
        if (_phy.getSpeed() < 0.1)
        {
            _timeIdle -= Time.deltaTime;
            if (_timeIdle <= 0)
            {
                if (_targetIdle == 0) //alternate between the idles animations
                {
                    _oldIdle *= -1;
                    _targetIdle = _oldIdle;
                }
                else //return to animation default
                    _targetIdle = 0;
                

                switch (_targetIdle)
                {
                    case 1: _timeIdle = idle3.averageDuration; break;
                    case -1: _timeIdle = idle2.averageDuration; break;
                    default: _timeIdle = _defaultIdleDuration; break;
                }
            }

        }
        else
        {
            _timeIdle = _defaultIdleDuration;
        }

        //interpolate the idle parameter
        float idleSpeed = 2f;
        _currentIdle = Mathf.Lerp(_currentIdle, _targetIdle, idleSpeed * Time.deltaTime);
        _currentIdle = Mathf.Clamp(_currentIdle, -1, 1);
    }

    

    private void updateJumpAndDodge()
    {
        if (_controller.isGrounded)
        {
            if (doJump)
            {
                _soundManager.playJump();
                _jump = true;
            }
            else if (doDodge)
            {
                _dodge = true;
                _soundManager.playDodge();
            }
        }
    }

    private void setAnimatorValues()
    {
        _animator.SetFloat("Speed X", _phy.getVelocity().x);
        _animator.SetFloat("Speed Z", _phy.getVelocity().y);
        _animator.SetFloat("Speed", _phy.getSpeed());

        _animator.SetBool("Run", _phy.getRun());

        _animator.SetBool("Jump", _jump);

        _animator.SetBool("Dodge", _dodge);

        _animator.SetFloat("IdleType", _currentIdle);
    }

    public void setInputValues(bool run, bool jump, bool dodge, Vector2 velocity)
    {
        if (run)
            _phy.setRun(!_phy.getRun());

        doJump = jump;
        doDodge = dodge;
        _inputVector = velocity;
    }

}
