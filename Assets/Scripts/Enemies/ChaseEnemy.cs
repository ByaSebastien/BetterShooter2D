using UnityEngine;

public class ChaseEnemy : BaseEnemy
{
    protected override void Move()
    {
        base.Move();
        transform.position = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
    }
}
