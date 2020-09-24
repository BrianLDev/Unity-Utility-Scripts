/*
ECX UTILITY SCRIPTS
Audio Manager (Singleton)
Last updated: August 16, 2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;

namespace EcxUtilities {
    public class AudioManager : SingletonMono<AudioManager> {

        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private AudioSource audioSourceSFX;
        [SerializeField] private AudioSource audioSourceMusic;

        private void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe to OnSceneLoaded event to play music for that scene
        }
        
        public void PlayRandomClip(AudioClip[] clips) {
            if(clips.Length==0) {Debug.Log("empty clips");return;}
            PlayClip(clips[Random.Range(0,clips.Length)]);
        }
        
        public void PlayClip(AudioClip audioClip) {
            if(!audioClip) {
                Debug.LogError("Missing audioclip: " + audioClip);
            }
            else {
                audioSourceSFX.PlayOneShot(audioClip);
            }
        }

        public void PlayClipUninterrupted(AudioClip audioClip) {
            if(!audioClip) {
                Debug.LogError("Missing audioclip: " + audioClip);
            }
            else if (!audioSourceSFX.isPlaying) {
                audioSourceSFX.clip = audioClip;
                audioSourceSFX.Play();
            }
        }

        public void PlayMusic(AudioClip music) {
            if(music==null) {
                Debug.LogError("Missing music: " + music);
            }
            else {
                audioSourceMusic.clip = music;
                audioSourceMusic.loop = true;
                audioSourceMusic.Play();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // add code here to start playing a music track when a scene is loaded
        }
    }
}
