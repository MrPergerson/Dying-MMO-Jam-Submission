using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Agent))]
public class AgentHealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    private void Awake()
    {
        if (healthBar == null) Debug.LogError(this + " needs a reference to a UI slider element");

        var agent = GetComponent<Agent>();
        healthBar.maxValue = agent.Health;
        healthBar.value = agent.Health;

        agent.onHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(float newHealth)
    {
        healthBar.value = newHealth;
    }
}
