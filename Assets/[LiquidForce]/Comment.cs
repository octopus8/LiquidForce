using UnityEngine;

namespace O8C
{
    public class Comment : MonoBehaviour
    {
        [TextArea] public string text = "";
        public int messageType = 1;

        [HideInInspector] public bool isFirstTimeDrawingEditor = true;
    }
}