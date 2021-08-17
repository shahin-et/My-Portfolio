using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FPSControllerLPFP;

public class GuessTheWordUIController : MonoBehaviour {

    // is details dialog open
    public bool isOpen;

    private GameplayController gameplayController;
    // Key audio clip
    [SerializeField] private AudioClip keyClip;
    // Clear audio clip
    [SerializeField] private AudioClip clearClip;
    // Brawl audio clip
    [SerializeField] private AudioClip brawlClip;
    // Hint audio clip
    [SerializeField] private AudioClip hintClip;
    // English chars
    private string[] engChars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    // Answer letters placement
    private Transform answerLettersTransform;
    // Answer keys array
    private AnswerKey[] answerKeys;
    // Keyboard letters placement
    private Transform keyboardLettersTransform;
    // Keyboard keys array
    private KeyboardKey[] keyboardKeys;
    // Generated keyboard letters
    private List<string> finalKeyboardLetters;
    // Target word for openning the door
    private string targetWord;
    // Current answer key index
    private int currentAnswerKeyIndex;
    // Current player entered word
    private string currentEnteredWord;
    // Current player entered letter IDs
    private List<int> currentEnteredLetterIDs;
    // Confirm button
    private Button confirmButton;
    // Confirm button image
    private Image confirmButtonImage;
    // Can confirm;
    private bool canConfirm;
    // Refresh keyboard button
    private Button refreshKeyboardButton;
    // Hint button
    private Button hintButton;
    // Hint button coroutine
    private Coroutine hintButtonCoroutine;
    // Hint Text
    private Text hintText;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Initialize this gameObject
    public void Initialize(GameplayController gameplayController) {
        isOpen = false;

        this.gameplayController = gameplayController;

        currentAnswerKeyIndex = 0;

        currentEnteredWord = "";

        canConfirm = false;

        currentEnteredLetterIDs = new List<int>();

        answerLettersTransform = transform.Find("AnswerLetterPanel").GetComponent<Transform>();

        answerKeys = new AnswerKey[answerLettersTransform.childCount];
        answerKeys = answerLettersTransform.GetComponentsInChildren<AnswerKey>();
        for (int i = 0; i < answerLettersTransform.childCount; i++) {
            answerKeys[i].InitializeKey();
            answerKeys[i].SetID(i);
        }

        keyboardLettersTransform = transform.Find("DownBar/Keyboard").GetComponent<Transform>();
        keyboardKeys = new KeyboardKey[keyboardLettersTransform.childCount];
        keyboardKeys = keyboardLettersTransform.GetComponentsInChildren<KeyboardKey>();
        for (int i = 0; i < keyboardLettersTransform.childCount; i++) {
            keyboardKeys[i].InitializeKey();
            keyboardKeys[i].SetText("");
        }

        confirmButton = transform.Find("DownBar/RightBar/ConfirmButton").GetComponent<Button>();
        confirmButtonImage = confirmButton.GetComponent<Image>();

        refreshKeyboardButton = transform.Find("DownBar/LeftBar/RefreshKeyboardLettersButton").GetComponent<Button>();

        hintButton = transform.Find("DownBar/LeftBar/HintButton").GetComponent<Button>();

        hintText = transform.Find("HintText").GetComponent<Text>();
        hintText.enabled = false;

        gameObject.SetActive(false);
    }

    // Generate keyboard letters
    private void GenerateInKeyboardWords() {
        finalKeyboardLetters = new List<string>();

        targetWord = "zombie";

        for (int i = 0; i < targetWord.Length; i++) {
            finalKeyboardLetters.Add(targetWord[i].ToString());
        }

        int otherLettersCount = keyboardKeys.Length - targetWord.Length;
        for (int i = 0; i < otherLettersCount; i++) {
            finalKeyboardLetters.Add(engChars[Random.Range(0, engChars.Length)]);
        }

        finalKeyboardLetters = ShuffleLetterArray(finalKeyboardLetters);

        for (int i = 0; i < keyboardKeys.Length; i++) {
            keyboardKeys[i].SetID(i);
            keyboardKeys[i].SetText(finalKeyboardLetters[i].ToUpper());
        }
    }

