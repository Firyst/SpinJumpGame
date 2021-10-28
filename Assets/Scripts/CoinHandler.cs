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
    public Material Diamond;


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
                int pos = last_coin - Random.Range(5, 10) * 10;
                coin.transform.position = new Vector3(coin.transform.position.x, pos, coin.transform.position.z);
                coin.GetComponent<MoneyScript>().SetCollider(true); // включение коллайдера на монетке
                last_coin = pos;
                coin.GetComponent<MoneyScript>().isGlowing = false;
                if (score > 45)
                {
                    if (score > 150)
                    {
                        // алмазная rgb монетка
                        coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Diamond;
                        coin.GetComponent<MoneyScript>().amount = 2*((score + 5) / 25) - 1;
                        coin.GetComponent<MoneyScript>().isGlowing = true;
                    }
                    else if (score > 95)
                    { // золотая монетка
                        coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Gold;
                        coin.GetComponent<MoneyScript>().amount = 1 + ((score + 5) / 25);
                    } else
                    { // серебряная монетка
                        coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Silver;
                        coin.GetComponent<MoneyScript>().amount = ((score + 5) / 25);
                    }
                } else
                {
                    // бронзовая монетка
                    coin.GetComponent<MoneyScript>().amount = 1;
                }
            }
        }
    }
}
