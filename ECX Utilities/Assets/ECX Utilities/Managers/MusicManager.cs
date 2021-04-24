using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;

public class MusicManager : SingletonMono<MusicManager> {
    // ADD MUSIC CLIPS HERE, THEN DRAG/DROP THEM IN THE UNITY EDITOR
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    public AudioClip gameOverMusic;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe to OnSceneLoaded event to play music for that scene
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0)      // Main Menu
            AudioManager.Instance.PlayMusic(mainMenuMusic);
        else if (scene.buildIndex == 1) // Game
            AudioManager.Instance.PlayMusic(gameMusic);
        else if (scene.buildIndex == 2) // Game Over
            AudioManager.Instance.PlayMusic(gameOverMusic);
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // unsubscribe to OnSceneLoaded event to play music for that scene
    }
}
