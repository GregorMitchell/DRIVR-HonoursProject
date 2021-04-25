using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Globalization;

public class BallBirdseyeMovement : MonoBehaviour
{
    //private variable declaration
    private float angle;
    private SceneManagement newSceneManagement;
    private GameObject player;
    private Animator anim;

    //public variable declaration
    public Transform ballCamera;
    void Start()
    {
        //receivs the angle from the scene management script and assigns it to angle
        player = GameObject.Find("Player");
        newSceneManagement = player.GetComponent<SceneManagement>();
        angle = newSceneManagement.angle;

        //get child animator to reference later
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //depending on the angle, various different animations will be played
        if (angle >= -10 && angle <= 10)
        {
            anim.SetBool("isStraight", true);
        }
        else if (angle > 10 && angle <= 30)
        {
            anim.SetBool("isSlightLeft", true);
        }
        else if (angle > 30)
        {
            anim.SetBool("isFarLeft", true);
        }
        else if (angle < -10 && angle >= -30)
        {
            anim.SetBool("isSlightRight", true);
        }
        else if (angle < -30)
        {
            anim.SetBool("isFarRight", true);
        }
    }
}
