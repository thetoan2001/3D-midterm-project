using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics
{
    private Vector2 _velocityVector;
    private float _acceleration = 5f;
    private float _speed = 0;
    private bool _run = false;


    public Physics(bool run, float acceleration)
    {
        _run = run;
        _acceleration = acceleration;
        _speed = 0;
        _velocityVector = new Vector2(0, 0);
    }

    public Physics(float acceleration)
    {
        _run = false;
        _acceleration = acceleration;
        _speed = 0;
        _velocityVector = new Vector2(0, 0);
    }


    public void updateMovement(Vector2 targetVector)
    {
        //Interpolate the actual velocity vector and the target vector and clamp it
        _velocityVector.x = Mathf.Lerp(_velocityVector.x, targetVector.x, _acceleration * Time.deltaTime);
        _velocityVector.y = Mathf.Lerp(_velocityVector.y, targetVector.y, _acceleration * Time.deltaTime);
        _velocityVector.x = Mathf.Clamp(_velocityVector.x, -1, 1);
        _velocityVector.y = Mathf.Clamp(_velocityVector.y, -1, 1);

        updateSpeed();
    }

    public void updateSpeed()
    {
        //If there are any velocity component, the speed increments
        float targetSpeed = 0;
        if (_velocityVector.y > 0.1 || _velocityVector.y < -0.1 || _velocityVector.x > 0.1 || _velocityVector.x < -0.1)
        {
            targetSpeed = 1;
            if (_run) targetSpeed = 2;
        }

        //Interpolate the current speed and target speed and clamp it
        _speed = Mathf.Lerp(_speed, targetSpeed, _acceleration * Time.deltaTime);
        _speed = Mathf.Clamp(_speed, 0, 2);
    }

    public float getSpeed()
    {
        return _speed;
    }

    public Vector2 getVelocity()
    {
        return _velocityVector;
    }

    public bool getRun()
    {
        return _run;
    }

    public float getAcceleration()
    {
        return _acceleration;
    }

    public void setAcceleration(float a)
    {
        _acceleration = a;
    }

    public void setRun(bool r)
    {
        _run = r;
    }
}
