using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMoveToTarget))]
public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected float _health = 100;
    [SerializeField] protected AgentAudioData audioData;
    protected AgentMoveToTarget move;

    public float Health { 
        get { return _health; } 
        protected set { 
            _health = value;
            onHealthChanged?.Invoke(Health);
            if (_health <= 0)
            {
                onDeath?.Invoke(this);
                Die();
            }
        } 
    }

    public delegate void HealthChanged(float health);
    public event HealthChanged onHealthChanged;

    public delegate void Death(Agent agent);
    public event Death onDeath;

    protected virtual void Awake()
    {
        move = GetComponent<AgentMoveToTarget>();
    }

    protected virtual void Start()
    {
        move.audioData = audioData;
    }

    public abstract void TakeDamage(Agent origin, float damage);

    protected abstract void Die();

}
