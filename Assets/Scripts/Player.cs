using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    
    [SerializeField] private WeaponBase[] arsenal;
    private AmmoType[] ammunition = new AmmoType[4] {AmmoType.Base,AmmoType.Fire,AmmoType.Reverse,AmmoType.Moon};
    
    [SerializeField] private WeaponBase curWeapon;
    [SerializeField] private AmmoType curAmmo = AmmoType.Base;
    private bool weaponTogle;

    private void Start()
    {
        curWeapon = arsenal[0];
        InputManager.Init(this);
        InputManager.EnableInGame();
    }

    public void Shoot()
    {
        //print("I shot: " + InputManager.GetCameraRay());
        weaponTogle = !weaponTogle;
        if (weaponTogle) curWeapon.StartShooting();
        else curWeapon.StopShooting();

    }
    public void SwitchWeapon(int wIndex)
    {
        curWeapon = arsenal[wIndex];
        curWeapon.SwitchAmmo(curAmmo);
    }

    public void SwitchAmmo(int aIndex)
    {
        AmmoType flag = ammunition[aIndex];
        if (curAmmo.HasFlag(flag))
        {
            curAmmo &= ~flag;
            print("removed flag: " + flag);
        }
        else
        {
            curAmmo |= flag;
            print("added flag: " + flag);
        }
        curWeapon.SwitchAmmo(curAmmo);
    }
}
