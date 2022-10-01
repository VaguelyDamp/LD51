using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UI_Meters : MonoBehaviour
{
    [MenuItem("GameObject/UI/Meters/Donut Meter")]
    public static void CreateDonutMeter()
    {
        GameObject donut = new GameObject("Donut Meter");
        donut.transform.parent = Selection.activeTransform;

        Image image = donut.AddComponent<Image>();
        image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/UI/UI_DonutBar.png", typeof(Sprite));
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillOrigin = 2;

        FillMeter fill = donut.AddComponent<FillMeter>();
        fill.bar = image;
        fill.minVal = 0;
        fill.maxVal = 1;

        Undo.RegisterCreatedObjectUndo(donut, "Create " + donut.name);
        Selection.activeObject = donut;
    }

    [MenuItem("GameObject/UI/Meters/Bar Meter")]
    public static void CreateBarMeter()
    {
        GameObject bar = new GameObject("Bar Meter");
        bar.transform.parent = Selection.activeTransform;

        Image image = bar.AddComponent<Image>();
        image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/UI/UI_BasicBox.png", typeof(Sprite));
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Horizontal;
        image.fillOrigin = 0;

        FillMeter fill = bar.AddComponent<FillMeter>();
        fill.bar = image;
        fill.minVal = 0;
        fill.maxVal = 1;

        Undo.RegisterCreatedObjectUndo(bar, "Create " + bar.name);
        Selection.activeObject = bar;
    }

    [MenuItem("GameObject/UI/Meters/Chunk Meter")]
    public static void CreateChunkMeter()
    {
        GameObject bar = new GameObject("Chunk Meter");
        bar.transform.parent = Selection.activeTransform;
        bar.AddComponent<Image>().enabled = false;

        ChunkMeter meter = bar.AddComponent<ChunkMeter>();
        meter.chunkSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Branding/dampboi.png", typeof(Sprite));
        meter.chunkSize = new Vector3(1, 1, 1);
        meter.chunkSpacing = new Vector3(80, 0, 1);

        Undo.RegisterCreatedObjectUndo(bar, "Create " + bar.name);
        Selection.activeObject = bar;
    }

    [MenuItem("GameObject/UI/Meters/Circular Needle Guage")]
    public static void CreateCircularGuage()
    {
        GameObject guage = new GameObject("Circular Needle Guage");
        guage.transform.parent = Selection.activeTransform;
        guage.AddComponent<Image>().enabled = false;

        GameObject needle = new GameObject("Needle");
        needle.transform.parent = guage.transform;
        needle.transform.localPosition = Vector3.zero;

        GameObject needleSprite = new GameObject("NeedleSprite");
        needleSprite.transform.parent = needle.transform;
        needleSprite.transform.localPosition = new Vector3(0, 40, 0);
        
        Image image = needleSprite.AddComponent<Image>();
        image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/UI/UI_PointerThin.png", typeof(Sprite));

        GameObject lowPoint = new GameObject("LowPoint");
        lowPoint.transform.parent = guage.transform;
        lowPoint.transform.localPosition = Vector3.zero;
        lowPoint.transform.localEulerAngles = new Vector3(0, 0, -90); 

        GameObject highPoint = new GameObject("HighPoint");
        highPoint.transform.parent = guage.transform;
        highPoint.transform.localPosition = Vector3.zero;
        highPoint.transform.localEulerAngles = new Vector3(0, 0, 90); 

        MoveMeter meter = guage.AddComponent<MoveMeter>();
        meter.needle = needle;
        meter.lowPoint = lowPoint.transform;
        meter.highPoint = highPoint.transform;

        Undo.RegisterCreatedObjectUndo(guage, "Create " + guage.name);
        Selection.activeObject = guage;
    }

    [MenuItem("GameObject/UI/Meters/Linear Needle Guage")]
    public static void CreateLinearGuage()
    {
        GameObject guage = new GameObject("Linear Needle Guage");
        guage.transform.parent = Selection.activeTransform;
        guage.AddComponent<Image>().enabled = false;

        GameObject needle = new GameObject("Needle");
        needle.transform.parent = guage.transform;
        needle.transform.localPosition = Vector3.zero;

        GameObject needleSprite = new GameObject("NeedleSprite");
        needleSprite.transform.parent = needle.transform;
        needleSprite.transform.localPosition = Vector3.zero;
        needleSprite.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        
        Image image = needleSprite.AddComponent<Image>();
        image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/UI/UI_Pointer.png", typeof(Sprite));

        GameObject lowPoint = new GameObject("LowPoint");
        lowPoint.transform.parent = guage.transform;
        lowPoint.transform.localPosition = Vector3.zero;
        lowPoint.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        GameObject highPoint = new GameObject("HighPoint");
        highPoint.transform.parent = guage.transform;
        highPoint.transform.localPosition = new Vector3(100, 0, 0);
        highPoint.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        MoveMeter meter = guage.AddComponent<MoveMeter>();
        meter.needle = needle;
        meter.lowPoint = lowPoint.transform;
        meter.highPoint = highPoint.transform;

        Undo.RegisterCreatedObjectUndo(guage, "Create " + guage.name);
        Selection.activeObject = guage;
    }

    [MenuItem("GameObject/UI/Meters/Scale Meter")]
    public static void CreateScaleMeter()
    {
        GameObject guage = new GameObject("Scale Meter");
        guage.transform.parent = Selection.activeTransform;
        guage.AddComponent<Image>().enabled = false;

        GameObject needle = new GameObject("Needle");
        needle.transform.parent = guage.transform;
        needle.transform.localPosition = Vector3.zero;

        GameObject needleSprite = new GameObject("NeedleSprite");
        needleSprite.transform.parent = needle.transform;
        needleSprite.transform.localPosition = Vector3.zero;
        needleSprite.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        
        Image image = needleSprite.AddComponent<Image>();
        image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Branding/dampboi.png", typeof(Sprite));

        GameObject lowPoint = new GameObject("LowPoint");
        lowPoint.transform.parent = guage.transform;
        lowPoint.transform.localPosition = Vector3.zero;
        lowPoint.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        GameObject highPoint = new GameObject("HighPoint");
        highPoint.transform.parent = guage.transform;
        highPoint.transform.localPosition = Vector3.zero;
        highPoint.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        MoveMeter meter = guage.AddComponent<MoveMeter>();
        meter.needle = needle;
        meter.lowPoint = lowPoint.transform;
        meter.highPoint = highPoint.transform;

        Undo.RegisterCreatedObjectUndo(guage, "Create " + guage.name);
        Selection.activeObject = guage;
    }
}
