using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviour
{
    public Material[] innerSkins;
    public Material[] outerSkins;

    public MeshRenderer innerSkin;
    public MeshRenderer outerSkin;

    public int innerSkinID;
    public int outerSkinID;

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
        innerSkinID = get_skin("IS");
        outerSkinID = get_skin("OS");
        Redraw();
    }
    public void Redraw()
    {
        innerSkin.material = innerSkins[innerSkinID - 1];
        outerSkin.material = outerSkins[outerSkinID - 1];
    }
    // Update is called once per frame
    void Update()
    {
        

    }
}
