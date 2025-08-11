#if UNITY_EDITOR

using System;
using UnityEngine;

public class TileTreePositioner : MonoBehaviour
{
    [SerializeField]
    private TileRun[] tileRuns;

    [SerializeField] private float tileSize = 5f;
    
    [Serializable]
    class TileRun
    {
        public string name;
        public Axis axis = Axis.Z;
        public GameObject parentGameObject;
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
        if (tileRuns == null || tileRuns.Length == 0)
        {
            Debug.LogWarning("No tiles to position.");
            return;
        }

        foreach (var tileRun in tileRuns)
        {
            for (int i = 1; i < tileRun.parentGameObject.transform.childCount; i++)
            {
                Transform parentTile = tileRun.parentGameObject.transform.GetChild(i - 1);
                Vector3 vec = new Vector3(0, 0, tileSize);
                Quaternion rot = Quaternion.identity; 
                switch (tileRun.axis)
                {
                    case Axis.X:
                        vec = new Vector3(tileSize, 0, 0);
                        rot = Quaternion.Euler(0, 0, -parentTile.rotation.eulerAngles.x);
                        break;
                    case Axis.Z:
                        vec = new Vector3(0, 0, tileSize);
                        rot = Quaternion.Euler(parentTile.rotation.eulerAngles.x, 0, 0);
                        break;
                    case Axis.NegativeX:
                        vec = new Vector3(-tileSize, 0, 0);
                        rot = Quaternion.Euler(0, 0, parentTile.rotation.eulerAngles.x);
                        break;
                    case Axis.NegativeZ:
                        vec = new Vector3(0, 0, -tileSize);
                        rot = Quaternion.Euler(parentTile.rotation.eulerAngles.x, 0, 0);
                        break;
                }
                vec = rot * vec;
                vec += parentTile.position;
                
                tileRun.parentGameObject.transform.GetChild(i).position = vec;
            }
        }
        Debug.Log("Tiles positioned successfully.");
    }

}

#endif
