using UnityEngine;

public class Pistol : BaseWeapon
{
    protected override void Fire()
    {
        if (projectilePrefab)
        {
            float finalDamage = DamageModifier?.Invoke(damage) ?? damage;

            Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

            projectile.Initialize(finalDamage, range, projectileSpeed, IsPlayerWeapon);
        }
    }
}