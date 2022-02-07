using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMoveToTarget))]
public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected float _health = 100;
    [SerializeField] protected AgentAudioData _audioData;
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

    public AgentAudioData AudioData
    {
        get { return _audioData; }
    }

    public delegate void HealthChanged(float health);
    public event HealthChanged onHealthChanged;

    public delegate void Death(Agent agent);
    public event Death onDeath;

    protected virtual void Awake()
    {
        move = GetComponent<AgentMoveToTarget>();
        move.audioData = AudioData;
    }

    protected virtual void Start()
    {
    }

    public abstract void TakeDamage(Agent origin, float damage);

    protected abstract void Die();

}
