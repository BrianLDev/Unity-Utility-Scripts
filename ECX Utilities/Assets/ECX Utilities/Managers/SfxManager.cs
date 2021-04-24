using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;

public class SfxManager : SingletonMono<SfxManager> {
    // ADD SFX CLIPS HERE, THEN DRAG/DROP THEM IN THE UNITY EDITOR
    public AudioClip sound1;

    // ADD ANY PUBLIC METHODS HERE TO PLAY SPECIFIC SFX
    // OR ALTERNATIVELY JUST USE THE AUDIOMANAGER TO PLAY A CLIP AND LOAD IT USING THIS INSTANCE
}
