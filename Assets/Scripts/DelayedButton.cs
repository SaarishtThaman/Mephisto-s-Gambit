using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedButton : MonoBehaviour {

    public GameObject nextButton;

    void Start() {
        nextButton.SetActive(false);
        StartCoroutine(EnableButton());
    }

    IEnumerator EnableButton() {
        yield return new WaitForSeconds(3f);
        nextButton.SetActive(true);
    }

}