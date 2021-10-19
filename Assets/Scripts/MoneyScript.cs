using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    public GameObject moneyObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moneyObject.transform.eulerAngles = new Vector3(moneyObject.transform.eulerAngles.x, moneyObject.transform.eulerAngles.y + 5, moneyObject.transform.eulerAngles.z);
    }
}
