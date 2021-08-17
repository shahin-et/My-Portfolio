using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FPSControllerLPFP;

public class DamageReceivedUIController : MonoBehaviour {

    private GameUIController gameUIController;
    [SerializeField] private float impactTime = 0.3f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Initialize this gameObject
    public void Initialize(GameUIController gameUIController) {
        this.gameUIController = gameUIController;

        gameObject.SetActive(false);
    }

    public void ShowDamageImpact() {
        gameObject.SetActive(true);

        StartCoroutine(ShowSplatter());
    }

    private IEnumerator ShowSplatter() {
        yield return new WaitForSeconds(impactTime);
        gameObject.SetActive(false);
    }
}
