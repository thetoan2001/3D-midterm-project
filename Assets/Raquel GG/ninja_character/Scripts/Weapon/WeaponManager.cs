using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Owner")]
    [Tooltip("Object with health, animator and stamina (character)")]
    [SerializeField] private GameObject _owner;

    [Header ("Weapons")]
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private Weapon _currentWeapon;

    private int _idx;

    void Start()
    {
        //Deactive the weapons that aren't current weapon
        _currentWeapon.gameObject.SetActive(true);
        int cont = 0;
        foreach (Weapon we in _weapons)
        {
            we.setOwner(_owner);    //add the owner to all weapons
            if (we != _currentWeapon)
                we.gameObject.SetActive(false);
            else
                _idx = cont;
            cont++;
        }
    }

    public void changeWeapon(int idx)
    {
        _currentWeapon.gameObject.SetActive(false);
        _currentWeapon = _weapons[idx];
        _currentWeapon.gameObject.SetActive(true);
    }

    public void changeWeapon()
    {
        _idx++;
        if (_idx >= _weapons.Length)
            _idx = 0;
        changeWeapon(_idx);
    }

    public Weapon getCurrentWeapon()
    {
        return _currentWeapon;
    }

}
