using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUpDown: MonoBehaviour
{
    public GameObject mazepiece;


    //    private void OnMouseUpAsButton()
    //{
    //mazepiece.transform.position += new Vector3(0, 20, 0);
    //    mazepiece.transform.Translate(0f, .1f, 0f);
    // }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mazepiece.transform.Translate(0f, .1f, 0f);
        }
        if (Input.GetMouseButtonDown(1))
        {
            mazepiece.transform.Translate(0f, -.1f, 0f);
        }
    }
}