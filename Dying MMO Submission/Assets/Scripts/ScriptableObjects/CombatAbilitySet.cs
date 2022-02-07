using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatAbilitySet", menuName = "Data/CombatAbilitySet")]
public class CombatAbilitySet : ScriptableObject
{
    // could this class be iterable instead containing a list? 


    public List<CombatAbility> abilities;
}
