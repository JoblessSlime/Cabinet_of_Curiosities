using UnityEngine;

public class UIToggleManager : MonoBehaviour
{
    public GameObject canvasA;  // Assign the first canvas
    public GameObject canvasB;  // Assign the second canvas
    public GameObject extraObject; // Assign the extra GameObject to deactivate
    public GameObject Bar; // Assign the extra GameObject to deactivate

    public void ToggleUI()
    {
        if (canvasA != null && canvasB != null)
        {
            bool isCanvasAActive = canvasA.activeSelf;

            // Toggle canvases
            canvasA.SetActive(!isCanvasAActive);
            Bar.SetActive(isCanvasAActive);
            canvasB.SetActive(isCanvasAActive);

            // Deactivate the extra object when switching to Canvas B
            if (extraObject != null)
            {
                extraObject.SetActive(!isCanvasAActive);
            }
        }
        else
        {
            Debug.LogWarning("Canvas references are missing in UIToggleManager.");
        }
    }
}