using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FPSControllerLPFP;

public class GameUIController : MonoBehaviour {

    [SerializeField] private LayerMask uiItemsLayer;

    private GameplayController gameplayController;
    private AboutGameUIController aboutGameUIController;
    private MyProfileUIController myProfileUIController;
    private Camera fpsCamera;
    private Text pressFKeyText;
    private Text pressEKeyText;
    private Text pressBKeyText;
    private GameOverUIController gameOverUIController;
    private DamageReceivedUIController damageReceivedUIController;
    private GuessTheWordUIController guessTheWordUIController;
    private SettingsUIController settingsUIController;
    private Text toastMessageText;

    // Start is called before the first frame update
    void Start() {
        
    }

    public void Initialize(GameplayController gameplayController) {
        DOTween.Init();

        this.gameplayController = gameplayController;

        aboutGameUIController = GameObject.Find("Canvas/AboutGameUI").GetComponent<AboutGameUIController>();
        aboutGameUIController.Initialize(gameplayController);

        myProfileUIController = GameObject.Find("Canvas/MyProfileUI").GetComponent<MyProfileUIController>();
        myProfileUIController.Initialize(gameplayController);

        fpsCamera = gameplayController.fpsTransform.Find("Handgun_01_Arms/arms_handgun_01/Armature/camera/Main Camera").GetComponent<Camera>();

        pressFKeyText = GameObject.Find("Canvas/PressFKeyText").GetComponent<Text>();
        pressFKeyText.gameObject.SetActive(false);

        pressEKeyText = GameObject.Find("Canvas/PressEKeyText").GetComponent<Text>();
        pressEKeyText.gameObject.SetActive(false);

        pressBKeyText = GameObject.Find("Canvas/PressBKeyText").GetComponent<Text>();
        pressBKeyText.gameObject.SetActive(false);

        gameOverUIController = GameObject.Find("Canvas/GameOverUI").GetComponent<GameOverUIController>();
        gameOverUIController.Initialize(gameplayController);

        damageReceivedUIController = GameObject.Find("Canvas/DamageReceivedUI").GetComponent<DamageReceivedUIController>();
        damageReceivedUIController.Initialize(this);

        guessTheWordUIController = GameObject.Find("Canvas/GuessTheWordUI").GetComponent<GuessTheWordUIController>();
        guessTheWordUIController.Initialize(gameplayController);

        settingsUIController = GameObject.Find("Canvas/SettingsUI").GetComponent<SettingsUIController>();
        settingsUIController.Initialize(gameplayController);

        toastMessageText = GameObject.Find("Canvas/ToastMessageText").GetComponent<Text>();
        toastMessageText.enabled = false;

        ShowPressBKeyText();
    }

