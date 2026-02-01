using UnityEngine;

public class AlienAnimController : MonoBehaviour
{
    public Animator animator;

    [Header(
        "0 = Idle\n" +
        "1 = Walk right\n" +
        "2 = Look at painting\n" +
        "3 = Turns around, then switches to Idle\n" +
        "4 = Fail"
    )]
    [Range(0, 4)]
    public int alienAnimInt;

    public enum alienAnimations 
    {
        Idle = 0,
        Walk = 1,
        LookAt = 2,
        Turn = 3,
        Fail = 4
    };

    void Update()
    {
        if (animator != null)
        {
            //animator.SetInteger("AlienAnimInt", alienAnimInt);
        }
    }

    public void PlayAnimation(int animInt)
    {
        if (animator != null)
        {
            Debug.Log("Playing alien animation: " + animInt);
            animator.SetInteger("AlienAnimInt", animInt);
        }
    }
}

