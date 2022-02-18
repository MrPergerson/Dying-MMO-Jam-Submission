using UnityEngine;

public class AttackState : State
{
    public override void EnterState(NPC npc)
    {
        Debug.Log("Entering combat with " + npc.name);
        npc.attackAbility.EnterCombat(npc.threat);
        npc.attackAbility.onAttackEnded += npc.RemoveThreat;
    }

    public override void Update(NPC npc)
    {
        if (GetDistance(npc.threat.transform.position, npc.transform.position) > npc.attackRange)
        {
            Debug.Log("Out of distance");
            npc.attackAbility.EndCombat();
            npc.SwitchState(npc.idleState);
        }
    }

    public float GetDistance(Vector3 a, Vector3 b)
    {
        return (b - a).magnitude;
    }

}
