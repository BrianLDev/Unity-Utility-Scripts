/*
ECX UTILITY SCRIPTS
Camera 2D pan controller
Last updated: Jan 21, 2022
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple 2D camera panning controller.  Just add this as a component of your camera and move with WASD.
/// </summary>
public class Camera2DPanController : MonoBehaviour {
#region VARIABLES
    [SerializeField] private float panSpeed = 6f;
    [SerializeField] private float fastPanMultiplier = 2f;
    private Camera camera2D;
    private Vector2 inputDirection;
    private Vector3 targetPosition;
    private bool fastPanEnabled = false;
    #if ENABLE_LEGACY_INPUT_MANAGER
        private KeyCode upLegacy = KeyCode.W;
        private KeyCode leftLegacy = KeyCode.A;
        private KeyCode downLegacy = KeyCode.S;
        private KeyCode rightLegacy = KeyCode.D;
        private KeyCode fastPanLegacy = KeyCode.LeftShift;
    #endif
    #if ENABLE_INPUT_SYSTEM
        // TODO: CUSTOMIZE FOR NEW INPUT SYSTEM
    #endif
#endregion  // end variables region

#region METHODS - LIFECYCLE
    private void Awake() {
        camera2D = GetComponent<Camera>();
    }

    private void Update() {
        GetInput();
        MoveCamera();
    }
#endregion METHODS - LIFECYCLE

#region METHODS - CUSTOM
    private void GetInput() {
        inputDirection = Vector2.zero;
        // INPUT - Legacy Input system
        #if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(upLegacy))
                inputDirection += Vector2.up;
            if (Input.GetKey(downLegacy))
                inputDirection += Vector2.down;
            if (Input.GetKey(leftLegacy))
                inputDirection += Vector2.left;
            if (Input.GetKey(rightLegacy))
                inputDirection += Vector2.right;

            if (Input.GetKeyDown(fastPanLegacy))
                fastPanEnabled = true;
            else if (Input.GetKeyUp(fastPanLegacy))
                fastPanEnabled = false;
        #endif

        // INPUT - New Input system
        #if ENABLE_INPUT_SYSTEM
            // TODO: CUSTOMIZE FOR NEW INPUT SYSTEM
        #endif
    }

    private void MoveCamera() {
        float fp = fastPanEnabled ? fastPanMultiplier : 1;
        targetPosition.x = transform.position.x + (inputDirection.x * panSpeed * fp);
        targetPosition.y = transform.position.y + (inputDirection.y * panSpeed * fp);
        targetPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
#endregion METHODS - CUSTOM

}