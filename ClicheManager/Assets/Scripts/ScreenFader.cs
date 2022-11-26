using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;
    private void Awake()
    {
        instance = this;
    }

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOFF()
    {
        animator.SetBool("IsBlack", false);
    }

        public void FadeON()
    {
        animator.SetBool("IsBlack", true);
    }

  
}
