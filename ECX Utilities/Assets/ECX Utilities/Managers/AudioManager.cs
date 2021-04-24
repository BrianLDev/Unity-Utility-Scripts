/*
ECX UTILITY SCRIPTS
Audio Manager (Singleton)
Last updated: March 10, 2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace EcxUtilities {
    public enum AudioCategory { Music, Sfx, UI}

    public class AudioManager : SingletonMono<AudioManager> {

        [SerializeField] private AudioSource audioSourceMusic;
        [SerializeField] private AudioSource audioSourceSfx;
        [SerializeField] private AudioSource audioSourceUI;
        private bool isMusicPaused = false;
        [SerializeField][Range(0f, 1f)]
        private float volumeMaster = 1f;
        public float VolumeMaster => volumeMaster;
        [SerializeField][Range(0f, 1f)]
        private float volumeMusic = 0.6f;
        public float VolumeMusic => volumeMusic;
        [SerializeField][Range(0f, 1f)]
        private float volumeSfx = 1f;
        public float VolumeSfx => volumeSfx;
        [SerializeField][Range(0f, 1f)]
        private float volumeUI = 1f;
        public float VolumeUI => volumeUI;


        private void Awake() {
            if (!audioSourceMusic)
                Debug.LogError("Error: Missing Audio Source for Music");
            if (!audioSourceSfx)
                Debug.LogError("Error: Missing Audio Source for Sfx");
            if (!audioSourceUI)
                Debug.LogError("Error: Missing Audio Source for UI");
            // TODO: auto-create audio sources as child game objects if they are not assigned in the Unity Editor
            SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe to OnSceneLoaded event to play music for that scene
        }

        private void Start() {
            audioSourceMusic.volume = volumeMusic * volumeMaster;
            audioSourceSfx.volume = volumeSfx * volumeMaster;
            audioSourceUI.volume = volumeUI * volumeMaster;
        }

        /// <summary>
        /// Fastest and simplest way to play a sound clip that won't be interrupted.  
        /// But note that it can't be controlled (paused, stopped, volume, pitch, etc) after it starts playing.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public void PlayClip(AudioClip audioClip, AudioCategory ac, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                if (audioSourceSfx.pitch==pitch || !audioSourceSfx.isPlaying) { 
                    audioSourceSfx.pitch = pitch;
                    audioSourceSfx.PlayOneShot(audioClip, adjVolume);
                }
                else
                    PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
            } 
            else {
                Debug.LogError("Missing audioclip: " + audioClip);
            }
        }

        /// <summary>
        /// Fastest and simplest way to play a sound clip AT A SPECIFIC POSITION that won't be interrupted.  
        /// But note that it can't be controlled (paused, stopped, volume, pitch, etc) after it starts playing.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="position"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        public void PlayClip(AudioClip audioClip, AudioCategory ac, Vector3 position, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                if (pitch==1) { 
                    AudioSource.PlayClipAtPoint(audioClip, position, adjVolume);
                }
                else
                    PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
            } 
            else {
                Debug.LogError("Missing audioclip: " + audioClip);
            }
        }

        /// <summary>
        /// Plays a sound clip that won't be interrupted. 
        /// Also returns the AudioSource so the sound clip can be controlled (paused, stopped, volume, pitch, etc).
        /// Slightly slower and uses more memory than PlayClip().
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public AudioSource PlayClipControllable(AudioClip audioClip, AudioCategory ac, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                if (!audioSourceSfx.isPlaying) {
                    audioSourceSfx.clip = audioClip;
                    audioSourceSfx.volume = adjVolume;
                    audioSourceSfx.pitch = pitch;
                    audioSourceSfx.Play();
                    return audioSourceSfx;
                }
                // Slightly slower option
                else {
                    AudioSource audioSourceTemp = PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
                    return audioSourceTemp;       
                }
            } else {
                Debug.LogError("Missing audioclip: " + audioClip);
                return null;
            }
        }

        /// <summary>
        /// Plays a sound clip AT A SPECIFIC POSITION that won't be interrupted. 
        /// Also returns the AudioSource so the sound clip can be controlled (paused, stopped, volume, pitch, etc).
        /// Slightly slower and uses more memory than PlayClip().
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="position"></param>
        /// <param name="volume">Volume prior to any user settings (master volume, sfx volume, etc).</param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public AudioSource PlayClipControllable(AudioClip audioClip, AudioCategory ac, Vector3 position, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                if (!audioSourceSfx.isPlaying) {
                    audioSourceSfx.clip = audioClip;
                    audioSourceSfx.volume = adjVolume;
                    audioSourceSfx.pitch = pitch;
                    audioSourceSfx.Play();
                    return audioSourceSfx;
                }
                // Slightly slower option
                else {
                    AudioSource audioSourceTemp = PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
                    return audioSourceTemp;           
                }
            } else {
                Debug.LogError("Missing audioclip: " + audioClip);
                return null;
            }
        }

        private AudioSource PlayClipNewAudioSource(AudioClip audioClip, AudioCategory ac, Transform parent, Vector3 position, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);    
                GameObject tempGO = new GameObject("Temp AudioSource");
                tempGO.transform.parent = parent;
                AudioSource audioSourceTemp = tempGO.AddComponent<AudioSource>();
                audioSourceTemp.clip = audioClip;
                audioSourceTemp.volume = adjVolume;
                audioSourceTemp.pitch = pitch;
                tempGO.transform.position = position;
                audioSourceTemp.Play();
                Destroy(tempGO, audioClip.length); // sets a timer to destroy the temp audioSource after the clip is done playing
                return audioSourceTemp;
            }
            else {
                Debug.LogError("Missing audioclip: " + audioClip);
                return null;
            }
        }

        /// <summary>
        /// Plays a random clip from an array of clips, at a random pitch. (volume and pitch inputs are optional)
        /// </summary>
        /// <param name="clips"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitchMin"></param>
        /// <param name="pitchMax"></param>
        public void PlayRandomClip(AudioClip[] clips, AudioCategory ac, float volume=1, float pitchMin=1, float pitchMax=1) {
            if(clips.Length!=0) { 
                float pitch = Random.Range(pitchMin, pitchMax);
                int clipIndex = Random.Range(0,clips.Length);
                PlayClip(clips[clipIndex], ac, volume, pitch);
            } else {
                Debug.Log("Can't play: clips array is empty. Skipping.");  
                return; 
            }
        }

        /// <summary>
        /// Plays a random clip from a List of clips, at a random pitch. Pitch inputs are optional.
        /// </summary>
        /// <param name="clips"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitchMin"></param>
        /// <param name="pitchMax"></param>
        public void PlayRandomClip(List<AudioClip> clips, AudioCategory ac, float volume=1, float pitchMin=1, float pitchMax=1) {
            if(clips.Count!=0) { 
                float pitch = Random.Range(pitchMin, pitchMax);
                int clipIndex = Random.Range(0,clips.Count);
                PlayClip(clips[clipIndex], ac, volume, pitch);
            } else {
                Debug.LogError("Can't play: clips array is empty. Skipping.");  
                return; 
            }
        }


        /// <summary>
        /// Loads a music track to the music AudioSource and plays it. (volume and pitch are optional)
        /// </summary>
        /// <param name="music"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        public void PlayMusic(AudioClip music, float volume=1, float pitch=1) {
            if(music!=null) {
                volume = CalcAdjustedVolume(AudioCategory.Music, volume);
                audioSourceMusic.clip = music;
                audioSourceMusic.loop = true;
                audioSourceMusic.Play();
            }
            else {
                Debug.LogError("Missing music: " + music);
            }
        }

        public void PauseMusic() {
            audioSourceMusic.Pause();
            isMusicPaused = true;
        }

        public void UnPauseMusic() {
            audioSourceMusic.UnPause();
            isMusicPaused = false;
        }

        public void StopMusic() {
            audioSourceMusic.Stop();
        }

        public bool IsMusicPlaying() {
            return (audioSourceMusic.isPlaying);
        }
        
        public bool IsMusicPaused() {
            return isMusicPaused;
        }



        public void ResetAdjustedVolumes() {
            audioSourceMusic.volume = volumeMaster * volumeMusic;
            audioSourceSfx.volume = volumeMaster * volumeSfx;
            audioSourceUI.volume = volumeMaster * volumeUI;
        }

        /// <summary>
        /// Calculates the adjusted volume after taking into account master volume level and audio type volume level.
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="rawVolume"></param>
        /// <returns></returns>
        private float CalcAdjustedVolume(AudioCategory ac, float rawVolume) {
            float adjustedVolume = 1f;
            if (ac==AudioCategory.Music)
                adjustedVolume = rawVolume * volumeMaster * volumeMusic;
            if (ac==AudioCategory.Sfx)
                adjustedVolume = rawVolume * volumeMaster * volumeSfx;
            if (ac==AudioCategory.UI)
                adjustedVolume = rawVolume * volumeMaster * volumeUI;
            return adjustedVolume;
        }


        /// <summary>
        /// Utility script to load in AudioClipLists. (Optional: not required for all audio setups)
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Plays a music track as soon as a scene is finished loading.
        /// use this format in any script (e.g. MusicManager) to play a music track any as soon as a scene is loaded
        /// Note: need to subscribe in Awake (SceneManager.sceneLoaded += OnSceneLoaded;) unsubscribe in OnDestroy (SceneManager.sceneLoaded -= OnSceneLoaded;)
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // ADD CODE HERE FOR PLAYING VARIOUS TRACKS FOR EACH SCENE
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;  // unsubscribe to OnSceneLoaded event to play music for that scene
        }

    }
}
