using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player
    [SerializeField] private float detectionRadius = 5.0f; // Distance at which the monster detects the player
    private float stopDuration = 4f; // Time the monster stops after the collision
    private NavMeshAgent navMeshAgent;
    private bool isTriggered = false; // Flag to track if the monster is already triggered

    [SerializeField] private Healthbar healthbar; // Reference to the Healthbar script (via Canvas)

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (healthbar == null)
        {
            Debug.LogError("Healthbar reference is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (player != null && !isTriggered)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRadius)
            {
                navMeshAgent.SetDestination(player.transform.position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player && !isTriggered)
        {
            Debug.Log("Collision activated");

            if (healthbar != null)
            {
                healthbar.DamageFromMonster();
            }

            // Start the stop logic
            StartCoroutine(StopMonster());
        }
    }

    private IEnumerator StopMonster()
    {
        isTriggered = true; // Set the flag to true to prevent reactivation
        navMeshAgent.isStopped = true; // Stop the monster's movement
        navMeshAgent.velocity = Vector3.zero; // Zero out the velocity to avoid any unwanted movement

        navMeshAgent.enabled = false; // Disable the NavMeshAgent temporarily

        yield return new WaitForSeconds(stopDuration); // Wait for the stop duration

        navMeshAgent.enabled = true; // Re-enable the NavMeshAgent
        navMeshAgent.isStopped = false; // Resume movement
        navMeshAgent.velocity = Vector3.zero; // Ensure velocity is zero when resuming

        isTriggered = false; // Reset the flag
    }

}
