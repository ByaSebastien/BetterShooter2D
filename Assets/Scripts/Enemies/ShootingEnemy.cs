using UnityEngine;

public class ShootingEnemy : BaseEnemy
{
    [SerializeField] private Pistol pistol;
    [SerializeField] private float keepDistance = 5f;

    protected override void Start()
    {
        base.Start();
        if (pistol)
        {
            pistol = Instantiate(pistol,transform.position,transform.rotation,transform);
            pistol.Initialize(false,ApplyDamageMultiplier);
        }
    }
    
    protected override void Update()
    {
        if (Player)
        {
            Move();
            pistol.TryFire();
        }
    }

    protected override void Move()
    {
        base.Move();
        
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);
            
        if (distanceToPlayer < keepDistance)
        {
            Vector2 directionFromPlayer = (Vector2)transform.position - (Vector2)Player.position;
            transform.position = Vector2.MoveTowards(transform.position, 
                (Vector2)transform.position + directionFromPlayer.normalized, 
                moveSpeed * 0.5f * Time.deltaTime);
        }
        else if (distanceToPlayer > keepDistance + 1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
        }
    }
    
    
}
