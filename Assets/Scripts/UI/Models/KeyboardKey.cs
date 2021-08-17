using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeyboardKey : MonoBehaviour {
    private Text keyText;
    private string keyValue;
    private int id;
    private GameObject go;
    private Button button;

    public void InitializeKey() {
        go = transform.GetChild(0).gameObject;

        keyText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        button = transform.GetChild(0).GetComponent<Button>();
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

    public GameObject GetGO() {
        return go;
    }

    public void ResetKey() {
        SetText("");
        SetID(-1);
    }

    public void SetScale(string type) {
        go.SetActive(true);

        if (type.Equals("in")) {
            transform.localScale = Vector3.zero;

            transform.DOScale(1.0f, 0.3f).OnComplete(() => {
                go.SetActive(true);
            });
        } else {
            transform.localScale = Vector3.one;

            transform.DOScale(0.0f, 0.3f).OnComplete(() => {
                go.SetActive(false);
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
