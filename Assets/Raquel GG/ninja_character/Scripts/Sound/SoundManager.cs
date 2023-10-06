using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Follow")]
    [Tooltip ("Object to follow position (character)")]
    [SerializeField] private Transform _objectFollow;
    [Header("Audio Source")]
    [SerializeField] private AudioSource _footsteps;
    [SerializeField] private AudioSource _footstepsRun;
    [SerializeField] private AudioSource _footstepsJump;
    [SerializeField] private AudioSource _attack;
    [SerializeField] private AudioSource _attackCollision;
    [SerializeField] private AudioSource _specialAttack;
    [SerializeField] private AudioSource _voicePain;
    [SerializeField] private AudioSource _voiceAttack;
    [SerializeField] private AudioSource _voiceSpecialAttack;
    [SerializeField] private AudioSource _voiceJump;
    [SerializeField] private AudioSource _voiceEffort;

    private void Start()
    {
        if (!_footsteps.clip) _footsteps = null;
        if (!_footstepsRun.clip) _footstepsRun = null;
        if (!_footstepsJump.clip) _footstepsJump = null;
        if (!_attack.clip) _attack = null;
        if (!_attackCollision.clip) _attackCollision = null;
        if (!_specialAttack.clip) _specialAttack = null;
        if (!_voicePain.clip) _voicePain = null;
        if (!_voiceAttack.clip) _voiceAttack = null;
        if (!_voiceSpecialAttack.clip) _voiceSpecialAttack = null;
        if (!_voiceJump.clip) _voiceJump = null;
        if (!_voiceEffort.clip) _voiceEffort = null;
    }
    private void Update()
    {
       
        //Move the object position to the objectFollow
        if(_objectFollow)
            transform.localPosition = _objectFollow.localPosition;
        
    }

    //Sound event methods
    public void playAttack()
    {
        stopMove();
        if(_attack) _attack.PlayDelayed(0.5f);
        if(_voiceAttack) _voiceAttack.PlayDelayed(0.5f);
    }

    public void playSpecialAttack()
    {
        stopMove();
        if(_specialAttack) _specialAttack.Play();
        if(_voiceSpecialAttack) _voiceSpecialAttack.Play();
    }

    public void playJump()
    {
        stopMove();
        if(_voiceJump) _voiceJump.Play();
        if(_footstepsJump) _footstepsJump.PlayDelayed(0.9f);
    }

    public void playDodge()
    {
        stopMove();
        if (_footstepsJump)  _footstepsJump.Play();
        if (_voiceEffort)  _voiceEffort.Play();
    }

    public void playAttackCollision()
    {
        stopMove();
        if(_attackCollision) _attackCollision.Play();
    }

    public void playReact()
    {
        stopMove();
        if(_voicePain) _voicePain.Play();
    }

    public void playMove(float speed)
    {
        if (speed < 0.1)
        {
            stopMove();
        }
        else if (!playOtherActions())
        {
            if (speed <= 1)
                playWalk();
            else
                playRun();
        }
    }

    private void playWalk()
    {
        if(_footstepsRun && _footstepsRun.isPlaying)
            _footstepsRun.Stop();

        if(_footsteps && !_footsteps.isPlaying)
             _footsteps.Play();
    }
    private void playRun()
    {
        if (_footsteps && _footsteps.isPlaying)
            _footsteps.Stop();

        if (_footstepsRun && !_footstepsRun.isPlaying)
            _footstepsRun.Play();
    }

    public void stopMove()
    {
        if(_footsteps) _footsteps.Stop();
        if(_footstepsRun) _footstepsRun.Stop();
    }

    private bool playOtherActions()
    {
        if ((_footstepsJump &&_footstepsJump.isPlaying) || (_voiceAttack && _voiceAttack.isPlaying))
            return true;
        return false;
    }
}
