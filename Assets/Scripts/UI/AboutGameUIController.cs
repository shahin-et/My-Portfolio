using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AboutGameUIController : MonoBehaviour {

    // Is details dialog open
    public bool isOpen;

    // Language hash
    private JSONObject langHash;
    private Text gameTitleText;
    private Text gameDescriptionText;
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

        LoadLanguageTexts();

        gameTitleText = transform.Find("BgImage/GameTitleText").GetComponent<Text>();
        gameDescriptionText = transform.Find("BgImage/GameDescriptionText").GetComponent<Text>();

        gameTitleText.text = "";
        gameDescriptionText.text = "";

        isOpen = false;

        gameObject.SetActive(false);
    }

    // Show more details about the game
    public void ShowGameDetails(string gamePanelName) {
        isOpen = true;

        // Check which panel
        if (gamePanelName.Contains("Happy Soccer")) {
            gameTitleText.text = "Happy Soccer";
            gameDescriptionText.text = GetLanguageText("happy_soccer_description");
        } else if (gamePanelName.Contains("Magic Defence")) {
            gameTitleText.text = "Magic Defence";
            gameDescriptionText.text = GetLanguageText("magic_defence_description");
        } else if (gamePanelName.Contains("Superior Memory")) {
            gameTitleText.text = "Superior Memory";
            gameDescriptionText.text = GetLanguageText("superior_memory_description");
        } else if (gamePanelName.Contains("Joojoo Shoot")) {
            gameTitleText.text = "Joojoo Shoot";
            gameDescriptionText.text = GetLanguageText("joojoo_shoot_description");
        } else if (gamePanelName.Contains("Fighter Jet")) {
            gameTitleText.text = "Fighter Jet";
            gameDescriptionText.text = GetLanguageText("fighter_jet_description");
        } else if (gamePanelName.Contains("Khabar24")) {
            gameTitleText.text = "Khabar24";
            gameDescriptionText.text = GetLanguageText("khabar_24_description");
        } else if (gamePanelName.Contains("Shater")) {
            gameTitleText.text = "Shater";
            gameDescriptionText.text = GetLanguageText("shater_description");
        } else if (gamePanelName.Contains("Jazireh App")) {
            gameTitleText.text = "Jazireh App";
            gameDescriptionText.text = GetLanguageText("jazireh_description");
        } else if (gamePanelName.Contains("Marmoolak")) {
            gameTitleText.text = "Marmoolak";
            gameDescriptionText.text = GetLanguageText("marmoolak_description");
        } else if (gamePanelName.Contains("Virtual Exhibition")) {
            gameTitleText.text = "Virtual Exhibition";
            gameDescriptionText.text = GetLanguageText("virtual_exhibition_description");
        } else if (gamePanelName.Contains("Jungle Warriors")) {
            gameTitleText.text = "Jungle Warriors";
            gameDescriptionText.text = GetLanguageText("jungle_warriors_description");
        } else if (gamePanelName.Contains("Catch The Monster")) {
            gameTitleText.text = "Catch The Monster";
            gameDescriptionText.text = GetLanguageText("catch_the_monster_description");
        } else if (gamePanelName.Contains("Little Scientists")) {
            gameTitleText.text = "Little Scientists";
            gameDescriptionText.text = GetLanguageText("little_scientists_description");
        } else if (gamePanelName.Contains("Happy Runner")) {
            gameTitleText.text = "Happy Runner";
            gameDescriptionText.text = GetLanguageText("happy_runner_description");
        } else if (gamePanelName.Contains("Words War Tanks Battle")) {
            gameTitleText.text = "Words War: Tanks Battle";
            gameDescriptionText.text = GetLanguageText("words_war_tanks_battle_description");
        } else if (gamePanelName.Contains("Speedy Snake")) {
            gameTitleText.text = "Speedy Snake";
            gameDescriptionText.text = GetLanguageText("speedy_snake_description");
        } else if (gamePanelName.Contains("Pixelo")) {
            gameTitleText.text = "Pixelo";
            gameDescriptionText.text = GetLanguageText("pixelo_description");
        } else if (gamePanelName.Contains("Zanboori")) {
            gameTitleText.text = "Zanboori";
            gameDescriptionText.text = GetLanguageText("zanboori_description");
        } else if (gamePanelName.Contains("Mini Football")) {
            gameTitleText.text = "Mini Football";
            gameDescriptionText.text = GetLanguageText("mini_football_description");
        }

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

    // Load language texts
    private void LoadLanguageTexts() {
        string language = PlayerPrefs.GetString("lang", "en");
        TextAsset langFile;

        if (language.Equals("en")) {
            langFile = Resources.Load("LanguageFiles/Local_En") as TextAsset;
            langHash = new JSONObject(langFile.ToString());
        }
    }

    // Get langauge text
    private string GetLanguageText(string keyValue) {
        //Debug.Log(langHash[keyValue].str);
        return langHash[keyValue].str;
    }

    // Callback for close button
    public void OnClickCloseButton() {
        AudioController.Instance.PlayOneShotAudio(AudioController.Instance.buttonClip, 1.0f);

        // Complete previous effects if still playing
        transform.DOComplete();

        // Scale effect
        transform.DOScale(1.1f, 0.2f).OnComplete(() => {
            transform.DOScale(1.0f, 0.2f);
            transform.DOScale(0.0f, 0.3f).SetDelay(0.2f).OnComplete(() => {
                // Clear texts and close detials dialog
                gameTitleText.text = "";
                gameDescriptionText.text = "";

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
    }
}
