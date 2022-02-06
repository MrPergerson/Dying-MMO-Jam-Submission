using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AgentAttack : MonoBehaviour
{
    public AgentAudioData audioData;
    //public float simpleAttackCoolDown = 1;
    //[SerializeField] float _damage = 20;
    [SerializeField] float _attackDistance = 2;
    [SerializeField] CombatAbilitySet combatAbilitySet;
    [SerializeField] GameObject test_vfxAnchor;

    private List<CombatAbility> abilitiesInCoolDown;
    private Dictionary<int, VisualEffect> vfxPool;

    private GameObject vfxContainer;
    private AudioSource audio;

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
        vfxPool = new Dictionary<int, VisualEffect>();

        vfxContainer = new GameObject("vfxContainer");
        vfxContainer.transform.parent = this.transform;
        vfxContainer.transform.position = Vector3.zero;

        var audioContainer = new GameObject("AgentAttack AudioSource");
        audioContainer.transform.parent = this.transform;
        audioContainer.transform.position = Vector3.zero + Vector3.up;
        audio = audioContainer.AddComponent<AudioSource>();
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
           

            for(int i = 0; i < combatAbilitySet.abilities.Count; i++)
            { 
                if (combatAbilitySet.abilities[i].VFX_Hit == null) continue;

                var vfxGameObjec = Instantiate(combatAbilitySet.abilities[i].VFX_Hit, vfxContainer.transform);

                if(vfxGameObjec.TryGetComponent<VisualEffect>(out VisualEffect visualEffectComponent))
                {
                    visualEffectComponent.Stop();
                    vfxPool.Add(i, visualEffectComponent);
                }
                
            }

            cPerformSimpleAttack = PerformSimpleAttack(GetComponent<Agent>());
            StartCoroutine(cPerformSimpleAttack);

        }

    }

    public void EndAttack()
    {
        Target = null;
        onAttackEnded?.Invoke();
        StopCoroutine(cPerformSimpleAttack);

        for(int i = 0; i < combatAbilitySet.abilities.Count; i++)
        {
            Destroy(vfxPool[i].gameObject, .2f);
        }
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
            int index = 0;
            CombatAbility simpleAttack = combatAbilitySet.abilities[index];
            Target.TakeDamage(self, simpleAttack.Damage);

            //vfx
            var origin = transform.position + Vector3.up;
            var to = Target.transform.position + Vector3.up;
            vfxPool[index].gameObject.transform.position = (transform.forward * .6f) + Vector3.up + transform.position;
            vfxPool[index].Play();

            //audio
            if(audioData && audioData.AttackAudio.Count != 0 && index >= audioData.AttackAudio.Count)
            {
                audio.PlayOneShot(audioData.AttackAudio[index]);
            }

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
