using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base code downloaded from https://github.com/saivittalb/zomboid-survival
*/

public class EnemyAttack : MonoBehaviour {

    [SerializeField] private DamageReceivedUIController damageReceivedUIController;
    private PlayerHealth target;
    [SerializeField] private float damage = 40f;

    void Start() {
        target = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent() {
        if (target == null) 
            return;

        target.TakeDamage(damage);
        damageReceivedUIController.ShowDamageImpact();
    }

}
