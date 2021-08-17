using FPSControllerLPFP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameplayController : MonoBehaviour {

    public Transform fpsTransform;
    public bool isPuzzleSolved;
    public bool isVideoClipEnded;

    [SerializeField] private EnemyAI[] zombies;
    [SerializeField] private GameObject[] zombiePods;
    [SerializeField] private GameObject[] zombieBrokenPods;

    // Fps gameobjects
    private GameObject fpsArms;
    private FpsControllerLPFP fpsControllerLPFP;
    private HandgunScriptLPFP handgunScriptLPFP;
    private Camera fpsCamera;
    private Camera gunCamera;
    private GameObject fpsCanvas;
    // game UI controller
    private GameUIController gameUIController;
    private FinalDoorController finalDoorController;
    private VideoPlayerController videoPlayerController;
    private VideoPlayerTriggerController videoPlayerTriggerController;
    private Quaternion fpsCameraLastRot;
    private Quaternion gunCameraLastRot;

    // Start is called before the first frame update
    void Start() {
        fpsArms = fpsTransform.Find("Handgun_01_Arms/arms_handgun_01/arms").gameObject;
        fpsCanvas = fpsTransform.Find("Handgun_01_Arms/Player Canvas").gameObject;
        fpsControllerLPFP = fpsTransform.GetComponent<FpsControllerLPFP>();
        handgunScriptLPFP = fpsTransform.Find("Handgun_01_Arms/arms_handgun_01").GetComponent<HandgunScriptLPFP>();
        fpsCamera = fpsTransform.Find("Handgun_01_Arms/arms_handgun_01/Armature/camera/Main Camera").GetComponent<Camera>();
        gunCamera = fpsTransform.Find("Handgun_01_Arms/Gun Camera").GetComponent<Camera>();

        finalDoorController = GameObject.Find("Doors/Door_004").GetComponent<FinalDoorController>();

        videoPlayerController = GameObject.Find("Props/Screen_001").GetComponent<VideoPlayerController>();
        videoPlayerTriggerController = GameObject.Find("Props/Screen_001/VideoPlayerTrigger").GetComponent<VideoPlayerTriggerController>();

        gameUIController = GameObject.Find("Canvas").GetComponent<GameUIController>();
        gameUIController.Initialize(this);

        isPuzzleSolved = false;

        isVideoClipEnded = false;

        // Disable first person shooter in start
        SetActionFPSArmsAndCanvas(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Get FPS controller
    public FpsControllerLPFP GetFpsControllerLPFP() {
        return fpsControllerLPFP;
    }

    // Get game ui controller
    public GameUIController GetGameUIController() {
        return gameUIController;
    }

    public FinalDoorController GetFinalDoorController() {
        return finalDoorController;
    }

    public VideoPlayerController GetVideoPlayerController() {
        return videoPlayerController;
    }

    public void ShowPuzzleUI() {
        // Disable fps controller for ui functionality
        SetActiveFPSController(false);

        gameUIController.GetGuessTheWordUIController().ShowUI();
    }

    public void PuzzleSolved() {
        SetActiveFPSController(true);

        // Hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;

        isPuzzleSolved = true;

        finalDoorController.UnlockDoor();

        gameUIController.ShowPressBKeyText();
    }

    public void FreezeFPSForWatchingVideo(Vector3 triggerPos, Vector3 videoPlayerTransform) {
        // Disable fps controller
        SetActiveFPSController(false);

        fpsCameraLastRot = fpsCamera.transform.rotation;
        gunCameraLastRot = gunCamera.transform.rotation;

        fpsTransform.DOMove(triggerPos, 0.5f).OnComplete(() => {
            fpsCamera.transform.DOLookAt(videoPlayerTransform, 0.5f).OnComplete(() => {
                gameUIController.ShowPressEKeyText();

                videoPlayerController.PlayVideo();

                gameUIController.HidePressBKeyText();
            });
        });
    }


    // Video clip ended
    public void VideoClipEnded(bool isCanceled) {
        gameUIController.HidePressEKeyText();

        fpsCamera.transform.DORotateQuaternion(fpsCameraLastRot, 0.2f).OnComplete(() => {
            gunCamera.transform.DORotateQuaternion(gunCameraLastRot, 0.2f).OnComplete(() => {
                SetActiveFPSController(true);

                // Hide mouse cursor
                Cursor.lockState = CursorLockMode.Locked;
            });
        });

        if (!isCanceled) {
            isVideoClipEnded = true;

            // Enable first person shooter in start
            SetActionFPSArmsAndCanvas(true);

            for (int i = 0; i < zombies.Length; i++) {
                zombies[i].GetNavMeshAgent().enabled = true;

                zombies[i].PlayZombieAudio();

                zombiePods[i].SetActive(false);

                zombieBrokenPods[i].SetActive(true);
            }

            videoPlayerTriggerController.GetComponent<BoxCollider>().enabled = false;
        }

        gameUIController.ShowPressBKeyText();
    }

    public void SetActionFPSArmsAndCanvas(bool value) {
        if (value) {
            fpsArms.SetActive(true);
            fpsCanvas.SetActive(true);
            handgunScriptLPFP.enabled = true;
        } else {
            fpsArms.SetActive(false);
            fpsCanvas.SetActive(false);
            handgunScriptLPFP.enabled = false;
        }
    }

    public void SetActiveFPSController(bool value) {
        if (value) {
            fpsControllerLPFP.enabled = true;
        } else {
            fpsControllerLPFP.StopFPS();
            fpsControllerLPFP.enabled = false;
        }
    }

    public void SetMouseSensitivity(float value) {
        fpsControllerLPFP.SetMouseSensitivity(value);
    }
}
