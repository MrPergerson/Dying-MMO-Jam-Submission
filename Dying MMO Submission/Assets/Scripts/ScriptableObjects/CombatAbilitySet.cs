using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatAbilitySet", menuName = "Data/CombatAbilitySet")]
public class CombatAbilitySet : ScriptableObject
{
    public List<CombatAbility> abilities;
}
