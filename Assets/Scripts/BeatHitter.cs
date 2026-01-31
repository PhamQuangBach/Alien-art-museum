using UnityEngine;
using UnityEngine.InputSystem;

public class BeatHitter : MonoBehaviour
{
    private beat beatScript;
    private AudioManager audioManager;

    private InputAction[] inputActions = new InputAction[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatScript = GameObject.Find("beat").GetComponent<beat>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        inputActions[0] = InputSystem.actions.FindAction("Up", true);
        inputActions[1] = InputSystem.actions.FindAction("Right", true);
        inputActions[2] = InputSystem.actions.FindAction("Down", true);
        inputActions[3] = InputSystem.actions.FindAction("Left", true);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (inputActions[i].WasPerformedThisFrame())
            {
                InputPressed(i);
            }
        }
    }

    public void InputPressed(int input)
    {
        Debug.Log("input pressed!");

        //if hit on beat
        if (beatScript.HitBeat(input))
        {
            print("Good Hit");
        }
        else
        {
            print("Bad Hit");
        }
        MakeSound(input);
    }

    private void MakeSound(int input)
    {
        audioManager.PlaySound(beatScript.beatSounds[input], 1, 1.2f);
    }
}
