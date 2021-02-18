using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcxUtilities {

    [CreateAssetMenu(fileName = "AudioClips-X", menuName = "ECX Utilities/AudioClipList", order = 1)]
    public class AudioClipList : ScriptableObject {
        public List<AudioClip> clips;
    }

}