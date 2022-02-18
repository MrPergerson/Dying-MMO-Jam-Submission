using UnityEngine;

public class IdleState : State
{
    public override void EnterState(NPC npc)
    {
        npc.RemoveThreat();
    }

    public override void ExitState(NPC npc)
    {
        //
    }

    public override void Update(NPC npc)
    {
        //
    }
}
