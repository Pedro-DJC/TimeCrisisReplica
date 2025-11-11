using UnityEditor.UI;
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
        animator.SetBool("IsDead", true);
        animator.SetBool("IsRunnning", false);
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsFiring", false);
        animator.SetBool("Standup", false);

        standing = false;
    }

    public void Run() //Telling to walk
    {
        animator.SetBool("IsDead", false);
        animator.SetBool("IsRunnning", true);
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsFiring", false);
        animator.SetBool("Standup", false);

        standing = false;
    }

    public void Stand() //Telling to stand
    {
        animator.SetBool("IsDead", false);
        animator.SetBool("IsRunnning", false);
        animator.SetBool("IsStanding", true);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsFiring", false);
        animator.SetBool("Standup", false);

        standing = true;
    }

    public void Crouch() //Telling is crouching
    {
        animator.SetBool("IsDead", false);
        animator.SetBool("IsRunnning", false);
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsCrouching", true);
        animator.SetBool("IsFiring", false);
        animator.SetBool("Standup", false);

        standing = true;
    }

    public void Shoot() //Telling is Shooting
    {
        animator.SetBool("IsDead", false);
        animator.SetBool("IsRunnning", false);
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsFiring", true);
        animator.SetBool("Standup", false);

        standing = true;
    }
}
