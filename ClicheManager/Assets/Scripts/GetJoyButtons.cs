using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetJoyButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            Debug.Log("Jump = joystick button 3");
        if (Input.GetButtonDown("Fire1"))
            Debug.Log("Fire1 = joystick button 0");
        if (Input.GetButtonDown("Fire2"))
            Debug.Log("Fire2 = joystick button 1");
        if (Input.GetButtonDown("Fire3"))
            Debug.Log("Fire3 = joystick button 2");
       


    }
}
