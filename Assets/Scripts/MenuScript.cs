using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button playButton, localButton, shopButton;
    public Button shopLeftButton, shopRightButton, shopLeftButtonO, shopRightButtonO, shopBuyButton, shopBuyButtonO, shopBackButton;
    public Text outerSkinText, innerSkinText, outerSkinPrice, innerSkinPrice, moneyText;
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

    private int innerSkinEnter;
    private int outerSkinEnter;

    private int innerSkinC;
    private int outerSkinC;
    private int currentMoney;

    private int[] skinsDataI;
    private int[] skinsDataO;

    private int[] test = new int[10] { 0, 10, 10, 10, 10, 10, 10, 20, 20, 50 };

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
        shopLeftButtonO.enabled = false;
        shopRightButtonO.enabled = false;
        shopBuyButtonO.enabled = false;

        cameraAnim["cameraAnim"].speed = -1;
        cameraAnim["cameraAnim"].time = 1;

        playButton.enabled = true;
        localButton.enabled = true;
        shopButton.enabled = true;

        fadeAnim2.Play();
        fadeAnim3.Play();

        cameraAnim.Play();
        menuBall.GetComponent<SkinHandler>().Reload();

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
        shopLeftButtonO.enabled = true;
        shopRightButtonO.enabled = true;
        shopBuyButtonO.enabled = true;
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

        innerSkinEnter = get_pref("IS");
        outerSkinEnter = get_pref("OS");
        innerSkin = innerSkinEnter;
        outerSkin = outerSkinEnter;
        Reload();
    }

    void Reload()
    {
        //PlayerPrefs.SetFloat("IS", Mathf.Pow(innerSkin, 0.11764705f));
        //PlayerPrefs.SetFloat("OS", Mathf.Pow(outerSkin, 0.11764705f));
        menuBall.GetComponent<SkinHandler>().innerSkinID = innerSkin;
        menuBall.GetComponent<SkinHandler>().outerSkinID = outerSkin;
        menuBall.GetComponent<SkinHandler>().Redraw();

        moneyText.text = "$" + (currentMoney).ToString();

        // inner
        if (skinsDataI[innerSkin - 1] == 0)
        {
            if (innerSkin == innerSkinEnter)
            {
                innerSkinPrice.text = "Chosen";
            } else
            {
                innerSkinPrice.text = "Choose";
            }
            
        } else
        {
            innerSkinPrice.text = "$" + (skinsDataI[innerSkin - 1].ToString());
        }

        // outer
        if (skinsDataO[outerSkin - 1] == 0)
        {
            if (outerSkin == outerSkinEnter)
            {
                outerSkinPrice.text = "Chosen";
            }
            else
            {
                outerSkinPrice.text = "Choose";
            }

        }
        else
        {
            outerSkinPrice.text = "$" + (skinsDataO[outerSkin - 1].ToString());
        }
    }
    void SelectOuterSkin()
    {
        if (skinsDataO[outerSkin - 1] == 0)
        {
            outerSkinEnter = outerSkin;
            updatePrefs();
        } else
        {
            if (true)
            {
                currentMoney -= skinsDataO[outerSkin - 1];
                skinsDataO[outerSkin - 1] = 0;
                encode_skins_data(skinsDataO, "OSD");
                outerSkinEnter = outerSkin;
                updatePrefs();
            }
        }

        Reload();
    }

    void SelectInnerSkin()
    {
        if (skinsDataI[innerSkin - 1] == 0)
        {
            innerSkinEnter = innerSkin;
            updatePrefs();

        }
        else
        {
            if (true)
            {
                currentMoney -= skinsDataI[innerSkin - 1];
                skinsDataI[innerSkin - 1] = 0;
                encode_skins_data(skinsDataI, "ISD");
                innerSkinEnter = innerSkin;
                updatePrefs();
            }
        }

        Reload();
    }

    void moveLeftI()
    {
        innerSkin -= 1;
        if (innerSkin == 0)
        {
            innerSkin = innerSkinC;
        }
        Reload();
    }
    void moveRightI()
    {
        innerSkin += 1;
        if (innerSkin > innerSkinC)
        {
            innerSkin = 1;
        }
        Reload();
    }
    void moveLeftO()
    {
        outerSkin -= 1;
        if (outerSkin == 0)
        {
            outerSkin = outerSkinC;
        }
        Reload();
    }
    void MoveRightO()
    {
        outerSkin += 1;
        if (outerSkin > outerSkinC)
        {
            outerSkin = 1;
        }
        Reload();
    }
    
    void updatePrefs()
    {
        // обновить значения скинов в player prefs
        PlayerPrefs.SetFloat("IS", Mathf.Pow(innerSkinEnter, 0.11764705f));
        PlayerPrefs.SetFloat("OS", Mathf.Pow(outerSkinEnter, 0.11764705f));
        PlayerPrefs.SetFloat("money", Mathf.Pow(currentMoney, 0.11764705f));
    }
    void Start()
    {
        innerSkinC = menuBall.GetComponent<SkinHandler>().innerSkins.Length;
        outerSkinC = menuBall.GetComponent<SkinHandler>().outerSkins.Length;


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
        shopLeftButton.onClick.AddListener(moveLeftI);
        shopRightButton.onClick.AddListener(moveRightI);
        shopLeftButtonO.onClick.AddListener(moveLeftO);
        shopRightButtonO.onClick.AddListener(MoveRightO);
        shopBuyButton.onClick.AddListener(SelectInnerSkin);
        shopBuyButtonO.onClick.AddListener(SelectOuterSkin);


        fadeAnim3["fade3"].speed = 0;
        fadeAnim3["fade3"].time = 1;

        fadeAnim3.Play();

        shopLeftButton.enabled = false;
        shopRightButton.enabled = false;
        shopBuyButton.enabled = false;

        shopLeftButtonO.enabled = false;
        shopRightButtonO.enabled = false;
        shopBuyButtonO.enabled = false;

        if (decode_skins_data("OSD").Length == 0)
        {
            skinsDataI = new int[23] { 0, 10, 10, 10, 10, 10, 10, 10, 10, 50, 50, 50, 50, 50, 50, 50, 50, 75, 75, 75, 75, 100, 999 };
            encode_skins_data(skinsDataI, "ISD");
            skinsDataO = new int[11] { 0, 50, 50, 50, 50, 100, 100, 150, 100, 100, 300 };
            encode_skins_data(skinsDataO, "OSD");
        }
        skinsDataI = decode_skins_data("ISD");
        skinsDataO = decode_skins_data("OSD");
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
