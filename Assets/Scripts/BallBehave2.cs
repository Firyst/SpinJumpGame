using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehave2 : MonoBehaviour
{
    public LayerMask plLayer;
    public GameObject par;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("collide");
        if (!par.GetComponent<BallBehave>().paused)
        {
            par.GetComponent<BallBehave>().sideCollided = true;
        }
    }
}
