using UnityEngine;

public class Shotgun : BaseWeapon
{
    
    [SerializeField] private int pelletCount = 5;
    [SerializeField] private float spreadAngle = 30f;
    
    protected override void Fire()
    {
        float angleStep = spreadAngle / (pelletCount - 1);
        float startAngle = -spreadAngle / 2f;
        
        for (int i = 0; i < pelletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion pelletRotation = Quaternion.Euler(0f, 0f, angle) * transform.rotation;
            
            float finalDamage = DamageModifier?.Invoke(damage) ?? damage;
            
            if (projectilePrefab)
            {
                Projectile projectile = Instantiate(projectilePrefab, transform.position, pelletRotation);
                projectile.Initialize(finalDamage, range, projectileSpeed, IsPlayerWeapon);
            }
        }
        
    }
}
