using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Agent))]
public class AgentAttack : MonoBehaviour
{
    //public float simpleAttackCoolDown = 1;
    //[SerializeField] float _damage = 20;
    [Header("Settings")]
    [SerializeField] float _attackDistance = 2;
    [Header("Combat Abilities")]
    [SerializeField] CombatAbilitySet combatAbilitySet;
    [Header("Debug")]
    [SerializeField, ReadOnly] private Agent _target;
    [SerializeField] bool playsAudio = true;

    private List<CombatAbility> abilitiesInCoolDown;
    private List<int> abilitiesIndexInCoolDown;
    private Dictionary<int, VisualEffect> vfxPool;
    

    private AgentAudioPlayer audioPlayer;
    private GameObject vfxContainer;
    private Agent agent;

    private Agent Target { get { return _target; } set { _target = value; } }
    private List<Agent> targets;


    public float AttackDistance { get { return _attackDistance; } set { _attackDistance = value; } }

    private IEnumerator cPerformSimpleAttack;

    public delegate void AttackEnded();
    public event AttackEnded onAttackEnded;

    private void Awake()
    {
        agent = GetComponent<Agent>();

        audioPlayer = GetComponent<AgentAudioPlayer>();

        if (playsAudio && audioPlayer == null)
        {
            Debug.LogWarning(this + " is set to play audio but no AgentAudioComponent was found");
            playsAudio = false;
        }
    }

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

        for(int i=0;i< combatAbilitySet.abilities.Count; i++)
        {
            //Debug.Log(i+", "+ combatAbilitySet.abilities[i].TimeUntilCoolDownEnds);
            combatAbilitySet.abilities[i].TimeUntilCoolDownEnds = 0;
        }

        abilitiesInCoolDown = new List<CombatAbility>();
        abilitiesIndexInCoolDown=new List<int> { };
        targets = new List<Agent>();
        vfxPool = new Dictionary<int, VisualEffect>();

