using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button playButton, localButton, shopButton;
    public Button shopLeftButton, shopRightButton, shopBuyButton;
    public Image fader;
    public Color curColor;
    public Animation fadeAnim;
    public Animation fadeAnim2;
    public Animation fadeAnim3;
    public Animation cameraAnim;
    public Text title;


    private bool shop = false;


    
    // Start is called before the first frame update

    void start_game()
    {
        Debug.Log("play");

        fadeAnim.Play();

    }

    void visit_shop()
    {
        shop = true;
        title.text = "Shop";
        playButton.enabled = false;
        localButton.enabled = false;
        shopButton.enabled = false;

        shopLeftButton.enabled = true;
        shopRightButton.enabled = true;
        shopBuyButton.enabled = true;

        fadeAnim3["fade3"].speed = -1;
        fadeAnim3["fade3"].time = 1;
        fadeAnim2["fade2"].speed = 1;
        fadeAnim2["fade2"].time = 0;
        fadeAnim2.Play();
        fadeAnim3.Play();
        print("shop");
        cameraAnim.Play();
    }

    void moveLeft()
    {

    }
    void Start()
    {
        shop = false;

        playButton.onClick.AddListener(start_game);
        shopButton.onClick.AddListener(visit_shop);

        fadeAnim3["fade3"].speed = 0;
        fadeAnim3["fade3"].time = 1;

        fadeAnim3.Play();

        //shopLeftButton.enabled = false;
        //shopRightButton.enabled = false;
        //shopBuyButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (fader.color.a > 0.99f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
