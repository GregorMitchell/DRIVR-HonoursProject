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
    public float angle = 0f;
    private bool hit = false;

    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        newSceneManagement = player.GetComponent<SceneManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = newSceneManagement.speed;
        angle = newSceneManagement.angle;
        distance = newSceneManagement.distance;

        if (Input.GetButtonUp("Fire1"))
        {
            if (hit == false)
            {
                rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                hit = true;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        speed = 0; //comment out for ball to roll
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        if (hit == true)
        {
            newSceneManagement.SwitchCameras();
        }
    }
}
