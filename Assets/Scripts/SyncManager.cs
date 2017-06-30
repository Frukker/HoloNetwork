using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class SyncManager : NetworkBehaviour
{
    //Model transform properties
    [SyncVar] Vector3 sPosition;
    [SyncVar] Quaternion sRotation;
    [SyncVar] Vector3 sScale;

    //Paint properties
    public struct SerializableBrush
    {
        public Vector3 position;
        public Vector3 scale;
        public Color color;
    }

    public class SyncBrushList : SyncListStruct<SerializableBrush> {}
    public SyncBrushList Brushes = new SyncBrushList();

    private TexturePainterLight clientPainter;

    //Marker properties
    public struct SerializableMarker
    {
        public Vector3 position;
        public Quaternion rotation;
        public Color color;
    }

    public class SyncMarkerList : SyncListStruct<SerializableMarker> { }
    public SyncMarkerList Markers = new SyncMarkerList();

    //Methods
    void Start()
    {
        if (isServer)
        {
            MarkerManager.OnMarkerCreate += AddMarker;
            MarkerManager.OnMarkerDestroy += DestroyMarker;
            MarkerManager.OnMarkerClear += ClearMarkers;
        }
        else
        {
            Brushes.Callback = BrushListChanged;
            Markers.Callback = MarkerListChanged;
            clientPainter = GameObject.FindObjectOfType<TexturePainterLight>();
        }
    }

	void Update () {
	    if (isServer)
	    {
	        sPosition = transform.parent.localPosition;
	        sRotation = transform.parent.localRotation;
	        sScale = transform.parent.localScale;
	    }
	    else
	    {
            transform.parent.localPosition = sPosition;
            transform.parent.localRotation = sRotation;
            transform.parent.localScale = sScale;
        }
    }

    //Paint Methods
    void AddBrush(Transform newBrushTransform, Color newBrushColor)
    {
        SerializableBrush brush;
        brush.position = newBrushTransform.localPosition;
        brush.scale = newBrushTransform.localScale;
        brush.color = newBrushColor;

        Brushes.Add(brush);
    }

    void ClearBrushes()
    {
        Brushes.Clear();
    }

    void BrushListChanged(SyncListStruct<SerializableBrush>.Operation op, int index)
    {
        if (op == SyncList<SerializableBrush>.Operation.OP_CLEAR)
        {
            clientPainter.ClearTexture();
        }
        else if (op == SyncList<SerializableBrush>.Operation.OP_ADD)
        {
            SerializableBrush b = Brushes.Last();
            clientPainter.Paint(b.position, b.scale, b.color);
        }
    }

    //Marker Methods
    void AddMarker(Transform newMarkerTransform, Color newMarkerColor)
    {
        SerializableMarker marker;
        marker.position = newMarkerTransform.position;
        marker.rotation = newMarkerTransform.rotation;
        marker.color = newMarkerColor;

        Markers.Add(marker);
    }

    void DestroyMarker(Vector3 destroyedMarkerPosition)
    {
        for (int i = Markers.Count - 1; i >= 0; i--)
        {
            if (Markers[i].position.Equals(destroyedMarkerPosition))
                Markers.RemoveAt(i);
        }
    }

    void ClearMarkers()
    {
        Markers.Clear();
    }

    void MarkerListChanged(SyncListStruct<SerializableMarker>.Operation op, int index)
    {
        if (op == SyncList<SerializableMarker>.Operation.OP_CLEAR)
        {
            MarkerManager.DestroyAllMarkers();
        }
        else if (op == SyncList<SerializableMarker>.Operation.OP_ADD)
        {
            SerializableMarker m = Markers.Last();
            MarkerManager.CreateMarker(transform.parent, m.position, m.rotation, m.color);
        }
        else if (op == SyncList<SerializableMarker>.Operation.OP_REMOVEAT)
        {
            MarkerManager.DestroyMarkerAt(index);
        }
    }
}
