using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button playButton, localButton, shopButton;
    public Image fader;
    public Color curColor;
    public Animation fadeAnim;

    
    // Start is called before the first frame update

    void start_game()
    {
        Debug.Log("play");
        /*
        fader.gameObject.GetComponent<Animation>().enabled = true;
        fader.gameObject.GetComponent<Animation>().Play();
        */

        fadeAnim.Play();

    }
    void Start()
    {
        playButton.onClick.AddListener(start_game);
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
