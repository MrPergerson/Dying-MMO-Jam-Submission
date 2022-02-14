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

            Debug.Log("attack with mode - " + attackMode);

            //agent.Animator.SetBool("InCombat", true);
            agent.Animator.SetInteger("AttackMode", attackMode);

            for (int i = 0; i < combatAbilitySet.abilities.Count; i++)
            { 
                if (combatAbilitySet.abilities[i].VFX_Hit == null) continue;

                var vfxGameObject = Instantiate(combatAbilitySet.abilities[i].VFX_Hit, vfxContainer.transform);

                if(vfxGameObject.TryGetComponent<VisualEffect>(out VisualEffect visualEffectComponent))
                {
                    visualEffectComponent.Stop();
                    vfxPool.Add(i, visualEffectComponent);
                }
                
            }

            cPerformSimpleAttack = PerformSimpleAttack(GetComponent<Agent>(), attackMode);
            StartCoroutine(cPerformSimpleAttack);

        }

    }

    public void EndAttack()
    {
        Target = null;
        onAttackEnded?.Invoke();
        StopCoroutine(cPerformSimpleAttack);

        //agent.Animator.SetBool("InCombat", false);
        agent.Animator.SetInteger("AttackMode", -1);

        for (int i = 0; i < combatAbilitySet.abilities.Count; i++)
        {
            Destroy(vfxPool[i].gameObject, .2f);
        }

        vfxPool.Clear();
    }

    // need to pause the attack if doing movement within attackdistance 
    IEnumerator PerformSimpleAttack(Agent self, int attackMode)
    {
        while (Target.Health > 0 && Target != null)
        {

            if (GetDistance(this.transform.position, Target.transform.position) > AttackDistance)
            {
                // chase target??
                break;
            }

            // TODO: add support for different combat abilites
            //int index = 0;
            if (attackMode >= combatAbilitySet.abilities.Count)
            {
                Debug.LogError("Invalid attack mode");
                break;
            }
            CombatAbility simpleAttack = combatAbilitySet.abilities[attackMode];
            Target.TakeDamage(self, simpleAttack.Damage);

            //vfx
            var origin = transform.position + Vector3.up;
            var to = Target.transform.position + Vector3.up;
            vfxPool[0].gameObject.transform.position = (transform.forward * .6f) + Vector3.up + transform.position;
            vfxPool[0].Play();

            //audio
            if (playsAudio)
            {
                var hitAudio = simpleAttack.AUDIO_Hit;
                var hitMixer = simpleAttack.HitAudioMixer;
                audioPlayer.PlayAudioClip(hitAudio, hitMixer);
            }

            var targetPos = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            transform.LookAt(targetPos, Vector3.up);

            yield return new WaitForSeconds(simpleAttack.CoolDown);
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
}
