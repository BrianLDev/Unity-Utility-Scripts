/*
ECX UTILITY SCRIPTS
Gridd (uses double d's to avoid name conflict with UnityEngine.grid)
Last updated: Jan 21, 2022
*/

using UnityEngine;
using TMPro;

namespace EcxUtilities {
    public class Gridd {    // (uses double d's to avoid name conflict with UnityEngine.grid)
        private static Transform textContainerTfm;
        private int width;
        private int height;
        private float cellSize;
        private int[,] gridArray;
        private TextMeshPro[,] gridTextArray;   // to display values stored in gridArray

        // Constructor
        public Gridd(int width, int height, float cellSize = 1f, bool displayValues = true, bool displayGridLines = true) {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            gridArray = new int[width, height];
            // Debug text
            gridTextArray = new TextMeshPro[width, height];
            if (!textContainerTfm) {
                GameObject textContainerGO = new GameObject("Text Container");
                textContainerTfm = textContainerGO.transform;
            }
            for (int x=0; x<gridArray.GetLength(0); x++) {
                for (int y=0; y<gridArray.GetLength(1); y++) {
                    gridTextArray[x,y] = Tmp.CreateWorldTmp(gridArray[x,y].ToString(), textContainerTfm, GetWorldPosition(x,y) + new Vector3(cellSize/2, cellSize/2), (int)Mathf.Round(5*cellSize), Color.white, TextAlignmentOptions.Center, 5000);
                    gridTextArray[x,y].gameObject.SetActive(displayValues);
                }
            }
            if (displayGridLines == true)
                DisplayGridLines();
        }

        private (int,int) GetGridPosition(Vector3 worldPosition) {
            int x = Mathf.FloorToInt(worldPosition.x / cellSize);
            int y = Mathf.FloorToInt(worldPosition.y / cellSize);
            return (x, y);
        }

        private Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * cellSize;
        }

        public void SetValue(int x, int y, int value) {
            gridArray[x, y] = value;
            gridTextArray[x,y].text = value.ToString();
        }

        public void SetValue(Vector3 worldPosition, int value) {
            int x, y;
            (x, y) = GetGridPosition(worldPosition);
            SetValue(x, y, value);
            Debug.Log("Set (" + x + ", " + y + ") to " + value);
        }

        public void DisplayValues() {
            Vector3 centeringOffset = new Vector3(cellSize/2, cellSize/2);
            for (int x=0; x<gridArray.GetLength(0); x++) {
                for (int y=0; y<gridArray.GetLength(1); y++) {
                    gridTextArray[x,y].transform.position = GetWorldPosition(x, y) + centeringOffset;
                    gridTextArray[x,y].text = gridArray[x,y].ToString();
                    gridTextArray[x,y].gameObject.SetActive(true);
                }
            }
        }

        public void HideValues() {
            for (int x=0; x<gridArray.GetLength(0); x++) {
                for (int y=0; y<gridArray.GetLength(1); y++) {
                    gridTextArray[x,y].gameObject.SetActive(false);
                }
            }
        }

        public void DisplayGridLines() {
            for (int x=0; x<gridArray.GetLength(0); x++) {
                for (int y=0; y<gridArray.GetLength(1); y++) {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }
}
