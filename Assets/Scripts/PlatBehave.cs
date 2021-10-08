using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PlatBehave : MonoBehaviour
{

    public int score = 0;
    public GameObject handler;
    public Transform ballPos;
    
    public float last_pos = -10;

    public bool paused;

    List<Transform> passed = new List<Transform>();


    System.Random rnd = new System.Random();
    

    // Start is called before the first frame update

    void TeleportPlat(Transform plat)
    {
        var Rend = plat.GetComponent<Renderer>();
        plat.position = new Vector3(0, last_pos - 10, 0);
        /*Color col = new Color(255, 0, 0);
        Rend.material.SetColor("_Color", col);*/
        if (plat.childCount > 0)
        {
            for (int i = 0; i < plat.childCount; i++)
            {
                Transform zone = plat.transform.GetChild(i);
                zone.gameObject.SetActive(false);
            }
            if (rnd.Next(0, 2) == 1) {
                for (int i = 0; i < rnd.Next(0, 8); i++)
                {
                    Transform zone = plat.transform.GetChild(rnd.Next(0, plat.childCount - 1));
                    zone.gameObject.SetActive(true);
                }
            }
        }
        last_pos = last_pos - 10;

        plat.eulerAngles = new Vector3(plat.eulerAngles.x, plat.eulerAngles.y, rnd.Next(0, 300));

    }

    public void Restart()
    {
        for (int i = 0; i < handler.transform.childCount; i++)
        {
            Transform plat = handler.transform.GetChild(i);
            if (plat.tag != "Start_platform")
            {
                plat.position = new Vector3(plat.position.x, plat.position.y - last_pos + Mathf.Max(-70, last_pos), plat.position.z);

            }

            
        }
        last_pos = -70;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {

            for (int i = 0; i < handler.transform.childCount; i++)
            {
                Transform plat = handler.transform.GetChild(i);

                if (plat.position.y - 20 > ballPos.position.y)
                {

                    if (plat.tag != "Start_platform")
                    {
                        passed.Add(plat);
                        plat.position = new Vector3(plat.position.x, plat.position.y - 10000, plat.position.z);
                    }

                }
            }
            if ((last_pos - ballPos.position.y > -25))
            {
                int dind = rnd.Next(0, passed.Count);
                TeleportPlat(passed[dind]);
                passed.RemoveAt(dind);
            }
        }
    }
}
