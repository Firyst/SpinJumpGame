using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviour
{
    public Material[] innerSkins;
    public Material[] outerSkins;

    public MeshRenderer innerSkin;
    public MeshRenderer outerSkin;

    // Start is called before the first frame update
    void Start()
    {
        Reload();
    }
    int get_skin(string field)
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
    public void Reload()
    {
        innerSkin.material = innerSkins[get_skin("IS") - 1];
        outerSkin.material = outerSkins[get_skin("OS") - 1];
    }
    // Update is called once per frame
    void Update()
    {
        

    }
}
