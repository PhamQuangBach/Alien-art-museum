using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject cameraController;
    public GameObject Happy;
    public GameObject Surprise;
    public GameObject Angry;
    public GameObject Sad;
    public InputAction Up;
    public InputAction Down;
    public InputAction Left;
    public InputAction Right;
    public bool HappyDone = false;
    public bool SurpriseDone = false;
    public bool AngryDone = false;
    public bool SadDone = false;

    void Start()
    {
        
        Up = InputSystem.actions.FindAction("Up", true);
        Down = InputSystem.actions.FindAction("Down", true);
        Right = InputSystem.actions.FindAction("Right", true);
        Left = InputSystem.actions.FindAction("Left", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraController.GetComponent<CameraMover>().currentPoint >0 && !cameraController.GetComponent<CameraMover>().moving)
        {
            if(Up.WasPerformedThisFrame())
            {
                Happy.GetComponent<Emotion>().activateEmotion();
                HappyDone = true;
            }
            if(Down.WasPerformedThisFrame())
            {
                Sad.GetComponent<Emotion>().activateEmotion();
                SadDone = true;
            }
            if(Left.WasPerformedThisFrame())
            {
                Angry.GetComponent<Emotion>().activateEmotion();
                AngryDone = true;
            }
            if(Right.WasPerformedThisFrame())
            {
                Surprise.GetComponent<Emotion>().activateEmotion();
                SurpriseDone = true;
            }
            if (HappyDone&&SadDone&&AngryDone&&SurpriseDone)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<OnlineMode>().loadGame();
            }
        }
            
    }
}
