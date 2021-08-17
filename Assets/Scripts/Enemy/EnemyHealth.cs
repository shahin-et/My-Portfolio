using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base code downloaded from https://github.com/saivittalb/zomboid-survival
*/

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float hitPoints = 100f;
    [SerializeField] private AudioClip damagingClip;
    [SerializeField] private AudioClip deadClip;
    [SerializeField] private AudioSource audioSource;

    private bool isDead = false;

    public bool IsDead() {
        return isDead;
    }

    public void TakeDamage(float damage) {
        BroadcastMessage("OnDamageTaken");

        hitPoints -= damage;

        if (hitPoints <= 0) {
            Die();
        } else {
            audioSource.PlayOneShot(damagingClip, 1.0f);
        }
    }

    private void Die() {
        if (isDead) 
            return;

        audioSource.PlayOneShot(deadClip, 1.0f);

        audioSource.Stop();

        audioSource.clip = null;

        isDead = true;

        GetComponent<Animator>().SetTrigger("die");

        GetComponent<CapsuleCollider>().isTrigger = true;

        StartCoroutine(DieWithDelay());
    }

    private IEnumerator DieWithDelay() {
        yield return new WaitForSeconds(10.5f);

        Destroy(gameObject);
    }
}
