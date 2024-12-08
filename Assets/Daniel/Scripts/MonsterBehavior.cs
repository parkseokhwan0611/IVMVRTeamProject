using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private GameObject player; // Reference to the player
    [SerializeField] private float detectionRadius = 5.0f; // Distance at which the monster detects the player
    private float stopDuration = 4f; // Time the monster stops after the collision
    private NavMeshAgent navMeshAgent;
    private bool isTriggered = false; // Flag to track if the monster is already triggered

    [SerializeField] private Healthbar healthbar; // Reference to the Healthbar script (via Canvas)
    public float hp;
    public float maxHp;
    public Transform hudPos;
    public bool isAttacked;
    public bool isDead = false;

    void Awake() {
        animator = GetComponent<Animator>();
    }
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

            if (distance <= detectionRadius && !isDead && !isAttacked)
            {
                navMeshAgent.SetDestination(player.transform.position);
                animator.SetBool("isRun", true);
            }
            else if((distance > detectionRadius && !isDead) || isAttacked) {
                animator.SetBool("isRun", false);
                navMeshAgent.speed = 0;
            }
        }
        if(hp <= 0 && !isDead) {
            animator.SetTrigger("isDead");
            isDead = true;
            navMeshAgent.speed = 0;
            StartCoroutine(DeathCor());
        }
        else if(hp > 0) {
            lookAtPlayer();
        }
    }
    void lookAtPlayer() {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        if(direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
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

    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Smoke" && !isAttacked) {
            hp -= 2.5f;
            isAttacked = true;
            navMeshAgent.speed = 0;
            StartCoroutine(AttackedCor());
        }
    }

    IEnumerator AttackedCor() {
        yield return new WaitForSeconds(0.1f);
        if(!isDead) {
            navMeshAgent.speed = 2;
        }
        isAttacked = false;
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
    IEnumerator DeathCor() {
        yield return new WaitForSeconds(0.833f);
        Destroy(gameObject);
    }
}
