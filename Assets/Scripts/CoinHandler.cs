using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinHandler : MonoBehaviour
{

    public GameObject CoinsHandler, PlayerBall;
    public Text ScoreText;

    public Material Bronze;
    public Material Silver;
    public Material Gold;

    

    private int last_coin;
    // Start is called before the first frame update
    void Start()
    {
        last_coin = -54;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideCoins()
    {
        foreach (Transform coin in CoinsHandler.transform)
        {
            coin.GetComponent<MoneyScript>().SetCollider(false);
        }
    }
    public void Restart()
    {
        foreach (Transform coin in CoinsHandler.transform)
        {
            coin.transform.position = new Vector3(coin.transform.position.x, 100, coin.transform.position.z);
        }
        last_coin = -54;
    }

    private void FixedUpdate()
    {
        int score = int.Parse(ScoreText.text);
        foreach (Transform coin in CoinsHandler.transform)
        {
            if (coin.transform.position.y > PlayerBall.transform.position.y + 20)
                // проходимся по детям (монеткам), перемещаем их по необходимости
            {
                coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Bronze;
                int pos = last_coin - Random.Range(7, 12) * 10;
                coin.transform.position = new Vector3(coin.transform.position.x, pos, coin.transform.position.z);
                coin.GetComponent<MoneyScript>().SetCollider(true);
                last_coin = pos;
                if (score > 48)
                {
                    print("silver");
                    coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Silver;
                    coin.GetComponent<MoneyScript>().amount = Random.Range(3, 6);
                    if (score > 96)
                    {
                        coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Gold;
                        coin.GetComponent<MoneyScript>().amount = Random.Range(6, 15);
                    }
                } else
                {
                    coin.GetComponent<MoneyScript>().amount = Random.Range(1, 2);
                }
            }
        }
    }
}
