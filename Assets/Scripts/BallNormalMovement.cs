using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Globalization;
using System.IO.Ports;

public class BallNormalMovement : MonoBehaviour
{
    public float jumpSpeed = 8f;

    private SceneManagement newSceneManagement;
    private GameObject player;
    private float speed = 0f;
    private float distance = 0f;
    private bool hit = false;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        newSceneManagement = player.GetComponent<SceneManagement>();
    }

    void Update()
    {
        speed = newSceneManagement.speed;
        distance = newSceneManagement.distance;

        if (hit == false && speed > 0)
        {
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
            hit = true;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        speed = 0; //comment out for ball to roll
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        if (hit == true)
        {
            StartCoroutine(CameraSwitch());
        }
    }

    IEnumerator CameraSwitch()
    {
        yield return new WaitForSeconds(1);
        newSceneManagement.SwitchCameras();
    }
}
