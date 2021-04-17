using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using UnityEngine;
using TMPro;

public class SceneManagement : MonoBehaviour
{

    public Transform ballNormalDrop;
    public Transform ballBirdseyeDrop;
    public GameObject ballNormalPrefab;
    public GameObject ballBirdseyePrefab;
    private GameObject ballBirdseye;


    public TextMeshProUGUI speedText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI angleText;

    private Animator anim;

    public float speed = 0f;
    public float speedTemp = 0f;
    public float distance = 0f;
    public float distanceTemp = 0f;
    public float angle = 0f;

    private bool buttonCanBePressed = true;

    private string serialLine;

    SerialPort sp = new SerialPort("COM3", 115200);


    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                serialLine = sp.ReadLine();

                if (serialLine.Contains("Button Pressed"))
                {
                    if (buttonCanBePressed == true)
                    {
                        DropBall();
                        buttonCanBePressed = false;
                    }
                }

                if (serialLine.Contains("Speed(mph)"))
                {
                    serialLine = serialLine.Substring(serialLine.IndexOf(":") + 1);

                    speedTemp = float.Parse(serialLine, CultureInfo.InvariantCulture.NumberFormat);
                    distanceTemp = 3.16f * speed - 50.5f;

                    speedText.text = "Speed: " + speedTemp.ToString() + "mph";

                    if (distance <= 0)
                    {
                        distanceText.text = "Distance: too slow";
                    }
                    else
                    {
                        distanceText.text = "Distance: " + distanceTemp.ToString() + "yards";
                    }

                    anim.SetBool("isHitting", true);
                    StartCoroutine(Swing());
                }

                if (serialLine.Contains("Moved Angle"))
                {
                    serialLine = serialLine.Substring(serialLine.IndexOf(":") + 1);

                    angle = float.Parse(serialLine, CultureInfo.InvariantCulture.NumberFormat);

                    angleText.text = "Angle: " + angle.ToString() + "°";

                }

            }
            catch (System.Exception)
            {

            }
        }

        if (ballBirdseye)
        {
            if (ballBirdseye.transform.position.y == 61.5)
            {
                StartCoroutine(DestroyBirdseye());
            }
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(1);
        speed = speedTemp;
        distance = distanceTemp;
    }

    IEnumerator DestroyBirdseye()
    {
        yield return new WaitForSeconds(1);
        Destroy(ballBirdseye);
        buttonCanBePressed = true;
        anim.SetBool("isHitting", false);
    }

    void DropBall()
    {
        speed = 0;
        Instantiate(ballNormalPrefab, ballNormalDrop.position, ballNormalDrop.rotation);
    }

    public void SwitchCameras()
    {
        Instantiate(ballBirdseyePrefab, ballBirdseyeDrop.position, ballBirdseyeDrop.rotation);
        ballBirdseye = GameObject.Find("BallBirdseye(Clone)");
    }
}
