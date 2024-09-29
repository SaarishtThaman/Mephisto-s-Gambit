using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundPlayer : MonoBehaviour {
    AudioSource audioSource;
    public AudioClip text,music1,slash,music2,victory,lost;

    public static SoundPlayer instance;

    void Awake() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene().buildIndex >= 2) {
            PlayLoop(music2);
        }
    }

    public void PlaySound(AudioClip sound) {
        audioSource.loop = false;
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void PlayDelayed(AudioClip sound) {
        audioSource.loop = false;
        audioSource.clip = text;
        audioSource.PlayDelayed(0.02f);
    }

    public void PlayLoop(AudioClip sound) {
        audioSource.clip = sound;
        audioSource.loop = true;
        audioSource.Play();
    }

}