        vfxContainer = new GameObject("vfxContainer");
        vfxContainer.transform.parent = this.transform;
        vfxContainer.transform.position = Vector3.zero;



    }

    private void Update()
    {
        ProcessCoolDowns();
    }

    private void ProcessCoolDowns()
    {
        if(abilitiesInCoolDown.Count > 0)
        {
            //abilitiesInCoolDown.ForEach(ability => ability.TimeUntilCoolDownEnds -= Time.deltaTime);
            //abilitiesInCoolDown.RemoveAll(ability => ability.TimeUntilCoolDownEnds <= 0);

            for(int i = 0; i < abilitiesInCoolDown.Count; i++)
            {
                abilitiesInCoolDown[i].TimeUntilCoolDownEnds -= Time.deltaTime;
                if(abilitiesInCoolDown[i].TimeUntilCoolDownEnds <= 0)
                {
                    abilitiesInCoolDown.RemoveAt(i);
                    abilitiesIndexInCoolDown.RemoveAt((int)i);
                    Debug.Log("removing from abilitiesInCoolDown " + (i + 1));
                    i--;
                }
            }

        }
    }


    public void StartAttack(Agent target)
    {
        if (!target.Equals(Target))
        {
            Target = target;
            
            int attackMode = 0;
            Debug.Log("abilitiesInCoolDown.Count- " + abilitiesInCoolDown.Count);
            if (abilitiesInCoolDown.Count > 0)
            {
                attackMode = abilitiesIndexInCoolDown[0];
                abilitiesIndexInCoolDown.RemoveAt(0);
                abilitiesInCoolDown.RemoveAt(0);
            }

            Debug.Log("attack with mode - " + attackMode+", target-"+Target.gameObject.name);
            resetAnimatorParams();
            //agent.Animator.SetBool("InCombat", true);
            if(attackMode>=0)
                agent.Animator.SetBool("Ability1", true);
            if (attackMode == 1)
                agent.Animator.SetTrigger("Ability2");
            else if (attackMode == 2)
                agent.Animator.SetTrigger("Ability3");
            else if (attackMode == 3)
                agent.Animator.SetTrigger("Ability4");

            //if attack mode is 3, player needs to jump
            //based on combat ability canAttackMultipleEnemies, 
            targets.Clear();
            targets.Add(target);

            addNearbyEnemiesToList();

            //for (int i = 0; i < combatAbilitySet.abilities.Count; i++)
            //{ 
            //if (combatAbilitySet.abilities[i].VFX_Hit == null) continue;
            if (combatAbilitySet.abilities[attackMode].VFX_Hit != null) {
                var vfxGameObject = Instantiate(combatAbilitySet.abilities[attackMode].VFX_Hit, vfxContainer.transform);

                if(vfxGameObject.TryGetComponent<VisualEffect>(out VisualEffect visualEffectComponent))
                {
                    visualEffectComponent.Stop();
                    vfxPool.Add(0, visualEffectComponent);
                }
                
            }

            cPerformSimpleAttack = PerformSimpleAttack(GetComponent<Agent>(), attackMode);
            StartCoroutine(cPerformSimpleAttack);

        }

    }

    private void addNearbyEnemiesToList()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) < 5.0f)
            {
                if(targets[0]!= enemies[i].GetComponent<Agent>())
                    targets.Add(enemies[i].GetComponent<Agent>());
            }
        }
    }

    public void EndAttack()
    {
        Target = null;
        onAttackEnded?.Invoke();
        StopCoroutine(cPerformSimpleAttack);

        resetAnimatorParams();
        Debug.Log("EndAttack");
        //agent.Animator.SetBool("InCombat", false);
        //agent.Animator.SetInteger("AttackMode", -1);
        agent.Animator.SetBool("Ability1", false);

        /*for (int i = 0; i < combatAbilitySet.abilities.Count; i++)
        {
            Destroy(vfxPool[i].gameObject, .2f);
        }*/

        Destroy(vfxPool[0].gameObject);

        vfxPool.Clear();
    }

    // need to pause the attack if doing movement within attackdistance 
    IEnumerator PerformSimpleAttack(Agent self, int attackMode)
    {
        while (Target.Health > 0 && Target != null)
        {
            Debug.Log("[" + gameObject.name + "] starting attack");
            var targetPos = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            transform.LookAt(targetPos, Vector3.up);
            if (Vector3.Distance(this.transform.position, Target.transform.position) > AttackDistance)
            {
                // chase target??
                while (GetComponent<EnemyAIBrain>() != null && Vector3.Distance(transform.position, Target.transform.position)<2*AttackDistance)
                {
                    Debug.Log("[" + gameObject.name + "] chasing target");
                    GetComponent<AgentMoveToTarget>().SetDestination(Target.transform.position, 0);
                    yield return new WaitForSeconds(0.5f);
                }
                if (Vector3.Distance(transform.position, Target.transform.position) >= 2 * AttackDistance)
                {
                    Debug.Log("too far" + Vector3.Distance(this.transform.position, Target.transform.position));
                    break;
                }
            }

            // TODO: add support for different combat abilites
            //int index = 0;
            if (attackMode >= combatAbilitySet.abilities.Count)
            {
                Debug.LogError("Invalid attack mode");
                break;
            }
            CombatAbility simpleAttack = combatAbilitySet.abilities[attackMode];

            bool isTargetAlive = true;

            if (simpleAttack.CanHitMultipleEnemies)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null)
                    {
                        targets[i].TakeDamage(self, simpleAttack.Damage);
                        Debug.Log("[" + gameObject.name + "] hitting - " + i+", "+ targets[i].gameObject.name + ", its health - " + targets[i].Health);
                        if (targets[i].Health <= 0) isTargetAlive = false;
                    }
                }
            }
            else
            {
                Target.TakeDamage(self, simpleAttack.Damage);
                Debug.Log("[" + gameObject.name + "] hitting - " + Target.gameObject.name+", its health - "+ Target.Health);
                if (Target.Health <= 0) isTargetAlive = false;
            }

            //if enemy is dead, wait for attack animation to end and then stop attack
            if (!isTargetAlive)
            {
                Debug.Log("[" + gameObject.name + "] target dead. quitting");
                yield return new WaitForSeconds(1.0f);
                break;
            }
            Debug.Log("["+gameObject.name+"] playing vfx");
            //vfx
            var origin = transform.position + Vector3.up;
            var to = Target.transform.position + Vector3.up;
            vfxPool[0].gameObject.transform.position = (transform.forward * .6f) + Vector3.up + transform.position;
            vfxPool[0].Play();

            Debug.Log("[" + gameObject.name + "] playing audio");
            //audio
            if (playsAudio)
            {
                var hitAudio = simpleAttack.AUDIO_Hit;
                var hitMixer = simpleAttack.HitAudioMixer;
                audioPlayer.PlayAudioClip(hitAudio, hitMixer);
            }

            Debug.Log("[" + gameObject.name + "] waiting for cooldown");

            yield return new WaitForSeconds(simpleAttack.CoolDown);
            Debug.Log("[" + gameObject.name + "] cooldown done");
            if (abilitiesInCoolDown.Count > 0)
            {
                attackMode = abilitiesIndexInCoolDown[0];
                abilitiesIndexInCoolDown.RemoveAt(0);
                abilitiesInCoolDown.RemoveAt(0);
            }
        }

        EndAttack();
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
            abilitiesIndexInCoolDown.Add(index);
            Debug.Log("Adding to abilitiesInCoolDown " + (index + 1));
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

    public Vector3 GetDirection(Vector3 from, Vector3 to)
    {
        return (to - from) / (to - from).magnitude;
    }

    private void OnDrawGizmos()
    {
        if(Target)
        {
            Gizmos.DrawLine(transform.position + Vector3.up, Target.transform.position + Vector3.up);
        }
    }

    private void resetAnimatorParams()
    {
        //agent.Animator.SetBool("Ability1", false);
        //agent.Animator.SetBool("Ability2", false);
        //agent.Animator.SetBool("Ability3", false);
        //agent.Animator.SetBool("Ability4", false);
    }
}
