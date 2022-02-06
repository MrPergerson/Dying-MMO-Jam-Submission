using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatAbility", menuName = "Data/CombatAbility")]
public class CombatAbility : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField, TextArea(1,10)] private string _description;
    [SerializeField] private float _damage;
    [SerializeField] private float _coolDown;
    [SerializeField] private bool _canHitMultipleEnemies;
    [SerializeField] private float _attackRange = 1;
    [SerializeField] private AudioClip _soundEffect;
    [SerializeField] private Sprite _image;


    private float _timeUntilCoolDownEnds = 0;



    public float Damage { get { return _damage; } private set { _damage = value; } }
    public float CoolDown { get { return _coolDown; } private set { _coolDown = value; } }
    public float TimeUntilCoolDownEnds { get { return _timeUntilCoolDownEnds; } set { _timeUntilCoolDownEnds = value; } }
}
