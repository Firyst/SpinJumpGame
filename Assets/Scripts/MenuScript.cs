using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button playButton, localButton, shopButton;
    public Button shopLeftButton, shopRightButton, shopBuyButton, shopBackButton;
    public Text outerSkinText, innerSkinText, moneyText;
    public Image fader;
    public Color curColor;
    public Animation fadeAnim;
    public Animation fadeAnim2;
    public Animation fadeAnim3;
    public Animation cameraAnim;
    public Text title;
    public GameObject menuBall;


    private bool shop = false;
    private int innerSkin;
    private int outerSkin;
    private int innerSkinC;
    private int outerSkinC;
    private int currentMoney;

    private int[] skinsData;

    private int[] test = new int[10] { 10, 10, 10, 10, 10, 10, 10, 20, 20, 50 };

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
    // Start is called before the first frame update

    string rjust(string input_str, int length, string symbol)
    {
        while (input_str.Length < length)
        {
            input_str = symbol + input_str;
        }
        return input_str;
    }

    int[] decode_skins_data(string field_name)
    {
        string raw_data = PlayerPrefs.GetString(field_name); // Получаем исходную строку
        int[] parsed_data = new int[raw_data.Length / 16]; // разделено по 16 символов

        for (int i=0; i<raw_data.Length;i+=16)
        {
            parsed_data.SetValue((int.Parse(raw_data.Substring(i, 16)) / 85), i / 16);
        }
        return parsed_data;
    }
    void encode_skins_data(int[] data, string field)
    {
        string write_data = "";

        for (int i=0; i<data.Length; i++)
        {
            write_data = write_data + (rjust((data[i] * 85).ToString(), 16, "0"));
        }
        PlayerPrefs.SetString(field, write_data);
        print(write_data);
    }
    void go_back()
    {
        // выход из магазинчика 
        shop = false;
        title.text = "XDJump";
        fadeAnim3["fade3"].speed = 1;
        fadeAnim3["fade3"].time = 0;
        fadeAnim2["fade2"].speed = -1;
        fadeAnim2["fade2"].time = 1;
        shopLeftButton.enabled = false;
        shopRightButton.enabled = false;
        shopBuyButton.enabled = false;
        shopBackButton.enabled = false;

        cameraAnim["cameraAnim"].speed = -1;
        cameraAnim["cameraAnim"].time = 1;

        playButton.enabled = true;
        localButton.enabled = true;
        shopButton.enabled = true;

        fadeAnim2.Play();
        fadeAnim3.Play();

        cameraAnim.Play();

    }
    void start_game()
    {
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
        shopBackButton.enabled = true;

        fadeAnim3["fade3"].speed = -1;
        fadeAnim3["fade3"].time = 1;
        fadeAnim2["fade2"].speed = 1;
        fadeAnim2["fade2"].time = 0;
        fadeAnim2.Play();
        fadeAnim3.Play();

        cameraAnim["cameraAnim"].speed = 1;
        cameraAnim["cameraAnim"].time = 0;
        cameraAnim.Play();
    }

    void moveLeftI()
    {
        innerSkin -= 1;
        if (innerSkin == 0)
        {
            innerSkin = innerSkinC;
        }
        menuBall.GetComponent<SkinHandler>().Reload();
    }
    void moveRightI()
    {
        innerSkin += 1;
        if (innerSkin > innerSkinC)
        {
            innerSkin = 1;
        }
        menuBall.GetComponent<SkinHandler>().Reload();
    }

    void moveLeftO()
    {
        outerSkin -= 1;
        if (outerSkin == 0)
        {
            outerSkin = outerSkinC;
        }
        menuBall.GetComponent<SkinHandler>().Reload();
    }

    void MoveRightO()
    {
        outerSkin += 1;
        if (outerSkin > outerSkinC)
        {
            outerSkin = 1;
        }
    }
    void updatePrefs()
    {
        // обновить значения скинов в player prefs
        PlayerPrefs.SetFloat("IS", Mathf.Pow(innerSkin, 0.11764705f));
        PlayerPrefs.SetFloat("OS", Mathf.Pow(outerSkin, 0.11764705f));
    }
    void Start()
    {
        innerSkinC = menuBall.GetComponent<SkinHandler>().innerSkins.Length;
        outerSkinC = menuBall.GetComponent<SkinHandler>().outerSkins.Length;

        encode_skins_data(test, "debug");

        currentMoney = get_pref("money");
        moneyText.text = "$" + (currentMoney).ToString();

        if (get_pref("OS") == 0 || get_pref("IS") == 0)
        {
            // если нет записей о скинах, или они сломаны, то сбрасываем на дефолтные
            innerSkin = 1;
            outerSkin = 1;
            updatePrefs();
        }
        innerSkin = get_pref("IS");
        outerSkin = get_pref("OS");
        shop = false;

        playButton.onClick.AddListener(start_game);
        shopButton.onClick.AddListener(visit_shop);
        shopBackButton.onClick.AddListener(go_back);

        fadeAnim3["fade3"].speed = 0;
        fadeAnim3["fade3"].time = 1;

        fadeAnim3.Play();

        shopLeftButton.enabled = false;
        shopRightButton.enabled = false;
        shopBuyButton.enabled = false;

        int[] xd = decode_skins_data("debug");
        for (int i = 0; i < xd.Length; i++)
        {
            print(xd[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        innerSkinText.text = (innerSkin).ToString() + "/" + (innerSkinC).ToString();
        outerSkinText.text = (outerSkin).ToString() + "/" + (outerSkinC).ToString();
        if (fader.color.a > 0.99f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
