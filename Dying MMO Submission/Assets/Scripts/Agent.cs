using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMoveToTarget))]
public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected float _health = 100;
    [SerializeField] protected AgentAudioData _audioData;
    protected AgentMoveToTarget move;
    protected Animator _animator;


    public delegate void HealthChanged(float health);
    public event HealthChanged onHealthChanged;

    public delegate  void Death(Agent agent);
    public event Death onDeath;

    public delegate void Respawned(Agent agent);
    public event Respawned onRespawned;

    public float Health { 
        get { return _health; } 
        protected set { 
            _health = value;
            onHealthChanged?.Invoke(Health);
            if (_health <= 0)
            {
                Die();
            }
        } 
    }

    public AgentAudioData AudioData
    {
        get { return _audioData; }
    }

    public Animator Animator
    {
        get { return _animator; }
    }

    protected virtual void Awake()
    {
        move = GetComponent<AgentMoveToTarget>();
        move.audioData = AudioData;

        _animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
    }

    public abstract void TakeDamage(Agent origin, float damage);

    public virtual void Die()
    {
        onDeath?.Invoke(this);
    }

    public virtual void Respawn()
    {
        onRespawned?.Invoke(this);
    }
}
