using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Globalization;
using System.IO.Ports;

public class BallNormalMovement : MonoBehaviour
{
    //public variable declaration
    public float jumpSpeed = 8f;

    //private variable declaration
    private SceneManagement newSceneManagement;
    private GameObject player;
    private float speed = 0f;
    private bool hit = false;
    private Rigidbody2D rigidBody;

    void Start()
    {
        //gets child rigidbody to reference later
        rigidBody = GetComponent<Rigidbody2D>();

        //gets scene management script to use later
        player = GameObject.Find("Player");
        newSceneManagement = player.GetComponent<SceneManagement>();
    }

    void Update()
    {
        //gets speed from scene management script (this is in update as the speed is not set when the ball spawns in, meaning it is consatly updated)
        speed = newSceneManagement.speed;

        //once the speed has been set and the ball hasn't been hit the ball will move
        if (hit == false && speed > 0)
        {
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
            hit = true;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //stops the ball from rolling forever after landing
        speed = 0;
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        //if the ball has been hit and lands then start the camera switch coroutine
        if (hit == true)
        {
            StartCoroutine(CameraSwitch());
        }
    }

    IEnumerator CameraSwitch()
    {
        //wait 1 second and run SwitchCameras() from scene management
        yield return new WaitForSeconds(1);
        newSceneManagement.SwitchCameras();
    }
}
