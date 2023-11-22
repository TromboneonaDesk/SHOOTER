using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[Flags]
public enum AmmoType { 
    Base = 1, 
    Fire = 2, 
    Reverse = 4, 
    Moon = 8 
};
public abstract class WeaponBase : MonoBehaviour
{

    [Header("Weapon Base Stats")]
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float maxChargeTime;
    [SerializeField, Range(0,1)] protected float minChargePercent;
    [SerializeField] private bool automatic;


    private AmmoType curAmmo;
    private Coroutine _currentFireTimer;
    private bool _onCooldown;
    private float _currentChargeTime;

    private WaitForSeconds _cooldownWait;
    private WaitUntil _cooldownEnforce;

    private void Start()
    {
        _cooldownWait = new WaitForSeconds(attackCooldown);
        _cooldownEnforce = new WaitUntil(() => !_onCooldown);
    }

    public void StartShooting()
    {
        _currentFireTimer = StartCoroutine(ReFireTimer());
    }

    public void StopShooting()
    {
        StopCoroutine(_currentFireTimer);


        float percent = _currentChargeTime / maxChargeTime;
        if (percent != 0) TryAttack(percent);
    }

    
    public void SwitchAmmo(AmmoType ammo)
    {
        curAmmo = ammo;
    }

    private IEnumerator cooldownTimer()
    {
        _onCooldown = true;
        yield return _cooldownWait;
        _onCooldown = false;
    }

    private IEnumerator ReFireTimer()
    {
        //print("waiting for cooldonw");
        yield return _cooldownEnforce;
        //print("post cooldown");

        while(_currentChargeTime < maxChargeTime)
        {
            _currentChargeTime += Time.deltaTime;
            yield return null;
        }


        TryAttack(1);
        yield return null;
    }

    protected abstract void Attack(float percent,AmmoType ammo);

    private void TryAttack(float percent)
    {
        _currentChargeTime = 0;
        if(!CanAttack(percent)) return; //cancel if not valid attack

        Attack(percent, curAmmo);//atack based on percent

        StartCoroutine(cooldownTimer()); //do the cooldown
        if(automatic && percent >= 1)  _currentFireTimer = StartCoroutine(ReFireTimer()); //REFIRE ATTO
    }

    protected virtual bool CanAttack(float percent)
    {
        return !_onCooldown && percent >= minChargePercent;
    }
}
