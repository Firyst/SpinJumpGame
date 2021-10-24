using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehave : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;

    public Transform cameraPos;
    public Transform platforms;
    public GameObject gameUI;
    public Material deathMat;

    public int frame = 0;
    public bool playing = false;

    private Vector3 finalPos = new Vector3(0, -4.849875f, -12.98f);
    private Vector3 beginPos;
    private Vector3 beginRot;
    private Vector3 moveVec;
    private Vector3 moveRot;

    public Material platMat;
    public Material poleMat;
    public Camera cameraC;


    // переменные, обозначающие, насколько изменился цвет. Используются для анимации.
    private float delta1;
    private float delta2;
    private float delta3;
    private float col1;
    private float col2;
    private float col3;

    void Start()
    {
        
    }

    public void StartAnimaton()
    {
        playing = true;
        beginPos = cameraPos.position;
        beginRot = platforms.eulerAngles;
        moveVec = finalPos - beginPos;
        moveRot = -1 * platforms.eulerAngles;
        float h = 0, s = 0, v = 0;
        UnityEngine.Color.RGBToHSV(platMat.color, out h, out s, out v);
        delta1 = (200 / 360f - h) / 99f;
        col1 = h;
        UnityEngine.Color.RGBToHSV(poleMat.color, out h, out s, out v);
        delta2 = (190 / 360f - h) / 99f;
        col2 = h;
        UnityEngine.Color.RGBToHSV(cameraC.backgroundColor, out h, out s, out v);
        delta3 = (220 / 360f - h) / 99f;
        col3 = h;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playing && frame < 100)
        {
            float mult = (Mathf.Sin((frame / 100f * 2 - 1) * Mathf.PI / 2) + 1) / 2;
            cameraPos.position = beginPos + moveVec * mult;
            platforms.eulerAngles = beginRot + moveRot * mult;

            Color color = new UnityEngine.Color(Mathf.Clamp(255 - 2.3f * frame, 23, 255)/255f, Mathf.Clamp(50 + 1.11f * frame, 0, 161)/255f, Mathf.Clamp(frame * 2.3f, 0, 230)/255f);
            //print((255 - 2.32f * frame).ToString() + "  " + (50 + 1.11f * frame).ToString() + "  " + (frame * 2.29f).ToString());
            // Color color = new UnityEngine.Color(1, 0.196f + frame / 124.3f, frame / 100f);
            deathMat.SetColor("_Color", color);
            Color color2 = new UnityEngine.Color(1, 0.078f, 0);
            deathMat.SetColor("_EmissionColor", color2 * (Mathf.Clamp(1 - frame * 0.05f, 0, 1)));

            platMat.color = UnityEngine.Color.HSVToRGB(Mathf.Clamp(col1 + frame * delta1, 0, 0.784f), 0.9f, 0.9f);
            poleMat.color = UnityEngine.Color.HSVToRGB((col2 + frame * delta2), 0.75f, 0.5f);
            cameraC.backgroundColor = UnityEngine.Color.HSVToRGB((col3 + frame * delta3), 0.5f, 0.7f);
            print((9 + (191 / 99f) * frame));
            frame += 1;
        }
        if (frame == 100)
        {
            player.GetComponent<BallBehave>().allowCameraMove = false;
            player.GetComponent<Rigidbody>().transform.position = new Vector3(0, -8.5f, -2.5f);
            cameraPos.position = finalPos;
            playing = false;
            frame = 0;
            gameUI.GetComponent<GameUIScript>().Begin();
            player.GetComponent<BallBehave>().allowCameraMove = true;


            platforms.gameObject.GetComponent<PlatBehave>().PlatRemoveZones();
            Color color = new UnityEngine.Color(1, 0.196f, 0);
            deathMat.SetColor("_Color", color);
            Color color2 = new UnityEngine.Color(1, 0.078f, 0);
            deathMat.SetColor("_EmissionColor", color2);
            
        }
    }
}
