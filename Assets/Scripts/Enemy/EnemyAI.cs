using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Base code downloaded from https://github.com/saivittalb/zomboid-survival
*/

public class EnemyAI : MonoBehaviour {

    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float turnSpeed = 5f;
    // The time in seconds  between each attack.
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    [SerializeField] private AudioClip breakGlassClip;
    [SerializeField] private AudioClip searchingClip;
    [SerializeField] private AudioClip attackingClip;

    private NavMeshAgent navMeshAgent;
    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;
    private EnemyHealth health;
    private EnemyAttack attack;
    private Transform target;
    private float attackTimer;
    private AudioSource audioSource;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        attack = GetComponent<EnemyAttack>();
        target = FindObjectOfType<PlayerHealth>().transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (!navMeshAgent.enabled)
            return;

        if (health.IsDead()) {
            enabled = false;
            navMeshAgent.enabled = false;

            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked) {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange) {
            isProvoked = true;
        }
    }

    public void OnDamageTaken() {
        isProvoked = true;
    }

    private void EngageTarget() {
        FaceTarget();
        if (distanceToTarget - 0.9f >= navMeshAgent.stoppingDistance) {
            ChaseTarget();
        }

        // Add the time since Update was last called to the timer.
        attackTimer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range   
        if (attackTimer >= timeBetweenAttacks && distanceToTarget - 0.9f <= navMeshAgent.stoppingDistance) {
            AttackTarget();
        }
    }

    private void ChaseTarget() {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");

        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget() {
        // Reset the timer.
        attackTimer = 0f;

        GetComponent<Animator>().SetBool("attack", true);

        StartCoroutine(ApplyAttackToPlayer());
    }

    private void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction.magnitude > Mathf.Epsilon) {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    private IEnumerator ApplyAttackToPlayer() {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(attackingClip, 1.0f);

        attack.AttackHitEvent();
    }

    public NavMeshAgent GetNavMeshAgent() {
        return navMeshAgent;
    }

    public void PlayZombieAudio() {
        audioSource.PlayOneShot(breakGlassClip, 1.0f);

        audioSource.clip = searchingClip;
        audioSource.Play();
    }
}
