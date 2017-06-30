using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MarkerManager : MonoBehaviour {
	private static GameObject prefab;
	private static List<GameObject> markers;
	private static GameObject activeMarker;

    public delegate void OnMarkerCreateDelegate(Transform newMarkerTransform, Color newMarkerColor);
    public static event OnMarkerCreateDelegate OnMarkerCreate;

    public delegate void OnMarkerDestroyDelegate(Vector3 destroyedMarkerPosition);
    public static event OnMarkerDestroyDelegate OnMarkerDestroy;

    public delegate void OnMarkerClearDelegate();
    public static event OnMarkerClearDelegate OnMarkerClear;

    void Awake() {
        prefab = (GameObject) Resources.Load("Pin", typeof(GameObject));
		markers = new List<GameObject>();
	}

	public static void CreateMarker(Transform parent, Vector3 position, Quaternion rotation, Color color) {
		GameObject marker = GameObject.Instantiate(prefab, parent, true);

		marker.transform.position = position;
		marker.transform.rotation = rotation;
		Renderer rend = marker.transform.GetChild(0).GetComponent<Renderer>();
		rend.material.color = color;

		markers.Add(marker);
		SetMarkerActive(marker);
	    if (OnMarkerCreate != null) OnMarkerCreate(marker.transform, color);
	}

	public static void SetMarkerActive(GameObject marker) {
		Marker markerComponent = marker.GetComponent<Marker>();

		if (markerComponent == null || !markers.Contains(marker))
			return;

		activeMarker = marker;
	}

    public static void DestroyMarkerAt(int index)
    {
        DestroyMarker(markers[index]);
    }

    public static void DestroyMarker(GameObject marker) {
	    if (!markers.Remove(marker)) return;
	    if (activeMarker == marker) {
	        activeMarker = null;
	    }

        if (OnMarkerDestroy != null) OnMarkerDestroy(marker.transform.position);

        Destroy (marker);
	}

    public static void DestroyAllMarkers() {
        activeMarker = null;
        GameObject m;

        for (int i = markers.Count - 1; i >= 0; --i) {
            m = markers[i];
            markers.RemoveAt(i);
            Destroy(m);
        }

        if (OnMarkerClear != null) OnMarkerClear();
    }

    //because you can't access static methods via the inspector
    public void DestroyAll()
    {
        DestroyAllMarkers();
    }

    public static void SetAllInactive() {
		activeMarker = null;
	}
}
