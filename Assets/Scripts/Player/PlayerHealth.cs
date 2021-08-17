using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base code downloaded from https://github.com/saivittalb/zomboid-survival
*/

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private GameOverUIController gameOverUIController;
    [SerializeField] private float hitPoints = 100f;

    public void TakeDamage(float damage) {
        hitPoints -= damage;
        if (hitPoints <= 0) {
            gameOverUIController.HandleDeath();
        }
    }
}
