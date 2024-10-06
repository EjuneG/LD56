using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float targetSize = 5f; // Set this to the desired size when the slime grows

    public void ZoomOut(float newSize)
    {
        StartCoroutine(ZoomCamera(newSize));
    }
    private IEnumerator ZoomCamera(float newSize)
    {
        float startSize = Camera.main.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            Camera.main.orthographicSize = Mathf.Lerp(startSize, newSize, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.orthographicSize = newSize; // Ensure it sets to the final size
    }
}