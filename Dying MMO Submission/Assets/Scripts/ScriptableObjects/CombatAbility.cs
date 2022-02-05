using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatAbility", menuName = "Data/CombatAbility")]
public class CombatAbility : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackCoolDown;
    public float Damage { get { return _damage; } private set { _damage = value; } }
    public float AttackCoolDown { get { return _attackCoolDown; } private set { _attackCoolDown = value; } }
}
