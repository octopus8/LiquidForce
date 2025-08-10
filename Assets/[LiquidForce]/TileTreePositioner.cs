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
        public string name;
        public Axis axis = Axis.Z;
        public GameObject[] tiles;
    }
    
    [Serializable]
    enum Axis
    {
        X,
        Z,
        NegativeX,
        NegativeZ
    }


    /// <summary>
    /// Positions the tiles in a tree structure based on the specified axis and tile size.
    /// </summary>
    public void PositionTiles()
    {
        if (tiles == null || tiles.Length == 0)
        {
            Debug.LogWarning("No tiles to position.");
            return;
        }

        foreach (var tile in tiles)
        {
            if (tile.tiles == null || tile.tiles.Length == 0)
            {
                Debug.LogWarning("No tiles in this group to position.");
                continue;
            }

            for (int i = 1; i < tile.tiles.Length; i++)
            {
                Vector3 vec = new Vector3(0, 0, tileSize);
                Quaternion rot = Quaternion.identity; 
                switch (tile.axis)
                {
                    case Axis.X:
                        vec = new Vector3(tileSize, 0, 0);
                        rot = Quaternion.Euler(0, 0, tile.tiles[i - 1].transform.rotation.eulerAngles.x);
                        break;
                    case Axis.Z:
                        vec = new Vector3(0, 0, tileSize);
                        rot = Quaternion.Euler(tile.tiles[i - 1].transform.rotation.eulerAngles.x, 0, 0);
                        break;
                    case Axis.NegativeX:
                        vec = new Vector3(-tileSize, 0, 0);
                        rot = Quaternion.Euler(0, 0, tile.tiles[i - 1].transform.rotation.eulerAngles.x);
                        break;
                    case Axis.NegativeZ:
                        vec = new Vector3(0, 0, -tileSize);
                        rot = Quaternion.Euler(tile.tiles[i - 1].transform.rotation.eulerAngles.x, 0, 0);
                        break;
                }
                vec = rot * vec;
                vec += tile.tiles[i - 1].transform.position;
                
                tile.tiles[i].transform.position = vec;
            }
        }
        Debug.Log("Tiles positioned successfully.");
    }

}
