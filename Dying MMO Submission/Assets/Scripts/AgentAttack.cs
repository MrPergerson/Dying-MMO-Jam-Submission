using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BUGs
 * Agents won't attack if they break off of an initial attack and then attack again
 * 
 * 
 * 
 */


[RequireComponent(typeof(Agent))]
public class AgentAttack : MonoBehaviour
{
    //public float simpleAttackCoolDown = 1;
    //[SerializeField] float _damage = 20;
    [SerializeField] float _attackDistance = 2;
    [SerializeField] CombatAbilitySet combatAbilitySet;

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



        cPerformSimpleAttack = PerformSimpleAttack(GetComponent<Agent>());

    }


    public void StartAttack(Agent target)
    {
        Target = target;
        StartCoroutine(cPerformSimpleAttack);

    }

    public void EndAttack()
    {
        Target = null;
        onAttackEnded?.Invoke();
        StopCoroutine(cPerformSimpleAttack);
    }


    IEnumerator PerformSimpleAttack(Agent self)
    {
        while(Target.Health > 0 && Target != null)
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

            yield return new WaitForSeconds(simpleAttack.AttackCoolDown);
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
