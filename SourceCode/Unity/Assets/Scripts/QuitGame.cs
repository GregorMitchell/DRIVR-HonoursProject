using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    //if the button is clicked on, the game will quit to desktop
    private void OnMouseDown()
    {
        Application.Quit();
    }
}
