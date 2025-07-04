using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "Scriptable Objects/HandData")]
public class AppHandData : ScriptableObject
{
    public string title;
    public Sprite thumbnail;
    public GameObject lefHandPrefab;
    public GameObject rightHandPrefab;

}