    // Update is called once per frame
    void Update() {
        // Check if details dialog is open
        if (aboutGameUIController.isOpen || myProfileUIController.isOpen || guessTheWordUIController.isOpen || settingsUIController.isOpen) {
            // Hide it only once
            if (pressFKeyText.gameObject.activeInHierarchy)
                HidePressFKeyText();

            // Show mouse cursor
            Cursor.lockState = CursorLockMode.None;

            return;
        }

        // Check any game item is in front of FPS
        Ray uiItemRay = fpsCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit uiItemHit;

        // Cast ray only on game panel layer
        if (Physics.Raycast(uiItemRay, out uiItemHit, 10, uiItemsLayer)) {
            float playerDistance = Vector3.Distance(gameplayController.fpsTransform.position, uiItemHit.collider.transform.position);
            //Debug.Log("playerDistance: " + playerDistance);

            // Check player is close enough to panels
            if (playerDistance < 2) {
                var direction = (fpsCamera.transform.position - uiItemHit.collider.transform.position).normalized;

                if (Vector3.Dot(uiItemHit.collider.transform.up, direction) > 0.0f) {
                    // The y axis of panel points at the camera
                    //Debug.Log("Front Panel Name: " + uiItemHit.collider.gameObject.name);

                    // Show it only once
                    if (!pressFKeyText.gameObject.activeInHierarchy)
                        // Show press key UI
                        ShowPressFKeyText("Press F to show more details");
                }
            }
        } else {
            // Hide it only once
            if (pressFKeyText.gameObject.activeInHierarchy && (!gameplayController.GetFinalDoorController().shouldShowPressFKeyText || gameplayController.isPuzzleSolved))
                HidePressFKeyText();
        }

        // Check F key pressed
        if (Input.GetKeyDown(KeyCode.F)) {
            // Prevent null hit objects
            if (uiItemHit.collider != null && pressFKeyText.gameObject.activeInHierarchy) {
                //Debug.Log("Front Panel Name: " + uiItemHit.collider.gameObject.name);
                AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

                if (!uiItemHit.collider.gameObject.name.Equals("My Profile Picture")) {
                    aboutGameUIController.ShowGameDetails(uiItemHit.collider.gameObject.name);
                } else {
                    myProfileUIController.ShowMyDetails();
                }

                HidePressBKeyText();
            } else {
                if (gameplayController.GetFinalDoorController().shouldShowPressFKeyText && !gameplayController.isPuzzleSolved) {
                    AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

                    gameplayController.ShowPuzzleUI();

                    HidePressBKeyText();
                }
            }
        }

        // Check E key pressed
        if (Input.GetKeyDown(KeyCode.E)) {
            if (gameplayController.GetVideoPlayerController().IsPlaying()) {
                AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

                gameplayController.GetVideoPlayerController().StopVideo();
                gameplayController.VideoClipEnded(true);
            }
        }

        // Check B key pressed
        if (Input.GetKeyDown(KeyCode.B)) {
            if (!gameplayController.GetVideoPlayerController().IsPlaying()) {
                if (!aboutGameUIController.isOpen || !myProfileUIController.isOpen || !guessTheWordUIController.isOpen || !settingsUIController.isOpen) {
                    AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

                    settingsUIController.ShowSettings();
                }
            }
        }
    }

    // Show blinked press key text
    public void ShowPressFKeyText(string text) {
        pressFKeyText.text = text;

        pressFKeyText.gameObject.SetActive(true);

        pressFKeyText.DOColor(Color.white, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Hide blinked press key text for 
    public void HidePressFKeyText() {
        pressFKeyText.gameObject.SetActive(false);

        pressFKeyText.DOKill();
        pressFKeyText.DOColor(Color.black, 0.0f);
    }

    // Show blinked press key text
    public void ShowPressEKeyText() {
        pressEKeyText.gameObject.SetActive(true);

        pressEKeyText.DOColor(Color.black, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Hide blinked press key text for 
    public void HidePressEKeyText() {
        pressEKeyText.gameObject.SetActive(false);

        pressEKeyText.DOKill();
        pressEKeyText.DOColor(Color.white, 0.0f);
    }

    // Show press B key text
    public void ShowPressBKeyText() {
        pressBKeyText.gameObject.SetActive(true);
    }

    // Hide press B key text for 
    public void HidePressBKeyText() {
        pressBKeyText.gameObject.SetActive(false);
    }

    public GameOverUIController GetGameOverUIController() {
        return gameOverUIController;
    }

    public DamageReceivedUIController GetDamageReceivedUIController() {
        return damageReceivedUIController;
    }

    public GuessTheWordUIController GetGuessTheWordUIController() {
        return guessTheWordUIController;
    }

    #region 
    // Code from https://stackoverflow.com/questions/52590525/how-to-show-a-toast-message-in-unity-similar-to-one-in-android
    // Showing toast message
    public void ShowToast(string text, int duration) {
        StartCoroutine(ShowToastCoroutine(text, duration));
    }

    private IEnumerator ShowToastCoroutine(string text, int duration) {
        Color orginalColor = toastMessageText.color;

        toastMessageText.text = text;
        toastMessageText.enabled = true;

        //Fade in
        yield return FadeInAndOutText(toastMessageText, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration) {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return FadeInAndOutText(toastMessageText, false, 0.5f);

        toastMessageText.enabled = false;
        toastMessageText.color = orginalColor;
    }

    private IEnumerator FadeInAndOutText(Text targetText, bool fadeIn, float duration) {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn) {
            a = 0f;
            b = 1f;
        } else {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration) {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
    #endregion
}
