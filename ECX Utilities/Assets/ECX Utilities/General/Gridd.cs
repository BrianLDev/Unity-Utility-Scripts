/*
ECX UTILITY SCRIPTS
Gridd (uses double d's to avoid name conflict with UnityEngine.grid)
Last updated: Jan 13, 2022
*/

using UnityEngine;

namespace EcxUtilities {
    public class Gridd {    // (uses double d's to avoid name conflict with UnityEngine.grid)
        private int width;
        private int height;
        private float cellSize;
        private int[,] gridArray;

        // Constructor
        public Gridd(int width, int height, float cellSize) {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            gridArray = new int[width, height];

            for (int x=0; x<gridArray.GetLength(0); x++) {
                for (int y=0; y<gridArray.GetLength(1); y++) {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
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
        }
        
    }
}
