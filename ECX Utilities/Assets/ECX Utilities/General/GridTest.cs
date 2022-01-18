using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcxUtilities;

public class GridTest : MonoBehaviour {
    Gridd grid;
    int width = 10;
    int height = 10;
    float cellsize = 1f;

    private void Start() {
        grid = new Gridd(width, height, cellsize);
        grid.SetValue(2, 1, 13);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            grid.SetValue(Random.Range(0, width), Random.Range(0, height), Random.Range(1, 999));
        }
    }
}