using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class TriggerZone : MonoBehaviour
{
    BoxCollider boxCollider;
    [Title("Size")]
    [SerializeField] Vector3 size = new Vector3(5, 5, 5);

    public delegate void TriggerZoneEnter(GameObject gameObject);
    public delegate void TriggerZoneExit(GameObject gameObject);

    public event TriggerZoneEnter onTriggerZoneEnter;
    public event TriggerZoneExit onTriggerZoneExit;

    [Title("Events")]
    [SerializeField] private UnityEvent onTriggerEnter;
    [SerializeField] private UnityEvent onTriggerExit;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = size;
    }

    [Title("Update Collider Size", "Runtime only")]
    [Button("Update Collider")]
    private void UpdateColliderSize()
    {
        boxCollider.size = size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerZoneEnter?.Invoke(other.gameObject);
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerZoneExit?.Invoke(other.gameObject);
        onTriggerExit.Invoke();
    }
}
