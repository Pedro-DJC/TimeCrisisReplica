//using UnityEditor.UI;
using UnityEngine;

public class TrialAnimation : MonoBehaviour
{
    //Privates
    private Animator animator;

    public bool standing;

    void Start()
    {
        animator = GetComponent<Animator>();
        Stand();
        standing = false;
    }
    public void Dead() //Telling is dead
    {
        animator.SetTrigger("IsDead");
    }

    public void Run() //Telling to walk
    {
        animator.SetTrigger("IsRunning");
    }

    public void Stand() //Telling to stand
    {
        animator.SetTrigger("IsStanding");
    }

    public void Crouch() //Telling is crouching
    {
        animator.SetTrigger("IsCrouching");
    }

    public void Shoot() //Telling is Shooting
    {
        animator.SetTrigger("IsFiring");
    }

    public void CoverShield()
    {
        animator.SetTrigger("Cover");
    }
    public void UncoverShield()
    {
        animator.SetTrigger("Uncover");
    }
}
