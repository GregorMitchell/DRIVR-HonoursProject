using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using UnityEngine;
using TMPro;

public class SceneManagement : MonoBehaviour
{
    //public variable declaration
    public Transform ballNormalDrop;
    public Transform ballBirdseyeDrop;
    public GameObject ballNormalPrefab;
    public GameObject ballBirdseyePrefab;


    public TextMeshProUGUI speedText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI warningText;

    public float speed = 0f;
    public float speedTemp = 0f;
    public float distance = 0f;
    public float distanceTemp = 0f;
    public float angle = 0f;

    //private variable declaration
    private Animator anim;
    private GameObject ballBirdseye;
    private bool buttonCanBePressed = true;
    private string serialLine;

    //this is how the program connects with the device
    SerialPort sp = new SerialPort("COM3", 115200);

    void Start()
    {
        //open the serial line for communication
        sp.Open();
        sp.ReadTimeout = 1;

        //get child animator to reference later
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //warning message to user, can dissapear with the space bar
        if (Input.GetButtonUp("Jump"))
        {
            warningText.enabled = false;
        }

        //all serial communication in here
        if (sp.IsOpen)
        {
            try
            {
                //reads in serial line
                serialLine = sp.ReadLine();

                //if line contains "Button Pressed" then drops ball and stops more from spawning
                if (serialLine.Contains("Button Pressed"))
                {
                    if (buttonCanBePressed == true)
                    {
                        speed = 0;
                        Instantiate(ballNormalPrefab, ballNormalDrop.position, ballNormalDrop.rotation);
                        buttonCanBePressed = false;
                    }
                }

                //if line contains "Speed(mph)" then parses to get the value, updates the HUD, calculates the distance and updates the HUD
                //'BallNormalMovement' uses this value to calculate movement
                //also starts the swing animation coroutine
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

                //if line contains "Moved Angle" then parses to get the value, updates the HUD
                //'BallBirdseyeMovement' uses this value to calculate movement
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

        //if the ballBirdseye has stopped its movement then start coroutine
        if (ballBirdseye)
        {
            if (ballBirdseye.transform.position.y == 61.5)
            {
                StartCoroutine(DestroyBirdseye());
            }
        }
    }

    //wait 1 second then assign the variable, meaning the ball will move
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(1);
        speed = speedTemp;
        distance = distanceTemp;
    }

    //wait 1 second, destroy birdseyeBall then reset
    IEnumerator DestroyBirdseye()
    {
        yield return new WaitForSeconds(1);
        Destroy(ballBirdseye);
        buttonCanBePressed = true;
        anim.SetBool("isHitting", false);
    }

    //switch cameras from ballNormal to ballBirdseye
    public void SwitchCameras()
    {
        Instantiate(ballBirdseyePrefab, ballBirdseyeDrop.position, ballBirdseyeDrop.rotation);
        ballBirdseye = GameObject.Find("BallBirdseye(Clone)");
    }
}
