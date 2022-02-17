using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMoveToTarget))]
public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected float _health = 100;
    protected float _maxHealth;
    [SerializeField] protected float _healingRate = 10;
    protected AgentMoveToTarget move;
    protected Animator _animator;


    public delegate void HealthChanged(float health);
    public event HealthChanged onHealthChanged;

    public delegate void Death();
    public event Death onDeath;

    public delegate void Respawned();
    public event Respawned onRespawned;

    public bool _isInCombat = false;
    float countDownSinceLastDamage = 0.0f;

    public bool IsInCombat { get { return _isInCombat; } set { _isInCombat = value; } }

    public float Health { 
        get { return _health; } 
        protected set {
            if (_health > value)
            {
                IsInCombat = true;
            }
            _health = value;
            onHealthChanged?.Invoke(Health);
            if (_health <= 0)
            {
                Die();
            }
        } 
    }

    public Animator Animator
    {
        get { return _animator; }
    }

    protected virtual void Awake()
    {
        move = GetComponent<AgentMoveToTarget>();

        _animator = GetComponent<Animator>();
        _maxHealth = Health;
    }

    protected virtual void Start()
    {
    }

    void Update()
    {
        /*
        if (IsInCombat)
        {
            //isAttacked = false;
            countDownSinceLastDamage += Time.deltaTime;
        }
        else
            countDownSinceLastDamage = 0.0f;

        if (countDownSinceLastDamage > 3.0f)
        {
            IsInCombat = false;
            StartCoroutine(startAutoHeal());
            countDownSinceLastDamage = 0.0f;
        }
        */
    }

    public abstract void TakeDamage(Agent origin, float damage);

    public virtual void Die()
    {
        onDeath?.Invoke();
    }

    public virtual void Respawn()
    {
        onRespawned?.Invoke();
    }

    IEnumerator startAutoHeal()
    {
        while(Health<_maxHealth && !IsInCombat)
        {
            Health+= _healingRate;
            if(Health > _maxHealth)
                Health = _maxHealth;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
