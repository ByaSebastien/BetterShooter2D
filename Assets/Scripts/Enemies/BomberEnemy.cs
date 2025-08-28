using UnityEngine;

public class BomberEnemy : BaseEnemy
{
    [Header("Bomber Settings")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int explosionDamage = 50;
    
    protected override void Move()
    {
        base.Move();
        transform.position = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
    }

    protected override void Die()
    {
        CreateExplosion();
        base.Die();
    }
    
    private void CreateExplosion()
    {
        GameObject explosionObj = new ("Explosion")
        {
            transform =
            {
                position = transform.position
            }
        };

        CircleCollider2D explosionCollider = explosionObj.AddComponent<CircleCollider2D>();
        explosionCollider.isTrigger = true;
        explosionCollider.radius = explosionRadius;
        
        explosionObj.AddComponent<ExplosionTrigger>().Setup(explosionDamage);
        
        Destroy(explosionObj, 0.1f);
    }
}

public class ExplosionTrigger : MonoBehaviour
{
    private int _damage;
    
    public void Setup(int explosionDamage)
    {
        _damage = explosionDamage;
    }
    
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if (player) player.TakeDamage(_damage);
        }
        else if (hit.CompareTag("Enemy"))
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy && enemy.gameObject != transform.parent)
            {
                enemy.TakeDamage(_damage / 2f);
            }
        }
    }
}