    // Scramble string list
    private List<string> ShuffleLetterArray(List<string> inputArray) {
        string tempLetter;
        for (int i = 0; i < inputArray.Count; i++) {
            int rnd = Random.Range(0, inputArray.Count);
            tempLetter = inputArray[rnd];
            inputArray[rnd] = inputArray[i];
            inputArray[i] = tempLetter;
        }

        return inputArray;
    }

    // Show guess the word UI
    public void ShowUI() {
        isOpen = true;

        currentAnswerKeyIndex = 0;

        currentEnteredWord = "";

        canConfirm = false;

        currentEnteredLetterIDs = new List<int>();

        hintText.enabled = false;

        for (int i = 0; i < keyboardKeys.Length; i++) {
            keyboardKeys[i].GetGO().SetActive(false);
        }

        for (int i = 0; i < answerKeys.Length; i++) {
            answerKeys[i].gameObject.SetActive(false);
        }

        GenerateInKeyboardWords();

        gameObject.SetActive(true);

        DisableHintButton();

        DisableConfirmButton();

        ScaleInKeyboardKeys();
    }

    // Clear all answer keys
    public void ClearAllAnswerKeys() {
        for (int i = 0; i < answerLettersTransform.childCount; i++) {
            if (answerKeys[i].gameObject.activeInHierarchy) {
                answerKeys[i].SetScale("out");
                keyboardKeys[answerKeys[i].GetLinkedKeyboardKeyID()].SetScale("in");
                answerKeys[i].SetID(i);
            }
        }

        currentAnswerKeyIndex = 0;
        currentEnteredWord = "";
        currentEnteredLetterIDs.Clear();
    }

    // Callback for keyboard keys
    public void OnClickKeyboardKey(int index) {
        AudioController.Instance.PlayOneShotAudio(keyClip, 1.0f);

        if (keyboardKeys[index].IsClicked)
            return;
        else
            keyboardKeys[index].IsClicked = true;

        keyboardKeys[index].SetScale("out");

        answerKeys[currentAnswerKeyIndex].SetScale("in");
        answerKeys[currentAnswerKeyIndex].SetLinkedKeyboardKeyID(index);
        answerKeys[currentAnswerKeyIndex].SetText(keyboardKeys[index].GetValue());
        
        currentEnteredWord += keyboardKeys[index].GetValue();
        currentEnteredLetterIDs.Add(index);

        currentAnswerKeyIndex++;

        if (currentEnteredWord.Length == targetWord.Length) {
            if (currentEnteredWord.ToLower().Equals(targetWord)) {
                EnableConfirmButton();
            }
        }
    }

    // Callback for keyboard keys
    public void OnClickAnswerKey(int index) {
        AudioController.Instance.PlayOneShotAudio(keyClip, 1.0f);

        if (answerKeys[index].IsClicked)
            return;
        else
            answerKeys[index].IsClicked = true;

        SetAllKeysDisinteractable();
        Invoke("SetAllKeysInteractable", 0.5f);

        if (index < (currentAnswerKeyIndex - 1)) {
            for (int i = index; i < currentAnswerKeyIndex; i++) {
                answerKeys[i].SetScale("out");
                keyboardKeys[answerKeys[i].GetLinkedKeyboardKeyID()].SetScale("in");
            }

            currentEnteredLetterIDs.RemoveRange(index, currentAnswerKeyIndex - index);
        } else if (index == (currentAnswerKeyIndex - 1)) {
            answerKeys[index].SetScale("out");
            keyboardKeys[answerKeys[index].GetLinkedKeyboardKeyID()].SetScale("in");

            currentEnteredLetterIDs.RemoveAt(index);
        }

        currentEnteredWord = currentEnteredWord.Remove(index);


        currentAnswerKeyIndex = index;

        if (currentEnteredWord.Length > 2) {
            if (currentEnteredWord.ToLower().Equals(targetWord)) {
                EnableConfirmButton();
            }
        }
    }

