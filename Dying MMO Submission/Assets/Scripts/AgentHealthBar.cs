using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Agent))]
public class AgentHealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    Canvas canvas;
    Camera camera;

    private void Awake()
    {
        if (healthBar == null) Debug.LogError(this + " needs a reference to a UI slider element");

        canvas = GetComponentInChildren<Canvas>();
        camera = Camera.main;
        canvas.worldCamera = camera;

        var agent = GetComponent<Agent>();
        healthBar.maxValue = agent.Health;
        healthBar.value = agent.Health;

        agent.onHealthChanged += UpdateHealthBar;
    }

    public void Update()
    {
        // rotates canvas to camera, but it doesn't look good
        if (camera == null)
            camera = Camera.main;

        canvas.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }


    private void UpdateHealthBar(float newHealth)
    {
        healthBar.value = newHealth;
    }
}
