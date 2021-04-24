using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EcxUtilities;

public class SfxManager : SingletonMono<SfxManager> {
    public AudioClip[] playerFootsteps;
    public AudioClip playerSwordSwing;
    public AudioClip playerDeath;
    public AudioClip playerJump1;
    public AudioClip playerJump2;
    public AudioClip[] skeletonFootsteps;
    public AudioClip skeletonSwordHit1;
    public AudioClip skeletonSwordHit2;
    public AudioClip skeletonDeath;
    public AudioClip[] minotaurFootsteps;
    public AudioClip minotaurAxeSwing;
    public AudioClip minotaurMeteorSummon;
    public AudioClip minotaurMeteorImpact;
    public AudioClip minotaurDeath;

    public void PlayRandomPlayerFootstep() {
        int clipNum = Random.Range(0, playerFootsteps.Length-1);
        AudioManager.Instance.PlayClip(playerFootsteps[clipNum], AudioCategory.Sfx);
    }

    public void PlayRandomSkeletonFootstep() {
        int clipNum = Random.Range(0, skeletonFootsteps.Length-1);
        AudioManager.Instance.PlayClip(skeletonFootsteps[clipNum], AudioCategory.Sfx);
    }

    public void PlayRandomMinotaurFootstep() {
        int clipNum = Random.Range(0, minotaurFootsteps.Length-1);
        AudioManager.Instance.PlayClip(minotaurFootsteps[clipNum], AudioCategory.Sfx);
    }
}
