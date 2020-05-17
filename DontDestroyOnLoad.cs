using UnityEngine;

namespace UnityUtilities {
    public class DontDestroyOnLoad : MonoBehaviour {
        void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
