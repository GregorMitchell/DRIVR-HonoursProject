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

    public float speed = 0f;
    public float distance = 0f;
    public float angle = 0f;

    private bool buttonCanBePressed = true;

    private string serialLine;

    SerialPort sp = new SerialPort("COM3", 115200);


    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
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
                        Swing();
                        buttonCanBePressed = false;
                    }
                }

                if (serialLine.Contains("Speed(mph)"))
                {
                    serialLine = serialLine.Substring(serialLine.IndexOf(":") + 1);

                    speed = float.Parse(serialLine, CultureInfo.InvariantCulture.NumberFormat);

                    distance = 3.16f * speed - 50.5f;

                    Debug.Log("Speed: " + speed);

                    if (distance <= 0)
                    {
                        Debug.Log("Swing too slow");
                    }
                    else
                    {
                        Debug.Log("Distance: " + distance);
                    }
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
    IEnumerator DestroyBirdseye()
    {
        yield return new WaitForSeconds(1);
        Destroy(ballBirdseye);
        buttonCanBePressed = true;
    }

    void Swing()
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
