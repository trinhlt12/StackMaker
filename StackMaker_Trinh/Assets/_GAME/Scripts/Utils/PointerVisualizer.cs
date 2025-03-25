using UnityEngine;
using UnityEngine.InputSystem;

public class PointerVisualizer : MonoBehaviour
{
    [SerializeField] private RectTransform pointerImage;
    [SerializeField] private Canvas        canvas;

    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            ShowPointer(screenPos);
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            ShowPointer(screenPos);
        }
        else
        {
            pointerImage.gameObject.SetActive(false);
        }
    }

    private void ShowPointer(Vector2 screenPos)
    {
        pointerImage.gameObject.SetActive(true);
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos, canvas.worldCamera,
            out anchoredPos
        );
        pointerImage.anchoredPosition = anchoredPos;
    }
}