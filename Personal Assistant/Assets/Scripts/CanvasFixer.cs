using UnityEngine;

public class CanvasFixer : MonoBehaviour
{
    [System.Serializable]
    public class CanvasElement
    {
        public RectTransform element;
        public float posY;
    }

    [SerializeField] private bool enableFixer = true;
    [SerializeField] private CanvasElement[] canvasElements;

    private void Awake()
    {
        if (enableFixer)
        {
            FixYPos();
        }
    }

    private void FixYPos()
    {
        foreach (CanvasElement canvasElement in canvasElements)
        {
            canvasElement.element.position = new Vector3(canvasElement.element.position.x, canvasElement.posY, 0);
        }
    }

}
