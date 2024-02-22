using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
public enum miniMapposition
{
    rightdown,leftdown,
}

public class minimap : MonoBehaviour
{
    public float terrionHeight = 0;
    public float terrionWidth = 0;
    public Camera cameraMap;
    public Camera cameraMain;
    public Texture image;

    public string targetTag;
    public Texture point;
    public Texture cmTexture;
    public float minimapw = 128;
    public float minimaph = 128;
    public miniMapposition mmPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraMap.transform.Translate(new Vector3(terrionWidth / 2, 200, terrionHeight / 2), Space.World);
        cameraMap.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        switch (mmPosition)
        {
            case miniMapposition.rightdown:
                GUI.BeginGroup(new Rect(Screen.width -minimapw, Screen.height - minimaph, minimapw, minimaph));
                break;
            case miniMapposition.leftdown:
                GUI.BeginGroup(new Rect(0, Screen.height - minimaph, minimapw, minimaph));
                break;
        }
        GUI.DrawTexture(new Rect(0, 0, minimapw, minimaph), image, ScaleMode.ScaleToFit, false, 0);
        GUI.EndGroup();
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject obj in objs)
        {
            drawFlage(obj.transform, point, 4, 4);
        }
        drawRotateFlage(cameraMain.transform, cmTexture, 10, 10);
    }
    void drawFlage(Transform tf, Texture flag, float flagw, float flagh)
    {
        Vector3 screenPos = cameraMap.WorldToScreenPoint(tf.position);
        float w1 = tf.position.x / terrionWidth;
        float h1 = tf.position.z / terrionHeight;
        float mpw = w1 * minimapw;
        float mph = minimaph - h1 * minimaph;

        switch (mmPosition)
        {
            case miniMapposition.rightdown:
                GUI.BeginGroup(new Rect(Screen.width -minimapw, Screen.height - minimaph, minimapw, minimaph));
                break;
            case miniMapposition.leftdown:
                GUI.BeginGroup(new Rect(0, Screen.height - minimaph, minimapw, minimaph));
                break;

        }
        GUI.DrawTexture(new Rect(mpw - flagw / 2, mph - flagh / 2, flagw, flagh), flag);
        GUI.EndGroup();
    }
    void drawRotateFlage(Transform tf, Texture flag, float flagw, float flagh)
    {
        Vector3 screenPose = cameraMap.WorldToScreenPoint(tf.position);
        float w1 = tf.position.x / terrionWidth;
        float h1 = tf.position.z / terrionHeight;
        float mpw = w1 * minimapw;
        float mph = minimaph - h1 * minimaph;
        switch (mmPosition)
        {
            case miniMapposition.rightdown:
                GUI.BeginGroup(new Rect(Screen.width -minimapw, Screen.height - minimaph, minimapw, minimaph));
                break;
            case miniMapposition.leftdown:
                GUI.BeginGroup(new Rect(0, Screen.height - minimaph, minimapw, minimaph));
                break;

        }
        GUIUtility.RotateAroundPivot(cameraMain.transform.eulerAngles.y + 90, new Vector2(mpw + flagw / 2, mph));
        GUI.DrawTexture(new Rect(mpw - flagw / 2, mph - flagh / 2, flagw, flagh), flag);
        GUI.EndGroup();
    }
    void miniMapMove()
    {
        Vector3 mp = Input.mousePosition;
        Debug.Log("mp:" + mp);
        switch (mmPosition)
        {
            case miniMapposition.rightdown:
                {
                    float miniMapX = Screen.width - minimapw;
                    float miniMapY = Screen.height - minimaph;
                    if (mp.x > miniMapX && mp.y > miniMapY)
                    {
                        float rx = mp.x - miniMapX;
                        float ry = mp.y - miniMapY;
                        float w1 = rx / minimapw;
                        float h1 = ry / minimaph;
                        cameraMain.transform.position = new Vector3(w1 * terrionWidth, cameraMain.transform.position.y, h1 * terrionHeight);
                    }

                }
                break;
            case miniMapposition.leftdown:
                {
                    float miniMapX = minimapw;
                    float miniMapY = minimaph;
                    if (mp.x < miniMapX && mp.y < miniMapY)
                    {
                        float rx = mp.x;
                        float ry = mp.y;
                        float w1 = rx / minimapw;
                        float h1 = ry / minimaph;
                        cameraMain.transform.position = new Vector3(w1 * terrionWidth, cameraMain.transform.position.y, h1 * terrionHeight);
                    }
                }
                break;
        }
    }
}
