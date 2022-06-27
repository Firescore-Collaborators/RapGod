using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestAnimationManager : MonoBehaviour
{

    private Animator animator;

    public bool isFemale;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // --- On Chair Animations --- //
    public void PlayIdleSit()
    {
        animator.Play("idle_sit");
    }

    public void PlayCrySit1()
    {
        animator.Play("cry_sit1");
    }

    public void PlayCrySit2()
    {
        animator.Play("cry_sit2");
    }

    public void PlayLaughSit1()
    {
        animator.Play("laugh_sit1");
    }

    public void PlayLaughSit2()
    {
        animator.Play("laugh_sit2");
    }

    public void PlayTalkSit1()
    {
        animator.Play("talk_sit1");
    }

    public void PlayTalkSit2()
    {
        animator.Play("talk_sit2");
    }

    // --- Standing Animations --- //

    public void PlayAngry1()
    {
        animator.Play("angry1");
    }
    public void PlayAngry2()
    {
        animator.Play("angry2");
    }
    public void PlayCry1()
    {
        animator.Play("cry1");
    }
    public void PlayCry2()
    {
        animator.Play("cry2");
    }
    public void PlayHappy1()
    {
        animator.Play("happy1");
    }
    public void PlayHappy2()
    {
        animator.Play("happy2");
    }
    public void PlayLaugh1()
    {
        animator.Play("laugh1");
    }
    public void PlayLaugh2()
    {
        animator.Play("laugh2");
    }
    
    public void PlaySayNo1()
    {
        animator.Play("say_no1");
    }
    public void PlaySayNo2()
    {
        animator.Play("say_no2");
    }
    public void PlaySayYes1()
    {
        animator.Play("say_yes1");
    }
    public void PlaySayYes2()
    {
        animator.Play("say_yes1");
    }
    public void PlayTalkStand1()
    {
        animator.Play("talk_stand1");
    }
    public void PlayTalkStand2()
    {
        animator.Play("talk_stand2");
    }

    public void PlaySitCombo1() {
        animator.Play("combo1_cry_sit1");
    }

    public void PlaySitCombo2()
    {
        animator.Play("combo2_laugh_sit1");
    }

    public void PlayIDCardFemaleCombo()
    {
        if (Random.Range(0, 2) == 0)
            animator.Play("IDcardCombo1_say_no1");
        else
            animator.Play("IDcardCombo3_laugh1");

    }
    public void PlayIDCardMaleCombo()
    {
         if (Random.Range(0, 2) == 0)
            animator.Play("IDcardCombo2_say_no2");
        else
            animator.Play("IDcardCombo4_laugh2");
    }

    public void PlayItemCheckCombo()
    {
        if (Random.Range(0, 2) == 0)
            animator.Play("ItemCheckcardCombo1");
        else
            animator.Play("ItemCheckcardCombo2");
    }

    public void PlayRandomHappyAnim()
    {
        if(Random.Range(0, 2) == 0)
            PlayHappy1();
        else
            PlayHappy2();

    }
}
