#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TileTreePositioner))]
public class TileTreePositionerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector for the component
        DrawDefaultInspector();

        TileTreePositioner tileTreePositioner = (TileTreePositioner)target; // Get a reference to your component

        // Create a button
        if (GUILayout.Button("Position tiles"))
        {
            tileTreePositioner.PositionTiles(); 
        }
    }}
#endif
