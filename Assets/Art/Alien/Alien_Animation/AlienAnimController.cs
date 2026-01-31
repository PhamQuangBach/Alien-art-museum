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

    void Update()
    {
        if (animator != null)
        {
            animator.SetInteger("AlienAnimInt", alienAnimInt);
        }
    }
}

