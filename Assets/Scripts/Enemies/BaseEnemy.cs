using UnityEngine;
using UnityEngine.UI;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Enemy Settings")] 
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected int scoreValue = 10;
    [SerializeField] protected GameObject deathEffectPrefab;

    [Header("UI")] 
    [SerializeField] protected Slider healthSlider;

    private float _currentHealth;
    protected Transform Player;

    private float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Max(value, 0f);
            if (healthSlider) healthSlider.value = _currentHealth;
            if (_currentHealth <= 0) Die();
        }
    }

    protected virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
        }
        
        CurrentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (Player)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        Vector2 direction = (Player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (healthSlider) healthSlider.value = CurrentHealth;
    }

    protected virtual void Die()
    {
        if (GameManager.Instance) GameManager.Instance.AddScore(scoreValue);

        if (deathEffectPrefab) Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        if (SpawnManager.Instance)
        {
            SpawnManager.Instance.SpawnPowerup(transform.position);
            SpawnManager.Instance.EnemyDestroyed();
        }
        

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        collision.GetComponent<PlayerController>().TakeDamage(10);
        Die();
    }
    
    protected float ApplyDamageMultiplier(float baseDamage)
    {
        return baseDamage * 0.5f;
    }
}