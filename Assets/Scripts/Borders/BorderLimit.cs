using UnityEngine;

public class BorderLimit : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private float _collisionTimer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _collisionTimer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _collisionTimer += Time.deltaTime;
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage * Time.deltaTime * _collisionTimer);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _collisionTimer = 0;
        }
    }
}
