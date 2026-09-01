using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpawnMarkerManager : MonoBehaviour
{

    public Canvas uiCanvas;               // assign your UI Canvas
    public GameObject markerPrefab;       // UI prefab (e.g., small red dot)

    private List<MarkerFollow> activeMarkers = new List<MarkerFollow>();

    public void CreateMarker(Transform target)
    {
        if (target == null) return;

        GameObject marker = Instantiate(markerPrefab, uiCanvas.transform);
        MarkerFollow follow = marker.AddComponent<MarkerFollow>();

        follow.Initialize(target, uiCanvas);
        activeMarkers.Add(follow);
    }

    // Optional cleanup (e.g., if scene resets)
    public void ClearAllMarkers()
    {
        foreach (var m in activeMarkers)
        {
            if (m != null) Destroy(m.gameObject);
        }
        activeMarkers.Clear();
    }

    /* public Canvas uiCanvas;        // assign your canvas here
     public GameObject markerPrefab; // small red dot or icon prefab

     public void CreateMarker(Vector3 worldPos)
     {
         // Convert world position to screen coordinates
         Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

         // Ignore if the point is behind the camera
         if (screenPos.z < 0)
             return;

         // Create marker and attach to UI
         GameObject marker = Instantiate(markerPrefab, uiCanvas.transform);
         marker.GetComponent<RectTransform>().position = screenPos;

         // Optional: auto-remove marker after a few seconds
         Destroy(marker, 15f);
     }

     private IEnumerator FadeAndDestroy(GameObject marker, float duration)
     {
         Image img = marker.GetComponent<Image>();

         Color c = img.color;
         float t = 0f;

         while (t < duration)
         {
             t += Time.deltaTime;
             c.a = Mathf.Lerp(1f, 0f, t / duration);
             img.color = c;
             yield return null;
         }
         Destroy(marker);
     }*/
}
