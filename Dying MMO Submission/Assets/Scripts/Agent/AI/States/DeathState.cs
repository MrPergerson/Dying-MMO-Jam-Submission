using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public override void EnterState(NPC npc)
    {
        npc.isAlive = false;
    }

    public override void ExitState(NPC npc)
    {
        npc.isAlive = true;
    }

    public override void Update(NPC npc)
    {
    }
}
