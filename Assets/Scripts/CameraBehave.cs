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

    public int frame = 0;
     public bool playing = false;

    private Vector3 finalPos = new Vector3(0, -4.849875f, -12.98f);
    private Vector3 beginPos;
    private Vector3 beginRot;
    private Vector3 moveVec;
    private Vector3 moveRot;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playing && frame < 100)
        {
            float mult = (Mathf.Sin((frame / 100f * 2 - 1) * Mathf.PI / 2) + 1) / 2;
            cameraPos.position = beginPos + moveVec * mult;
            platforms.eulerAngles = beginRot + moveRot * mult;

            frame += 1;
        }
        if (frame == 100)
        {
            player.GetComponent<BallBehave>().allowCameraMove = false;
            player.GetComponent<Rigidbody>().transform.position = new Vector3(0, -8.5f, -2.5f);
            cameraPos.position = finalPos;
            playing = false;
            frame = 0;
            gameUI.GetComponent<GameUIScript>().Reset();
            player.GetComponent<BallBehave>().allowCameraMove = true;
        }
    }
}
