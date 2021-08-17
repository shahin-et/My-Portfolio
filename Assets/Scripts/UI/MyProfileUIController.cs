using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MyProfileUIController : MonoBehaviour {

    // is details dialog open
    public bool isOpen;

    private Text nameTitleText;
    private Text phoneText;
    private Text emailText;
    private Text linkedinText;
    private Text githubText;
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

        nameTitleText = transform.Find("BgImage/NameTitleText").GetComponent<Text>();
        phoneText = transform.Find("BgImage/PhoneButton/Text").GetComponent<Text>();
        emailText = transform.Find("BgImage/EmailButton/Text").GetComponent<Text>();
        linkedinText = transform.Find("BgImage/LinkedInButton/Text").GetComponent<Text>();
        githubText = transform.Find("BgImage/GithubButton/Text").GetComponent<Text>();

        isOpen = false;

        gameObject.SetActive(false);
    }

    // Show more details about the game
    public void ShowMyDetails() {
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

    // Callback for close button
    public void OnClickUIButton(string name) {
        AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

        if (name.Equals("close_button")) {
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

                    gameplayController.GetGameUIController().ShowPressBKeyText();
                });
            });
        } else if (name.Equals("phone_button")) {
            GUIUtility.systemCopyBuffer = phoneText.text.Replace("-", "");

            gameplayController.GetGameUIController().ShowToast("Phone Number Copied", 1);
        } else if (name.Equals("email_button")) {
            GUIUtility.systemCopyBuffer = emailText.text;

            gameplayController.GetGameUIController().ShowToast("Email Address Copied", 1);
        } else if (name.Equals("linkedin_button")) {
            GUIUtility.systemCopyBuffer = linkedinText.text;

            gameplayController.GetGameUIController().ShowToast("LinkedIn URL Copied", 1);
        } else if (name.Equals("github_button")) {
            GUIUtility.systemCopyBuffer = githubText.text;

            gameplayController.GetGameUIController().ShowToast("Github URL Copied", 1);
        } else if (name.Equals("download_resume_button")) {
            Application.OpenURL("https://esmaeeldoost.com/My_Resume.pdf");
        }
    }
}
