using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorController : MonoBehaviour {

    public bool shouldShowPressFKeyText;

    private DoubleSlidingDoorController doubleSlidingDoorController;
    private GameplayController gameplayController;

    // Start is called before the first frame update
    void Start() {
        gameplayController = GameObject.Find("GameplayController").GetComponent<GameplayController>();

        doubleSlidingDoorController = GetComponent<DoubleSlidingDoorController>();
        doubleSlidingDoorController.isLocked = true;
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter(Collider other) {
        if (!gameplayController.isPuzzleSolved) {
            shouldShowPressFKeyText = true;

            gameplayController.GetGameUIController().ShowPressFKeyText("Press F to show puzzle");
        }
    }

    void OnTriggerExit(Collider other) {
        if (!gameplayController.isPuzzleSolved) {
            shouldShowPressFKeyText = false;

            gameplayController.GetGameUIController().HidePressFKeyText();
        }
    }

    public void UnlockDoor() {
        doubleSlidingDoorController.isLocked = false;

        doubleSlidingDoorController.DoOpenDoor();
    }
}
