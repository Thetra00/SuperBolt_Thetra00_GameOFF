using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{

    public Animator BotAnim;
    
    void Update()
    {
        Vector2 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
      
        BotAnim.SetFloat("DirX", mousePos.x);
        BotAnim.SetFloat("DirY", mousePos.y);
        transform.position = mousePos;
    }
}
