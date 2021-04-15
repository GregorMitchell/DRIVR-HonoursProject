using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{

    public Transform ballNormalDrop;
    public Transform ballBirdseyeDrop;
    public GameObject ballNormalPrefab;
    public GameObject ballBirdseyePrefab;
    private GameObject ballBirdseye;

    private Animator anim;

    public float speed = 0f;
    public float speedtemp = 0f;
    public float distance = 0f;
    public float distancetemp = 0f;
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

                    speedtemp = float.Parse(serialLine, CultureInfo.InvariantCulture.NumberFormat);
                    distancetemp = 3.16f * speed - 50.5f;

                    Debug.Log("Speed: " + speed);

                    if (distance <= 0)
                    {
                        Debug.Log("Swing too slow");
                    }
                    else
                    {
                        Debug.Log("Distance: " + distance);
                    }

                    anim.SetBool("isHitting", true);
                    StartCoroutine(Swing());
                }

                if (serialLine.Contains("Moved Angle"))
                {
                    serialLine = serialLine.Substring(serialLine.IndexOf(":") + 1);

                    angle = float.Parse(serialLine, CultureInfo.InvariantCulture.NumberFormat);

                    Debug.Log("Angle: " + angle);

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
        speed = speedtemp;
        distance = distancetemp;
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