    // Callback for down bar UIs
    public void OnClickDownBarUI(string name) {
        if (name.Equals("clear_answer_button")) {
            AudioController.Instance.PlayOneShotAudio(clearClip, 1.0f);

            ClearAllAnswerKeys();
            DisableConfirmButton();

            SetAllKeysDisinteractable();
            Invoke("SetAllKeysInteractable", 0.5f);
        } else if (name.Equals("refresh_keyboard_button")) {
            AudioController.Instance.PlayOneShotAudio(keyClip, 1.0f);

            RefreshKeyboardKeys();
        } else if (name.Equals("confirm_button")) {
            if (canConfirm) {
                AudioController.Instance.PlayOneShotAudio(keyClip, 1.0f);

                isOpen = false;

                canConfirm = false;

                gameplayController.PuzzleSolved();

                gameObject.SetActive(false);
            }
        } else if (name.Equals("hint_button")) {
            AudioController.Instance.PlayOneShotAudio(hintClip, 1.0f);

            hintButton.interactable = false;

            hintText.enabled = true;
        }
    }

    // Make refresh keyboard interactable
    private IEnumerator EnableRefreshKeyboardButton() {
        yield return new WaitForSeconds(0.6f);
        refreshKeyboardButton.interactable = true;
    }

    // Make all keys of game interactable
    private void SetAllKeysInteractable() {
        for (int i = 0; i < keyboardKeys.Length; i++) {
            keyboardKeys[i].SetInteractable(true);
        }

        for (int i = 0; i < answerKeys.Length; i++) {
            answerKeys[i].SetInteractable(true);
        }
    }

    // Make all keys of game disnteractable
    private void SetAllKeysDisinteractable() {
        for (int i = 0; i < keyboardKeys.Length; i++) {
            keyboardKeys[i].SetInteractable(false);
        }

        for (int i = 0; i < answerKeys.Length; i++) {
            answerKeys[i].SetInteractable(false);
        }
    }

    private void ScaleInKeyboardKeys() {
        for (int i = 0; i < keyboardKeys.Length; i++) {
            keyboardKeys[i].SetScale("in");
        }
    }

    private void ScaleOutKeyboardKeys() {
        for (int i = 0; i < keyboardKeys.Length; i++) {
            keyboardKeys[i].SetScale("out");
        }
    }

    private void ScaleInAnswerKeys() {
        for (int i = 0; i < answerKeys.Length; i++) {
            answerKeys[i].SetScale("in");
        }
    }

    private void ScaleOutAnswerKeys() {
        for (int i = 0; i < answerKeys.Length; i++) {
            answerKeys[i].SetScale("out");
        }
    }

    // Disable confirm button
    private void DisableConfirmButton() {
        confirmButton.interactable = false;
        confirmButtonImage.color = Color.white;
        canConfirm = false;
    }

    // Enable confirm button
    private void EnableConfirmButton() {
        confirmButton.interactable = true;
        confirmButtonImage.color = Color.yellow;
        canConfirm = true;
    }

    // Refresh keyboard keys
    private void RefreshKeyboardKeys() {
        refreshKeyboardButton.interactable = false;
        DisableConfirmButton();

        ClearAllAnswerKeys();

        SetAllKeysDisinteractable();

        GenerateInKeyboardWords();

        SetAllKeysInteractable();

        StartCoroutine(EnableRefreshKeyboardButton());
    }

    // Disable hint button
    private void DisableHintButton() {
        if (hintButtonCoroutine != null)
            StopCoroutine(hintButtonCoroutine);

        hintButtonCoroutine = StartCoroutine(EnableHintButton(5.0f));

        hintButton.interactable = false;
    }

    // Enable hint button after some seconds
    private IEnumerator EnableHintButton(float delay) {
        yield return new WaitForSeconds(delay);

        hintButton.interactable = true;
    }
}
