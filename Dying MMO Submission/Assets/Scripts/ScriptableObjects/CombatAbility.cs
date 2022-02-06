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
    [SerializeField] private GameObject _VFX_Hit;
    [SerializeField] private AudioClip _AUDIO_Hit;
    [SerializeField] private Sprite _IMG_Icon;


    private float _timeUntilCoolDownEnds = 0;


    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public float Damage { get { return _damage; } private set { _damage = value; } }
    public float CoolDown { get { return _coolDown; } private set { _coolDown = value; } }
    public float TimeUntilCoolDownEnds { get { return _timeUntilCoolDownEnds; } set { _timeUntilCoolDownEnds = value; } }
    public float AttackRange { get { return _attackRange; } }
    public GameObject VFX_Hit { get { return _VFX_Hit; } }
    public AudioClip AUDIO_Hit { get { return _AUDIO_Hit; } }
    public Sprite IMG_Icon { get { return _IMG_Icon; } }

}
