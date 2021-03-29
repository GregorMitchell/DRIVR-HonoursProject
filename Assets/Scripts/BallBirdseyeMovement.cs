using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Globalization;

public class BallBirdseyeMovement : MonoBehaviour
{
    private float angle;

    private GameObject normalBall;
    private BallNormalMovement newBallNormalMovement;

    private Animator anim;

    public Transform ballCamera;
    void Start()
    {
        normalBall = GameObject.Find("BallNormal(Clone)");
        newBallNormalMovement = normalBall.GetComponent<BallNormalMovement>();

        anim = GetComponent<Animator>();

        angle = newBallNormalMovement.angle;
        Debug.Log("Birdseye Angle: " + angle);

        Destroy(normalBall);
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

        if (Input.GetButtonUp("Fire2"))
        {
            Destroy(gameObject);
        }
    }
}
