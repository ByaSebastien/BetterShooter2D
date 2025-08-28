using System;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected string weaponName = "Base Weapon";
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float range = 20f;
    
    [Header("Projectile Settings")]
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected float projectileSpeed = 10f;
    
    private float _nextFireTime;
    protected bool IsPlayerWeapon;
    protected Func<float, float> DamageModifier;

    public string WeaponName => weaponName;
    
    public void Initialize(bool isPlayerWeapon, Func<float, float> damageModifier = null)
    {
        IsPlayerWeapon = isPlayerWeapon;
        DamageModifier = damageModifier;
    }

    public void TryFire()
    {
        if (Time.time < _nextFireTime) return;
        
        Fire();
        _nextFireTime = Time.time + 1f / fireRate;
    }

    protected abstract void Fire();
}
