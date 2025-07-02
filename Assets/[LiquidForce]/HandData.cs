using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "Scriptable Objects/HandData")]
public class HandData : ScriptableObject
{
    public string title;
    public Texture2D thumbnail;
    public  GameObject lefHandPrefab;
    public GameObject righHandPrefab;

}
