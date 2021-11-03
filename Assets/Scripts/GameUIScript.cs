using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class GameUIScript : MonoBehaviour
{
    public Button pauseButton;
    public Button quitButton;
    public Button restartButton;
    public Button adSkipButton;
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
    public Text MoneyText;

    public Image pauseImage;
    public Image playImage;
    public Image menuImage;
    public Image fadeImage;

    public Animation fAnim1;
    public Animation fAnim2;
    public Animation fAnim3;
    public Animation fAnim4;
    public Animation rAnim1;
    public Animation rAnim2;
    public Animation reviveAnim1;
    public Animation reviveAnim2;
    public Animation reviveAnim3; // текст no thanks

    private bool isQuit = false;
    private bool isRestart = false;
    private bool ad_available = true;
    private bool ad_playing = false;
    private bool ad_played = false;
    private bool decline_enabled = false; // вылезла ли кнопка no thanks
    private bool ad_success = false;

    private float ad_time = 0;
    private float restart_time = 0;

    public RewardedAd rewarded;

    // Start is called before the first frame update
    public void RequestReliveRewarded()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        //string adUnitId = "ca-app-pub-8454720276447685/2412245698"; // Мой АЙДИ";

        rewarded = new RewardedAd(adUnitId);

        rewarded.OnUserEarnedReward += ad_end;
        rewarded.OnAdFailedToShow += ad_closed;

        AdRequest request = new AdRequest.Builder().Build();
        rewarded.LoadAd(request);
    }

    int get_pref(string field)
    {
        // получает число из player pref
        float value = Mathf.Pow(PlayerPrefs.GetFloat(field), 8.5f);
        if (Mathf.Abs(value - Mathf.RoundToInt(value)) < 0.001f)
        {
            return Mathf.RoundToInt(value);
        }
        else
        {
            return 0;
        }
    }
    public void Begin()
    {
        decline_enabled = false;
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
    }

    public void Restart()
    {
        if (ad_playing)
        {
            play_ad();

        }
        else
        {
            if (!ad_played)
            {
                rAnim2["game_over_anim4"].time = 1;
                rAnim2["game_over_anim4"].speed = -1;
                CurrSText.text = "0";
                ad_available = true;
                RequestReliveRewarded();
            } else
            {
                ad_available = false;
            }
            rAnim1["fadeC1"].speed = 1;
            rAnim1["fadeC1"].time = 0;
            deathAnim["death"].speed = 1;

            rAnim2.Play();


            player.GetComponent<BallBehave>().allowCameraMove = false;
            restartButton.enabled = false;
            deathAnim.Play();
            isRestart = true;
            rAnim1.Play();
            coins.GetComponent<CoinHandler>().HideCoins();
            // platforms.GetComponent<PlatBehave>().Restart();
            ad_played = false;
        }
    }

    public void ad_closed(object sender, AdErrorEventArgs args)
    {
        print("AD CLOSED");
        ad_playing = true;
        game_end();
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

    void play_ad()
    {
        // реклама

        ad_played = true;
        ad_playing = false;

        if (rewarded.IsLoaded())
        {
            rewarded.Show();
        } else
        {
            reviveAnim1["game_over_anim"].time = 1;
            reviveAnim1["game_over_anim"].speed = -1;
            reviveAnim2["revive_image_anim"].time = 1;
            reviveAnim2["revive_image_anim"].speed = -1;
            if (decline_enabled)
            {
                reviveAnim3["game_over_anim"].time = 1;
                reviveAnim3["game_over_anim"].speed = -1;
            }

            reviveAnim1.Play();
            reviveAnim2.Play();
            reviveAnim3.Play();

            Restart();
        }
    }

    void ad_end(object sender, Reward args)
    {
        // вызывается после воспроизведнеия рекламы
        print("AD SHOWN");
        ad_success = true;
        reviveAnim1["game_over_anim"].time = 1;
        reviveAnim1["game_over_anim"].speed = -1;
        reviveAnim2["revive_image_anim"].time = 1;
        reviveAnim2["revive_image_anim"].speed = -1;
        if (decline_enabled)
        {
            print("ANIM");
            reviveAnim3["game_over_anim"].time = 1;
            reviveAnim3["game_over_anim"].speed = -1;
        }

        reviveAnim1.Play();
        reviveAnim2.Play();
        reviveAnim3.Play();

        Restart();
    }
    public void game_end()
    {
        ad_success = false;
        MoneyText.text = "$" + (get_pref("money")).ToString();
        ad_time = 0;
        paused = true;
        pauseButton.enabled = false;
        platforms.GetComponent<PlatBehave>().paused = true;
        pauseImage.enabled = false;
        playImage.enabled = false;
        menuImage.enabled = true;
        quitButton.enabled = true;
        adSkipButton.enabled = false;
        restartButton.enabled = true;

        if (!ad_playing)
        {
            // смерть с рекламой 
            ad_playing = true;
            if (int.Parse(CurrSText.text) > 50 && ad_available && rewarded.IsLoaded())
            {
                fAnim1["game_over_anim"].speed = 1;
                reviveAnim1["game_over_anim"].time = 0;
                reviveAnim1["game_over_anim"].speed = 1;
                reviveAnim2["revive_image_anim"].time = 0;
                reviveAnim2["revive_image_anim"].speed = 1;
                reviveAnim1.Play();
                reviveAnim2.Play();
            }  else
            {
                game_end();
            }
        }
        else
        {
            // смерть без рекламы
            print("dead");
            ad_playing = false;
            fAnim1["game_over_anim"].speed = 1;
            fAnim2["game_over_anim2"].speed = 1;
            fAnim3["game_over_anim3"].speed = 1;
            fAnim4["paused_breath"].speed = 1;
            fAnim4["paused_breath"].time = 0;
            rAnim2["game_over_anim4"].time = 0;
            rAnim2["game_over_anim4"].speed = 1;

            reviveAnim1["game_over_anim"].time = 0;
            reviveAnim1["game_over_anim"].speed = 0;
            reviveAnim2["revive_image_anim"].time = 0;
            reviveAnim2["revive_image_anim"].speed = 0;
            reviveAnim3["game_over_anim"].time = 0;
            reviveAnim3["game_over_anim"].speed = 0;

            rAnim2.Play();
            fAnim1.Play();
            fAnim2.Play();
            fAnim3.Play();
            fAnim4.Play();

            reviveAnim1.Play();
            reviveAnim2.Play();
            reviveAnim3.Play();

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
        adSkipButton.onClick.AddListener(game_end);
        MobileAds.Initialize(initStatus => { });
        RequestReliveRewarded();
    }

    // Update is called once per frame


    void Setup()
    {
        playImage.enabled = false;
        menuImage.enabled = false;
        quitButton.enabled = false;
        restartButton.enabled = false;
        adSkipButton.enabled = false;

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

        rAnim2["game_over_anim4"].time = 0;
        rAnim2["game_over_anim4"].speed = 0;

        reviveAnim1["game_over_anim"].time = 0;
        reviveAnim1["game_over_anim"].speed = 0;

        reviveAnim2["revive_image_anim"].time = 0;
        reviveAnim2["revive_image_anim"].speed = 0;

        reviveAnim3["game_over_anim"].time = 0;
        reviveAnim3["game_over_anim"].speed = 0;

        rAnim2.Play();
        fAnim1.Play();
        fAnim2.Play();
        fAnim3.Play();
        fAnim4.Play();
        rAnim1.Play();
        reviveAnim1.Play();
        reviveAnim2.Play();
        reviveAnim3.Play();

        restart_time = 0;
    }
    void Update()
    {
        if (isQuit && fadeImage.color.a == 1)
        {
            SceneManager.LoadScene(0);
        }
        if (isRestart)
        {
            restart_time += Time.deltaTime;
            if (restart_time > 0.4f)
            {
                player.GetComponent<BallBehave>().Restart();
                isRestart = false;
            }
        }
        
        if (ad_playing)
        {
            ad_time += Time.deltaTime;
        }

        if (ad_playing && reviveAnim2["revive_image_anim"].time >= 1.99f)
        {
            reviveAnim2["revive_image_anim"].time = 1f;
            reviveAnim2.Play();
        }

        if (ad_time > 3 && ad_playing && !decline_enabled)
        {
            print("VIEW BUTTON");
            adSkipButton.enabled = true;
            if (reviveAnim3["game_over_anim"].speed == 0)
            {
                reviveAnim3["game_over_anim"].time = 0;
                reviveAnim3["game_over_anim"].speed = 1;
                reviveAnim3.Play();
                decline_enabled = true;
            }
        }
    }
}
