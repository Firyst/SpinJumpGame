using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIScript : MonoBehaviour
{
    public Button pauseButton;
    public Button quitButton;
    public Button restartButton;
    public bool paused = false;
    public GameObject menu;
    public GameObject player;
    public GameObject platforms;
    public GameObject coins;
    public Animation pauseAnim;
    public Animation fadeAnim;
    public Animation deathAnim;

    public Text ScoreText;
    public Text HighSText;
    public Text CurrSText;

    public Image pauseImage;
    public Image playImage;
    public Image menuImage;
    public Image fadeImage;

    public Animation fAnim1;
    public Animation fAnim2;
    public Animation fAnim3;
    public Animation fAnim4;
    public Animation rAnim1;


    private bool isQuit = false;
    private bool isRestart = false;



    // Start is called before the first frame update


    public void Begin()
    {
        platforms.GetComponent<PlatBehave>().RestartPlat();
        deathAnim["death"].time = 0.4f;
        deathAnim["death"].speed = -1;
        deathAnim.Play();

        playImage.enabled = false;
        menuImage.enabled = false;
        quitButton.enabled = false;
        restartButton.enabled = false;
        pauseImage.enabled = true;
        // player.GetComponent<BallBehave>().allowCameraMove = false;
        // player.GetComponent<Rigidbody>().transform.position = new Vector3(0, 1.5f, -2.5f);
        isRestart = false;
        paused = false;


        platforms.GetComponent<PlatBehave>().paused = false;
        coins.GetComponent<CoinHandler>().Restart();
        

        Setup();
        pauseButton.enabled = true;
        print(pauseButton.enabled);
    }

    public void Restart()
    {
        rAnim1["fadeC1"].speed = 1;
        rAnim1["fadeC1"].time = 0;
        deathAnim["death"].speed = 1;

        player.GetComponent<BallBehave>().allowCameraMove = false;
        restartButton.enabled = false;
        deathAnim.Play();
        isRestart = true;
        rAnim1.Play();
        coins.GetComponent<CoinHandler>().HideCoins();
        // platforms.GetComponent<PlatBehave>().Restart();

    }


    int get_highscore()
    {
        float highscore = Mathf.Pow(PlayerPrefs.GetFloat("highscore"), 8.5f);
        if (Mathf.Abs(highscore - Mathf.RoundToInt(highscore)) < 0.001f)
        {
            return Mathf.RoundToInt(highscore);
        } else
        {
            return 0;
        }
    }

    public void game_end()
    {
        paused = true;
        pauseButton.enabled = false;
        platforms.GetComponent<PlatBehave>().paused = true;
        pauseImage.enabled = false;
        playImage.enabled = false;
        menuImage.enabled = true;
        quitButton.enabled = true;

        print("dead");
        fAnim1["game_over_anim"].speed = 1;
        fAnim2["game_over_anim2"].speed = 1;
        fAnim3["game_over_anim3"].speed = 1;
        fAnim4["paused_breath"].speed = 1;
        fAnim4["paused_breath"].time = 0;

        fAnim1.Play();
        fAnim2.Play();
        fAnim3.Play();
        fAnim4.Play();
        
        restartButton.enabled = true;

        int score = int.Parse(CurrSText.text);

        ScoreText.text = "Score: " + CurrSText.text;

        int highscore = get_highscore();

        if (score > highscore)
        {
            PlayerPrefs.SetFloat("highscore", Mathf.Pow(score, 0.11764705f));
        }

        HighSText.text = "High: " + get_highscore();
        
    }

    void pause()
    {
        print("pause event " + (paused).ToString());
        paused = !paused;
        pauseImage.enabled = !paused;
        platforms.GetComponent<PlatBehave>().paused = paused;
        playImage.enabled = paused;
        menuImage.enabled = paused;
        quitButton.enabled = paused;
        if (paused)
        {
            pauseAnim["paused_breath"].speed = 1;
            
        } else
        {
            pauseAnim["paused_breath"].speed = 0;
            pauseAnim["paused_breath"].time = 0;
        }
    }

    void quit()
    {
        if (!isQuit)
        {
            Debug.Log("quit");
            isQuit = true;
            fadeAnim["fade2"].speed = -1;
            fadeAnim.Play();
            fadeAnim["fade2"].time = 1;
        }
    }

    void Start()
    {
        Setup();
        pauseButton.onClick.AddListener(pause);
        quitButton.onClick.AddListener(quit);
        restartButton.onClick.AddListener(Restart);
    }

    // Update is called once per frame


    void Setup()
    {
        playImage.enabled = false;
        menuImage.enabled = false;
        quitButton.enabled = false;
        restartButton.enabled = false;

        pauseAnim["paused_breath"].speed = 0;
        pauseAnim["paused_breath"].time = 0;

        fAnim1["game_over_anim"].speed = 0;
        fAnim1["game_over_anim"].time = 0;

        fAnim2["game_over_anim2"].speed = 0;
        fAnim2["game_over_anim2"].time = 0;

        fAnim3["game_over_anim3"].speed = 0;
        fAnim3["game_over_anim3"].time = 0;

        fAnim4["paused_breath"].speed = 0;
        fAnim4["paused_breath"].time = 0;

        rAnim1["fadeC1"].speed = 0;
        rAnim1["fadeC1"].time = 0;

        fAnim1.Play();
        fAnim2.Play();
        fAnim3.Play();
        fAnim4.Play();
        rAnim1.Play();

    }
    void Update()
    {
        if (isQuit && fadeImage.color.a == 1)
        {
            SceneManager.LoadScene(0);
        }
        if (isRestart)
        {
            // Debug.Log(deathAnim["death"].time);
        }
        if (isRestart && deathAnim["death"].time >= 0.4f)
        {
            player.GetComponent<BallBehave>().Restart();
            isRestart = false;
        }
    }
}
