using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private float _range;
    private bool _isPlayerProjectile;
    private Vector3 _direction;
    
    private Rigidbody2D _rb;

    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float damage, float range, float speed, bool isPlayerProjectile)
    {
        _damage = damage;
        _range = range;
        _speed = speed;
        _isPlayerProjectile = isPlayerProjectile;
        Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
        _direction = (Vector2)(adjustedRotation * Vector2.right);
    }

    private void Start()
    {
        Destroy(gameObject,_range);
    }

    private void FixedUpdate()
    {
        _direction.z = 0;
        Vector2 nextPosition = transform.position + _direction.normalized * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(nextPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isPlayerProjectile && other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().TakeDamage(_damage);
            Destroy(gameObject);
        } else if (!_isPlayerProjectile && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
