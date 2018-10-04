using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class unitychanAni : MonoBehaviour
{

    Animator animator;
    int motion_avatar;
    int motion_unitychan = 0;
    int gameresult = 0;

    public Text text_bubble;
    public static Text out_text;

    bool gamemode = false;

    void Awake()
    {
        animator = GetComponent<Animator>(); //애니메이터 생성
        Debug.Log("start Unitychang");

    }

    // Update is called once per frame
    void Update()
    {
        out_text = text_bubble;
        motion_avatar = PoseDetectorScript.avatar; // 싱글턴과 public static으로 가져오는것의 차이?

        if (motion_avatar == 1)
        {
            gamemode = true;
            animator.SetBool("gameOn", true);
        }
        else if (motion_avatar == 2)
        {
            gamemode = false;
            animator.SetBool("gameOn", false);
        }

        if (gamemode == true)
        {
            GameAnimationUpdate();
        }
        else if (gamemode == false)
        {
            AnimationUpdate();
        }
    }

    private void GameAnimationUpdate()
    {
        if (motion_avatar != 0)
        {
            //Debug.Log("AnigameResult : " + animator.GetInteger("AnigameResult") + "     motion_unitychan" + animator.GetInteger("motion_unitychan"));
            if (animator.GetInteger("AnigameResult") == 0 && animator.GetInteger("motion_unitychan") == 0)
            {
                //Debug.Log("AnigameResult : " + gameresult + "      motion_unitychan : " + motion_unitychan);
                if (gameresult == 0 && motion_unitychan == 0)
                {

                    motion_unitychan = Random.Range(3, 6);
                    gameresult = Game.Debedegame(motion_avatar, motion_unitychan);
                    
                    animator.SetInteger("motion_unitychan", motion_unitychan);
                   
                    animator.SetInteger("AnigameResult", gameresult);
                    Debug.Log("motion_avatar : " + motion_avatar + " , motion_unitychan : " + motion_unitychan + " AnigameResult : " + gameresult);
                }
              
            }
           
        }
        
        gameresult = 0;
        motion_unitychan = 0;
        motion_avatar = 0;
    }
    private void AnimationUpdate()
    {

        //Debug.Log(motion_avatar);
        switch (motion_avatar)
        {
            case 0: // idle
                animator.SetInteger("motion_unitychan", 0);
                break;
            case 1: // sun
                animator.SetInteger("motion_unitychan", 1);

                break;
            case 2: // hi
                animator.SetInteger("motion_unitychan", 2);
                break;
        }

    }

}
