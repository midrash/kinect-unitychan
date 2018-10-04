using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class endgame : StateMachineBehaviour
{

    private Text text_bubble;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("motion_unitychan", 0);
        animator.SetInteger("AnigameResult", 0);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("text_bubble").transform.GetChild(0).gameObject.SetActive(true);
        text_bubble = unitychanAni.out_text;
        text_bubble.text = "생각중";
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("text_bubble").transform.GetChild(0).gameObject.SetActive(false);
    }
}


