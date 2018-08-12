using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animator anim;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) && !anim.IsInTransition(0))
        {
            anim.StopPlayback();
            Destroy(this.gameObject);
        }
	}
}
