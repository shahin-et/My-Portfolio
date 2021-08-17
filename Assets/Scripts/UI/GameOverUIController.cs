using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FPSControllerLPFP;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour {

    private GameplayController gameplayController;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Initialize this gameObject
    public void Initialize(GameplayController gameplayController) {
        this.gameplayController = gameplayController;

        gameObject.SetActive(false);
    }

    public void HandleDeath() {
        // Disable fps controller for ui functionality
        gameplayController.SetActiveFPSController(false);

        if (gameplayController.isVideoClipEnded) {
            gameplayController.SetActionFPSArmsAndCanvas(false);
        }

        gameObject.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickAgainButton() {
        AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

        SceneManager.LoadScene("MyPortfolioScene", LoadSceneMode.Single);
    }
}
