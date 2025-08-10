using System;
using UnityEngine;

public class TileTreePositioner : MonoBehaviour
{
    [SerializeField]
    private TreeTile[] tiles;

    [SerializeField] private float tileSize = 5f;
    
    [Serializable]
    class TreeTile
    {
        public GameObject[] tiles;
        public GameObject parent;
        public Axis axis;
    }
    
    [Serializable]
    enum Axis
    {
        X,
        Y,
        Z
    }


    /// <summary>
    /// Goes through all the tiles and positions them appropriately based on their axis and parent.
    /// This method is intended to be called from the editor to position tiles in the scene.
    /// It does not perform any runtime logic and is purely for editor use.
    /// </summary>
    public void PositionTiles()
    {
        foreach (var tile in tiles)
        {
            if (tile.parent == null || tile.tiles == null || tile.tiles.Length == 0)
            {
                Debug.LogWarning("Tile parent or tiles are not set up correctly.");
                continue;
            }

            Vector3 parentPosition = tile.parent.transform.position;

            for (int i = 0; i < tile.tiles.Length; i++)
            {
                GameObject currentTile = tile.tiles[i];
                if (currentTile == null) continue;

                Vector3 positionOffset = Vector3.zero;

                switch (tile.axis)
                {
                    case Axis.X:
                        positionOffset = new Vector3(i * tileSize, 0, 0);
                        break;
                    case Axis.Y:
                        positionOffset = new Vector3(0, i * tileSize, 0);
                        break;
                    case Axis.Z:
                        positionOffset = new Vector3(0, 0, i * tileSize);
                        break;
                }

                currentTile.transform.position = parentPosition + positionOffset;
            }
        }
        Debug.Log("Tiles positioned successfully.");
    }
}
