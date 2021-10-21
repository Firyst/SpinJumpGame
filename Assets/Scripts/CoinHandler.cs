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
        last_coin = -55;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        int score = int.Parse(ScoreText.text);
        foreach (Transform coin in CoinsHandler.transform)
        {
            if (coin.transform.position.y > PlayerBall.transform.position.y + 20)
                // ���������� �� ����� (��������), ���������� �� �� �������������
            {
                coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Bronze;
                int pos = last_coin - Random.Range(10, 15) * 10;
                coin.transform.position = new Vector3(coin.transform.position.x, pos, coin.transform.position.z);
                coin.GetComponent<MoneyScript>().SetCollider(true);
                last_coin = pos;
                print(score);
                if (score > 10)
                {
                    print("silver");
                    coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Silver;
                    coin.GetComponent<MoneyScript>().amount = Random.Range(2, 4);
                    if (score > 40)
                    {
                        coin.GetComponent<MoneyScript>().moneyMesh.GetComponent<MeshRenderer>().material = Gold;
                        coin.GetComponent<MoneyScript>().amount = Random.Range(5, 7);
                    }
                } else
                {
                    coin.GetComponent<MoneyScript>().amount = Random.Range(1, 2);
                }
            }
        }
    }
}