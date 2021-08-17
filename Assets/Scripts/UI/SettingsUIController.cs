using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FPSControllerLPFP;
using UnityEngine.SceneManagement;

public class SettingsUIController : MonoBehaviour {

    // Is settings dialog open
    public bool isOpen;

    private GameplayController gameplayController;
    private Slider mouseSensitivitySlider;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Initialize this gameObject
    public void Initialize(GameplayController gameplayController) {
        this.gameplayController = gameplayController;

        mouseSensitivitySlider = transform.Find("BgImage/MouseSensitivitySlider").GetComponent<Slider>();

        mouseSensitivitySlider.value = GetMouseSensitivitySliderValue();
        ChangeMouseSensitivity(mouseSensitivitySlider.value);

        isOpen = false;

        gameObject.SetActive(false);
    }

    // Show more details about the game
    public void ShowSettings() {
        isOpen = true;

        // Disable fps controller for ui functionality
        gameplayController.SetActiveFPSController(false);

        if (gameplayController.isVideoClipEnded) {
            gameplayController.SetActionFPSArmsAndCanvas(false);
        }

        // Open details with effect
        transform.localScale = Vector3.zero;

        gameObject.SetActive(true);

        // Scale effect
        transform.DOScale(1.0f, 0.3f).OnComplete(() => {
            transform.DOScale(1.1f, 0.2f);
            transform.DOScale(1.0f, 0.2f).SetDelay(0.2f);
        });
    }

    public void ChangeMouseSensitivity(float value) {
        mouseSensitivitySlider.value = value;

        PlayerPrefs.SetFloat("mouse_sensitivity_value", value);

        gameplayController.SetMouseSensitivity(value);
    }

    public float GetMouseSensitivitySliderValue() {
        return PlayerPrefs.GetFloat("mouse_sensitivity_value", 0.5f);
    }

    public void OnMouseSensitivitySliderValueChanged() {
        ChangeMouseSensitivity(mouseSensitivitySlider.value);
    }

    public void OnClickConfirmButton() {
        AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

        // Complete previous effects if still playing
        transform.DOComplete();

        // Scale effect
        transform.DOScale(1.1f, 0.2f).OnComplete(() => {
            transform.DOScale(1.0f, 0.2f);
            transform.DOScale(0.0f, 0.3f).SetDelay(0.2f).OnComplete(() => {
                gameObject.SetActive(false);

                isOpen = false;

                // Hide mouse cursor
                Cursor.lockState = CursorLockMode.Locked;

                // Enable fps controller
                gameplayController.SetActiveFPSController(true);

                if (gameplayController.isVideoClipEnded) {
                    gameplayController.SetActionFPSArmsAndCanvas(true);
                }
            });
        });
    }
}
