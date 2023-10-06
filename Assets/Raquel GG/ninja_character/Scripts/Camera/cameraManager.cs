using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class cameraManager : MonoBehaviour
{
    [Header ("Cemeras")]
    [Tooltip("Default camera")]
    [SerializeField] private CinemachineVirtualCamera _firstCam;
    [Tooltip("Secondary camera")]
    [SerializeField] private CinemachineVirtualCamera _secondCam;
    [Tooltip("How much do I increase the priority of the cinemachine cam")]
    [SerializeField] private int _priorityBoostAmount = 5;
    private bool _active = false;

    //Alternate the camera active between the first and second cam
    public void ChangeCam()
    {
        if (!_active)
        {
            _active = true;
            _secondCam.Priority += _priorityBoostAmount;
        }
        else
        {
            _secondCam.Priority -= _priorityBoostAmount;
            _active = false;
        }
    }
}
