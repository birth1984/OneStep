using UnityEngine;
using System.Collections;

public class CameraAnimator : StateMachineBehaviour
{

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.IsName("Idle") && !animator.GetBool("isIdle"))
        {
            animator.SetBool("isIdle", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!stateInfo.IsName("Idle"))
        {
            CameraManager.getInstance().setCameraFinalPos();
            CameraManager.getInstance().setCameraLookPos();
        }
    }


}
