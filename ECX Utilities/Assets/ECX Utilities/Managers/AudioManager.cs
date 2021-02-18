/*
ECX UTILITY SCRIPTS
Audio Manager (Singleton)
Last updated: January 7, 2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace EcxUtilities {
    public class AudioManager : SingletonMono<AudioManager> {
#pragma warning disable 0649 // ignore warnings when SerializeFields not assigned
        [SerializeField] private AudioSource audioSourceSFX;
        [SerializeField] private AudioSource audioSourceMusic;
        private AudioSource audioSourceSFX2;    // 2nd audioSource in case it's needed by PlayClipInterruptable()
#pragma warning restore 0649 // restore warnings when SerializeFields not assigned

        public static AudioClipList LoadAudioClipList(string assetName) {
            #if UNITY_EDITOR
                string[] guids = AssetDatabase.FindAssets(assetName);
                AudioClipList acl = (AudioClipList)AudioClipList.CreateInstance(typeof(AudioClipList));
                // grabbing the first guid. Make sure the provided name is an exact match.

                acl = (AudioClipList)AssetDatabase.LoadAssetAtPath(assetPath: AssetDatabase.GUIDToAssetPath(guids[0]), typeof(AudioClipList));   
                if(!acl) { Debug.LogError("Error: Couldn't find AudioClipList: " + assetName + ". Check to ensure file exists."); }
                return acl;
            #else
                // TODO: add loading code for outside of unity editor.  AssetDatabase only works in editor mode.
                return null;
            #endif
        }

        private void Awake() {
            if (!audioSourceSFX) {
                Debug.LogWarning("Audio Manager is missing an audio source for sfx.  Creating a new one as a child GameObject.");
                GameObject SfxGo = new GameObject("SFX");
                SfxGo.transform.parent = this.transform;
                audioSourceSFX = SfxGo.AddComponent(typeof(AudioSource)) as AudioSource;
            }
            if (!audioSourceMusic) {
                Debug.LogWarning("Audio Manager is missing an audio source for music.  Creating a new one as a child GameObject.");
                GameObject MusicGo = new GameObject("Music");
                MusicGo.transform.parent = this.transform;
                audioSourceMusic = MusicGo.AddComponent(typeof(AudioSource)) as AudioSource;
            }
            SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe to OnSceneLoaded event to play music for that scene
        }
        
        /// <summary>
        /// Easiest way to play a sound clip that won't be interrupted, but downside is you can't control the clip (pause, stop, etc);
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="pitch"></param>
        public void PlayClip(AudioClip audioClip, float pitch=1) {
            if(!audioClip)  {  Debug.LogError("Missing audioclip: " + audioClip);  }
            else if (pitch==1)  {  audioSourceSFX.PlayOneShot(audioClip);  }
            else {
                var tempAudioSrc = PlayClipTempAudioSource(audioClip);
                tempAudioSrc.pitch = pitch;
            }
        }

        /// <summary>
        /// Plays a solo sound clip that can be controlled through the AudioSource (paused, stopped, volume, pitch, etc).  
        /// Downside is it can be interrupted. Calling this function while a clip is playing will stop the clip and play a new one instead.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="pitch"></param>
        public void PlayClipSolo(AudioClip audioClip, float pitch=1) {
            audioSourceSFX.clip = audioClip;
            audioSourceSFX.pitch = pitch;
            audioSourceSFX.Play();
        }
        
        /// <summary>
        /// Creates a temp AudioSource to play a clip at a certain position, and returns the AudioSource to customize pitch and volume
        /// then finally destroys the temp audiosource after the clip is done playing.
        /// This function is similar to AudioSource.PlayClipAtPoint() but returns the AudioSource so you can change things like pitch and volume.
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public AudioSource PlayClipTempAudioSource(AudioClip clip) {
            GameObject tempGO = new GameObject("Temp Audio Source");
            tempGO.transform.parent = this.transform;
            AudioSource tempAudioSource = tempGO.AddComponent<AudioSource>();
            tempAudioSource.clip = clip;
            // add any other adjustments you may want here, or you can manipulate the audiosource after it's returned
            tempAudioSource.Play();
            Destroy(tempGO, clip.length);
            return tempAudioSource;
        }

        /// <summary>
        /// Plays a random clip from an array of clips, at a random pitch. Pitch inputs are optional.
        /// </summary>
        /// <param name="clips"></param>
        /// <param name="pitchMin"></param>
        /// <param name="pitchMax"></param>
        public void PlayRandomClip(AudioClip[] clips, float pitchMin=1, float pitchMax=1) {
            if(clips.Length==0) { 
                Debug.Log("Can't play: clips array is empty. Skipping.");  
                return; 
            }
            else {
                float pitch = Random.Range(pitchMin, pitchMax);
                PlayClip( clips[Random.Range(0,clips.Length)], pitch );
            }
        }

        public void PlayRandomClipSolo(AudioClip[] clips, float pitchMin=1, float pitchMax=1) {
            if(clips.Length==0) { 
                Debug.Log("Can't play: clips array is empty. Skipping.");  
                return; 
            }
            else {
                float pitch = Random.Range(pitchMin, pitchMax);
                PlayClipSolo( clips[Random.Range(0,clips.Length)], pitch );
            }
        }

        /// <summary>
        /// Plays a random clip from a List of clips, at a random pitch. Pitch inputs are optional.
        /// </summary>
        /// <param name="clips"></param>
        /// <param name="pitchMin"></param>
        /// <param name="pitchMax"></param>
        public void PlayRandomClip(List<AudioClip> clips, float pitchMin=1, float pitchMax=1) {
            if(clips.Count==0) { 
                Debug.Log("Can't play: clips array is empty. Skipping.");  
                return; 
            }
            else {
                float pitch = Random.Range(pitchMin, pitchMax);
                PlayClip( clips[Random.Range(0,clips.Count)], pitch );
            }
        }

        public void PlayRandomClipSolo(List<AudioClip> clips, float pitchMin=1, float pitchMax=1) {
            if(clips.Count==0) { 
                Debug.Log("Can't play: clips array is empty. Skipping.");  
                return; 
            }
            else {
                float pitch = Random.Range(pitchMin, pitchMax);
                PlayClipSolo( clips[Random.Range(0,clips.Count)], pitch );
            }
        }


        /// <summary>
        /// Loads a music track to the music AudioSource and plays it.
        /// </summary>
        /// <param name="music"></param>
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

        /// <summary>
        /// Allows you to play music tracks as soon as a scene is finished loading.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // add code here to start playing a music track when a scene is loaded
        }
    }
}
