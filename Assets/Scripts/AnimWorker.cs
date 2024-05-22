using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimWorker : MonoBehaviour
{
    private Animator animator;
    public Mina mina;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mina = FindObjectOfType<Mina>(); // This will find an instance of Mina in the scene. Make sure there's only one, or adjust accordingly.
    }

    // Update is called once per frame
    void Update()
    {
        if(mina != null && mina.dentroDeColision)
        {
            animator.SetBool("IsWorking", true);
        }
        else
        {
            animator.SetBool("IsWorking", false);
        }
    }
}
