using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BasePowerUp : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 6f;

    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if(!_playerTransform) return;
        
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPowerUp(other.GetComponent<PlayerController>());
            Destroy(gameObject);
        }
    }
    
    protected abstract void ApplyPowerUp(PlayerController player); 
}
