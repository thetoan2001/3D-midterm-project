using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float _maxStamina = 6f;
    private float _stamina = 0;
    [Header("HUD")]
    [SerializeField] private TMP_Text _staminaText;


    void Update()
    {
        if (_stamina<_maxStamina)
        {
            _stamina += Time.deltaTime;
        }
        _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);

        if(_staminaText)
            setTextStamina();
    }

    public bool useStamina(float points)
    {
        if (points <= _stamina)
        {
            _stamina -= points;
            return true;
        }
        return false;
    }

    //Use all the stamina
    public bool useStamina()
    {
        if (_stamina >= _maxStamina)
        {
            _stamina = 0;
            return true;
        }
        return false;
    }

    public float getStamina()
    {
        return _stamina;
    }

    public bool getStaminaCharged()
    {
        return _stamina >= _maxStamina;
    }

    private void setTextStamina()
    {
        float staminaFloor = Mathf.Floor(_stamina);
        _staminaText.SetText("Stamina: " + staminaFloor.ToString() + "/" + _maxStamina.ToString());
    }

    public float getMaxStamina()
    {
        return _maxStamina;
    }
}
