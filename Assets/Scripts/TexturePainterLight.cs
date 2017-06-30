using UnityEngine;

public class TexturePainterLight : MonoBehaviour
{
    public GameObject brushContainer; //our container for the brushes painted
    public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)

    int brushCounter = 0, MAX_BRUSH_COUNT = 1000; //To avoid having millions of brushes
    bool saving = false; //Flag to check if we are saving the texture

    void Start()
    {
        ClearTexture(); //Gets black sometimes
    }

    public void Paint(Vector3 pos, Vector3 scale, Color color)
    {
        if (saving) return;

        GameObject brushObj;
        brushObj = (GameObject) Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity")); //Paint a brush
        brushObj.GetComponent<SpriteRenderer>().color = color; //Set the brush color

        color.a = 2.0f; // Brushes have alpha to have a merging effect when painted over.
        brushObj.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
        brushObj.transform.localPosition = pos; //The position of the brush (in the UVMap)
        brushObj.transform.localScale = scale; //The size of the brush

        brushCounter++; //Add to the max brushes

        if (brushCounter >= MAX_BRUSH_COUNT)
            //If we reach the max brushes available, flatten the texture and clear the brushes
            Invoke("SaveTexture", 0.1f);
    }

    //Sets the base material with a our canvas texture, then removes all our brushes
    public void SaveTexture()
    {
        saving = true;

        RenderTexture.active = canvasTexture;
        Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base

        ClearBrushes();

        saving = false;
    }


    public void ClearTexture()
    {
        ClearBrushes();

        baseMaterial.mainTexture = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = null;
    }

    private void ClearBrushes()
    {
        foreach (Transform child in brushContainer.transform)
            Destroy(child.gameObject);
    }
}
