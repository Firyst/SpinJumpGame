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
        
    }
    int get_skin(string field)
    {
        float highscore = Mathf.Pow(PlayerPrefs.GetFloat(field), 8.5f);
        if (Mathf.Abs(highscore - Mathf.RoundToInt(highscore)) < 0.001f)
        {
            return Mathf.RoundToInt(highscore);
        }
        else
        {
            return 0;
        }
    }
    public void Reload()
    {

    }
    // Update is called once per frame
    void Update()
    {
        innerSkin.material = innerSkins[get_skin("IS")];
        outerSkin.material = outerSkins[get_skin("OS")];
    }
}
