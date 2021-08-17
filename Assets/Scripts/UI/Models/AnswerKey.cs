using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerKey : MonoBehaviour {
    private Text keyText;
    private string keyValue;
    private int id;
    private int linkedKeyboardKeyID;
    private Button button;

    public void InitializeKey() {
        keyText = transform.GetChild(0).GetComponent<Text>();
        button = GetComponent<Button>();
    }

    public void SetText(string value) {
        keyValue = value;
        keyText.text = keyValue;
    }

    public Text GetText() {
        return keyText;
    }

    public string GetValue() {
        return keyValue;
    }

    public string GetName() {
        return gameObject.name;
    }

    public int GetID() {
        return id;
    }

    public void SetID(int id) {
        this.id = id;
    }

    public int GetLinkedKeyboardKeyID() {
        return linkedKeyboardKeyID;
    }

    public void SetLinkedKeyboardKeyID(int linkedKeyboardKeyID) {
        this.linkedKeyboardKeyID = linkedKeyboardKeyID;
    }

    public void ResetKey() {
        SetText("");
        SetID(-1);
        SetLinkedKeyboardKeyID(-1);
    }

    public void SetScale(string type) {
        gameObject.SetActive(true);

        if (type.Equals("in")) {
            transform.localScale = Vector3.zero;

            transform.DOScale(1.0f, 0.3f).OnComplete(() => {
                gameObject.SetActive(true);
            });
        } else {
            transform.localScale = Vector3.one;

            transform.DOScale(0.0f, 0.3f).OnComplete(() => {
                gameObject.SetActive(false);
            });
        }
    }

    public void SetInteractable(bool value) {
        button.interactable = value;

        if (value)
            IsClicked = false;
    }

    public bool IsClicked { get; set; }
}
