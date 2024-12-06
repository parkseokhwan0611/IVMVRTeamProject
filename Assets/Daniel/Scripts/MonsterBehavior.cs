using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player
    [SerializeField] private float detectionRadius = 5.0f; // Distance at which the monster detects the player
    private float backwardDuration = 2.0f; // Time the monster moves backward
    private float backwardSpeed = 10.0f; // Speed of backward movement

    private NavMeshAgent navMeshAgent;
    private bool isMovingBackward = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null && !isMovingBackward)
        {
            // Calculate the distance between the monster and the player
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within detection radius
            if (distance <= detectionRadius)
            {
                // Set the NavMeshAgent's destination to the player's position
                navMeshAgent.SetDestination(player.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.gameObject == player && !isMovingBackward)
    {
        Debug.Log("Trigger actiavted");
        // Start moving backward only if not already in backward motion
        StartCoroutine(MoveBackward());
    }
}

private IEnumerator MoveBackward()
    {
    isMovingBackward = true;
    navMeshAgent.isStopped = true;

        // Calculate the backward direction (opposite of the player's direction)
        Vector3 backwardDirection = (transform.position - player.transform.position).normalized;

        float elapsedTime = 0f;

        while (elapsedTime < backwardDuration)
        {
            // Move the monster backward
            player.transform.position += backwardDirection * backwardSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Resume normal behavior
        isMovingBackward = false;
        navMeshAgent.isStopped = false;
    }
}
