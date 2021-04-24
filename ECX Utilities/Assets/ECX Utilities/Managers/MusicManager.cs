using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;

public class MusicManager : SingletonMono<MusicManager> {
    public AudioClip menuAndCredits;
    public AudioClip combatLoop;
    public AudioClip exploreLoop;
    public AudioClip bossMinotaur;
    public AudioClip bossVampire;
    public AudioClip bossWitch;
    public AudioClip deathStinger;
    public AudioClip beginBossBattle;
    public AudioClip victoryStinger;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe to OnSceneLoaded event to play music for that scene
    }

    public void PlayDeathStinger() {
        AudioManager.Instance.PlayMusic(deathStinger);
    }

    public void PlayVictoryStinger() {
        AudioManager.Instance.PlayMusic(victoryStinger);
    }

    public void PlayBeginBossBattle() {
        AudioManager.Instance.PlayMusic(beginBossBattle);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0)      // Main Menu
            AudioManager.Instance.PlayMusic(bossVampire);
        else if (scene.buildIndex == 1) // Minotaur Level
            AudioManager.Instance.PlayMusic(combatLoop);
        else if (scene.buildIndex == 2) // Minotaur Mansion
            AudioManager.Instance.PlayMusic(bossMinotaur);
        else if (scene.buildIndex == 3) // Credits
            AudioManager.Instance.PlayMusic(bossWitch);
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // unsubscribe to OnSceneLoaded event to play music for that scene
    }
}
