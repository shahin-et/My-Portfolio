using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController Instance;
    public AudioSource backgroundMusicAudioSource;
    public AudioSource clipsAudioSource;
    public AudioClip mainMenuBackgroundClip;
    public AudioClip gameplayBackgroundClip;
    public AudioClip buttonClip;
    public AudioClip endVideoClip;
    // Check Sound Activation
    public bool isSoundEnabled;
    // Check Music Activation
    public bool isMusicEnabled;
    // Play BG audio coroutine
    //public Coroutine playAudioBGCoroutine;

    void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayOneShotAudio(AudioClip clip, float soundScale) {
        if (isSoundEnabled) {
            if (clipsAudioSource != null)
                clipsAudioSource.PlayOneShot(clip, soundScale);
        }
    }

    public void StopOneShotAudio() {
        clipsAudioSource.clip = null;
        clipsAudioSource.Stop();
    }

    public void PlayBGAudio(AudioClip sound) {
        if (isMusicEnabled) {
            backgroundMusicAudioSource.clip = sound;
            backgroundMusicAudioSource.Play();
        } else {
            StopBGAudio();
        }
    }

    public void StopBGAudio() {
        backgroundMusicAudioSource.clip = null;
        backgroundMusicAudioSource.Stop();
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime) {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1) {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
