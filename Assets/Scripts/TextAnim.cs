using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextAnim : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;

    int i = -1;

    void Start() {
        EndCheck();
    }

    public void EndCheck() {
        StopAllCoroutines();
        i++;
        if (i <= stringArray.Length - 1) {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    private IEnumerator TextVisible() {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true) {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;
            SoundPlayer.instance.PlaySound(SoundPlayer.instance.text);

            if (visibleCount >= totalVisibleCharacters) {
                //i += 1;
                //Invoke("EndCheck", timeBtwnWords);
                break;
            }
            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars);
        }
    }
}