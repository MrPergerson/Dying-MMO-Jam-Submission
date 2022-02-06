using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    public AgentAudioData audioData;
    //public float simpleAttackCoolDown = 1;
    //[SerializeField] float _damage = 20;
    [SerializeField] float _attackDistance = 2;
    [SerializeField] CombatAbilitySet combatAbilitySet;

    private List<CombatAbility> abilitiesInCoolDown;

    public Agent Target { set; private get; }
    //public float Damage { set { _damage = value; } get { return _damage; } }
    public float AttackDistance { get { return _attackDistance; } set { _attackDistance = value; } }

    private IEnumerator cPerformSimpleAttack;

    public delegate void AttackEnded();
    public event AttackEnded onAttackEnded;

    private void Start()
    {
        if(combatAbilitySet == null)
        {
            Debug.LogError(gameObject.name + " -> " + this.ToString() + " -> Start(): combatAbilitySet is null.");

            
        }
        else if(combatAbilitySet.abilities.Count <= 0)
        {
            Debug.LogError(gameObject.name + " -> " + this.ToString() + " -> Start(): combatAbilitySet has no combat abilites.");
        }

        abilitiesInCoolDown = new List<CombatAbility>();
    }

    private void Update()
    {
        ProcessCoolDowns();
    }

    private void ProcessCoolDowns()
    {
        if(abilitiesInCoolDown.Count > 0)
        {
            abilitiesInCoolDown.ForEach(ability => ability.TimeUntilCoolDownEnds -= Time.deltaTime);
            abilitiesInCoolDown.RemoveAll(ability => ability.TimeUntilCoolDownEnds <= 0);
        }
    }


    public void StartAttack(Agent target)
    {
        if (!target.Equals(Target))
        {
            Target = target;
            cPerformSimpleAttack = PerformSimpleAttack(GetComponent<Agent>());
            StartCoroutine(cPerformSimpleAttack);
        }

    }

    public void EndAttack()
    {
        Target = null;
        onAttackEnded?.Invoke();
        StopCoroutine(cPerformSimpleAttack);
    }

    // need to pause the attack if doing movement within attackdistance 
    IEnumerator PerformSimpleAttack(Agent self)
    {
        while (Target.Health > 0 && Target != null)
        {
            if(GetDistance(this.transform.position, Target.transform.position) > AttackDistance)
            {
                EndAttack();
                // chase target??
                break;
            }

            // TODO: add support for different combat abilites
            CombatAbility simpleAttack = combatAbilitySet.abilities[0];
            Target.TakeDamage(self, simpleAttack.Damage);

            var targetPos = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            transform.LookAt(targetPos, Vector3.up);

            yield return new WaitForSeconds(simpleAttack.CoolDown);
        }            
    }

    public void PerformCombat(int index)
    {
        if(index >= combatAbilitySet.abilities.Count)
        {
            Debug.LogError(gameObject.name + " -> " + this.ToString() + " -> PerformCombat(): Ability index out of range. " +
                "PerformCombat() is calling for an ability that is not in combatAbilitySet");
            return;
        }


        CombatAbility combatAbility = combatAbilitySet.abilities[index];

        if(combatAbility.TimeUntilCoolDownEnds <= 0)
        {
            combatAbility.TimeUntilCoolDownEnds = combatAbility.CoolDown;
            abilitiesInCoolDown.Add(combatAbility);

            // animation and sound effect
            Debug.Log("Performed combat ability " + (index + 1));
            
        }
        else
        {
            print("Combat ability " + (index + 1) + " is on cooldown until " + combatAbility.TimeUntilCoolDownEnds + " seconds.");
        }

    }

    // include other attacks in here?
    // could they be there own classes??

    public float GetDistance(Vector3 a, Vector3 b)
    {
        return (b - a).magnitude;
    }

    private void OnDrawGizmos()
    {
        if(Target)
        {
            Gizmos.DrawLine(transform.position + Vector3.up, Target.transform.position + Vector3.up);
        }
    }
}
