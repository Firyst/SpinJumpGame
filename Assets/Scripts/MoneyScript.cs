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
    public Animation collectAnim;
    public bool isGlowing = false;

    private bool currentState = true;
    private int matColor = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public void SetCollider(bool state)
    {
        coinCollider.enabled = state;
        if (state)
        {
            collectAnim["moneyAnim"].speed = 0;
            collectAnim["moneyAnim"].time = 0;
            collectAnim.Play();
            currentState = true;
        } else
        {
            if (currentState)
            {
                collectAnim["moneyAnim"].speed = 1.25f;
                collectAnim.Play();
                currentState = false;
            }
        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moneyObject.transform.eulerAngles = new Vector3(moneyObject.transform.eulerAngles.x, moneyObject.transform.eulerAngles.y + 5, moneyObject.transform.eulerAngles.z);
        if (isGlowing)
        {
            Color color = UnityEngine.Color.HSVToRGB((matColor % 360)/360f, 0.95f, 0.65f);
            moneyMesh.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color);
        }
        matColor += 6;
        if (matColor > 360)
        {
            matColor = 0;
        }
    }
}
