using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 _moveDirection;
    private Vector2 _mouseDirection;
    private Rigidbody2D _rb;
    
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    
    [Header("Weapons")]
    [SerializeField] private List<BaseWeapon> weaponPrefabs = new();
    private BaseWeapon _currentWeapon;
    private int _weaponIndex = 0;

    [Header("Components")]
    [SerializeField] private Transform aimingCue;
    private float _aimOffset;
    
    private Camera _camera;

    private BaseWeapon CurrentWeapon
    {
        set
        {
            if(_currentWeapon) Destroy(_currentWeapon.gameObject);
            
            _currentWeapon = Instantiate(value, aimingCue.position, aimingCue.rotation, aimingCue);
            _currentWeapon.Initialize(true,ApplyDamageMultiplier);
            
            UiManager.Instance.UpdateWeaponText(_currentWeapon.WeaponName);
        }
    }

    private float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Max(value, 0f);
            if(UiManager.Instance) UiManager.Instance.UpdatePlayerHealth(_currentHealth, maxHealth);
            if(_currentHealth <= 0 && GameManager.Instance) GameManager.Instance.GameOver();
        }
    }

    private float _damageMultiplier = 1f;

    private float _currentMoveSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        
        _currentMoveSpeed = moveSpeed;
        
        _aimOffset = Vector3.Distance(aimingCue.position, transform.position);
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
        // if(UiManager.Instance) UiManager.Instance.UpdatePlayerHealth(_currentHealth, maxHealth);
        if(weaponPrefabs.Count > 0) CurrentWeapon = weaponPrefabs[_weaponIndex];
    }

    private void Update()
    {
        if(GameManager.Instance && GameManager.Instance.GameState != GameState.Playing) return;
        
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;
        _mouseDirection = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (_currentWeapon && Input.GetButton("Fire1"))
        {
            _currentWeapon.TryFire();
        } 
        if(Input.GetKeyDown(KeyCode.Q)) CycleWeapon(-1);
        if(Input.GetKeyDown(KeyCode.E)) CycleWeapon();
        if(Input.GetKeyDown(KeyCode.P) && GameManager.Instance) GameManager.Instance.PauseGame(); ;
    }
    
    private void FixedUpdate()
    {
        if(GameManager.Instance && GameManager.Instance.GameState != GameState.Playing) return;
        
        Move();
        Aim();
    }

    private void Move()
    {
        Vector2 nextPosition = (Vector2)transform.position + _moveDirection * _currentMoveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(nextPosition);
    }

    private void Aim()
    {
        Vector2 aimDirection = _mouseDirection - _rb.position;
        float angle = Mathf.Atan2(aimDirection.y,aimDirection.x) * Mathf.Rad2Deg - 90f;
        aimingCue.rotation = Quaternion.Euler(0f,0f,angle);
        aimingCue.localPosition = aimDirection.normalized * _aimOffset;
    }
    
    private void CycleWeapon(int index = 1)
    {
        _weaponIndex = (_weaponIndex + index + weaponPrefabs.Count) % weaponPrefabs.Count;
        CurrentWeapon = weaponPrefabs[_weaponIndex];
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0) GameManager.Instance.GameOver();
    }

    public void Heal()
    {
        CurrentHealth = maxHealth;
    }
    
    private float ApplyDamageMultiplier(float baseDamage)
    {
        return baseDamage * _damageMultiplier;
    }
    
    public void StartDoubleDamageEffect(float duration)
    {
        StartCoroutine(DoubleDamageRoutine(duration));
    }

    private IEnumerator DoubleDamageRoutine(float duration)
    {
        _damageMultiplier = 2;
        yield return new WaitForSeconds(duration);
        _damageMultiplier = 1;
    }

    public void StartSpeedBoostEffect(float duration)
    {
        if (_currentMoveSpeed > moveSpeed) return;
        StartCoroutine(SpeedBoostRoutine(duration));
    }
    
    private IEnumerator SpeedBoostRoutine(float duration)
    {
        _currentMoveSpeed *= 2;
        yield return new WaitForSeconds(duration);
        _currentMoveSpeed = moveSpeed;
    }
}
