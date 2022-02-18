using UnityEngine;

public abstract class State
{
    public abstract void EnterState(NPC npc);

    public abstract void Update(NPC npc);


}
