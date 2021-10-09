using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class BallBehave : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = -0.15f;
    public Transform platforms;
    public Transform cameraPos;
    public Text score_text;
    public Text combo_text;
    public LayerMask plLayer;
    public LayerMask dhLayer;
    public float colR;
    public float maxSpeed = 20;
    public float dec;
    private float yvel;
    public GameObject handler;
    private float clkPos;
    private int last_plat = -20;
    private int combo = 1;
    public GameObject gameUI;
    public bool paused;
    public bool sideCollided = false;
    public bool allowCameraMove = true;
    public Animation anim;


    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Application.targetFrameRate = 60;
        rb.velocity = new Vector3(0, -10f, 0);
        //anim["jump"].speed = 0;
        //anim["jump"].time = 0;
        //anim["death"].speed = 0;
        //anim["death"].time = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ScorePlus()
    {
        // счетчик пройденных платформ
        score_text.text = (int.Parse(score_text.text) + 1 * combo).ToString();
        combo_text.text = "x" + (combo).ToString();
        combo_text.color = new UnityEngine.Color(Mathf.Min(0.196f + (combo - 1) * 0.25f, 1), 0.196f, 0.196f);
        if (combo > 2)
        {
            combo_text.gameObject.GetComponent<Animation>().enabled = true;
        }
        combo += 1;
        last_plat -= 10;
        allowCameraMove = true;
    }


    public void Restart()
    {

        
        combo = 1;
        score_text.text = "0";
        rb.velocity = new Vector3(0, 0, 0);
        float camOffset = rb.position.y - cameraPos.position.y;
        
        rb.position = new Vector3(0, rb.position.y - last_plat + Mathf.Max(-40, last_plat), -2.5f);
        cameraPos.position = new Vector3(cameraPos.position.x, cameraPos.position.y - last_plat + Mathf.Max(-40, last_plat), cameraPos.position.z);
        // Debug.Log("reset");
        last_plat = -20;

        platforms.gameObject.GetComponent<PlatBehave>().Restart();
        cameraPos.gameObject.GetComponent<CameraBehave>().StartAnimaton();
    }
    public void Death()
    {


        gameUI.GetComponent<GameUIScript>().game_end();

        /*SceneManager.LoadScene(0);*/
    }


    private void FixedUpdate()
    {
        paused = gameUI.GetComponent<GameUIScript>().paused;

        if (!paused)
        {
            if (yvel != 0)
            {
                rb.velocity = new Vector3(0, yvel, 0);
                yvel = 0;
            }
            
            Vector3 gndVec = new Vector3(rb.position.x, rb.position.y - 0.25f, rb.position.z);
            Collider[] gnd = Physics.OverlapSphere(gndVec, colR-0.25f, plLayer);
            Vector3 deathVec = new Vector3(rb.position.x, rb.position.y-0.25f, rb.position.z);
            Collider[] death = Physics.OverlapSphere(deathVec, colR-0.25f, dhLayer);

            Collider[] sideCollider = Physics.OverlapSphere(rb.position, colR+0.1f, plLayer);


            /*Debug.Log(rb.velocity.y);*/
            speed = speed + dec;

            if (death.Length != 0) // детектор смерти
            {
                Death();
            }


            // касание платформы
            if (gnd.Length != 0 && rb.velocity.y <= 0)
            {
                anim.Play();
                speed = 0;
                rb.velocity = new Vector3(rb.velocity.x, 7, rb.velocity.z);
                combo = 1;
                combo_text.text = "x" + (combo).ToString();
                combo_text.color = new Color(0.196f, 0.196f, 0.196f);
                combo_text.gameObject.GetComponent<Animation>().enabled = false;
            }

            // ограничитель скорости
            if (rb.velocity.y + speed < maxSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxSpeed, rb.velocity.z);
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + speed, rb.velocity.z);
            }


            // счетчик очков
            if (rb.position.y < last_plat - 0.3f)
            {
                ScorePlus();
            }

            // управление
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                if (clkPos == -1)
                {
                    clkPos = mousePos.x;
                }
                else
                {
                    if ((sideCollider.Length == 0 || gnd.Length != 0) && death.Length == 0)
                    {
                        platforms.eulerAngles = new Vector3(0, platforms.eulerAngles.y + (clkPos - mousePos.x) / 4, 0);
                        clkPos = mousePos.x;
                    } else
                    {
                        clkPos = mousePos.x;
                    }
                    
                }

            }
            else
            {
                clkPos = -1;
            }

            if ((cameraPos.position.y - rb.position.y > 4) && allowCameraMove) // камера
            {
                cameraPos.position = new Vector3(cameraPos.position.x, rb.position.y + 4, cameraPos.position.z);
            }
        } else
        {
            if (yvel == 0)
            {
                yvel = rb.velocity.y;
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
        sideCollided = false;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("col");
    }*/
}
