using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerTriggerController : MonoBehaviour {

    [SerializeField] private Transform videoPlayerTransform;
    private GameplayController gameplayController;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start() {
        gameplayController = GameObject.Find("GameplayController").GetComponent<GameplayController>();

        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter(Collider other) {
        gameplayController.FreezeFPSForWatchingVideo(transform.position, videoPlayerTransform.position);

        //boxCollider.enabled = false;
    }

    void OnTriggerExit(Collider other) {
        
    }
}
