using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Globalization;

public class BallBirdseyeMovement : MonoBehaviour
{
    private float angle;

    private SceneManagement newSceneManagement;
    private GameObject player;

    private Animator anim;

    public Transform ballCamera;
    void Start()
    {
        player = GameObject.Find("Player");
        newSceneManagement = player.GetComponent<SceneManagement>();

        anim = GetComponent<Animator>();

        angle = newSceneManagement.angle;
        Debug.Log("Birdseye Angle: " + angle);
    }

    void Update()
    {

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
