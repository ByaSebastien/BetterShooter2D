using UnityEngine;

public class MachineGun : BaseWeapon
{
    [SerializeField] private float spreadFactor = 0.1f;
    
    protected override void Fire()
    {
        float randomSpread = Random.Range(-spreadFactor, spreadFactor);
        Quaternion spreadRotation = Quaternion.Euler(0f, 0f, randomSpread) * transform.rotation;
        
        float finalDamage = DamageModifier?.Invoke(damage) ?? damage;
        
        if (projectilePrefab)
        {
            Projectile projectile = Instantiate(projectilePrefab, transform.position, spreadRotation);
            projectile.Initialize(finalDamage, range, projectileSpeed, IsPlayerWeapon);
        }
    }
}
