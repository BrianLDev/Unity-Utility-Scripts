using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcxUtilities;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("*** RUNNING TESTS ***");
        Debug.Log("GameManager name check 1:");
        Debug.Log(GameManager.Instance.name);
        Debug.Log("GameManager name check 2:");
        Debug.Log(GameManager.Instance.name);
        Debug.Log("AudioManager name check 1:");
        Debug.Log(AudioManager.Instance.name);
        Debug.Log("AudioManager name check 2:");
        Debug.Log(AudioManager.Instance.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
