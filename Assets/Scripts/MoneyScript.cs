using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScript : MonoBehaviour
{
    public GameObject moneyObject, moneyMesh, gameUI, coinsHandler;
    public Collider coinCollider;
    public int amount;
    public Text animText;
    public Animation textCollectAnim, collectAnim;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public void SetCollider(bool state)
    {
        coinCollider.enabled = state;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moneyObject.transform.eulerAngles = new Vector3(moneyObject.transform.eulerAngles.x, moneyObject.transform.eulerAngles.y + 5, moneyObject.transform.eulerAngles.z);
    }
}
