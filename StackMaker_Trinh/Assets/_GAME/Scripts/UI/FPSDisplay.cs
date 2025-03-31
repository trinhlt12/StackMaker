using UnityEngine;
public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int      w     = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect     rect  = new Rect(10, 10, w, h * 2 / 100);
        style.alignment        = TextAnchor.UpperLeft;
        style.fontSize         = h * 2 / 50;
        style.normal.textColor = Color.white;
        float  fps  = 1.0f / deltaTime;
        string text = string.Format("{0:0.} fps", fps);
        GUI.Label(rect, text, style);
    }
}