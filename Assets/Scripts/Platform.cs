using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed = 1f;

    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        //speed = 5f;

        // set animation speed
        animator.SetFloat("Speed", speed);

        // randomize platform position
        animator.Play("Platform", 0, Random.Range(0f ,1f));
    }
    
}
