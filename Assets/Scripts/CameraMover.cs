using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> checkpoints;
    public Vector2 targetPos;
    public int currentPoint = 0;
    public bool moving = false;
    public InputAction nextButton;
    void Start()
    {
        nextButton = InputSystem.actions.FindAction("Up", true);
    }
    

    void nextPoint()
    {
        if (currentPoint < checkpoints.Count-1 )
        {
            
            targetPos = checkpoints[currentPoint].transform.position;
            currentPoint++;
            moving = true;
        }
        else if (currentPoint == checkpoints.Count-1)
        {
            currentPoint = 0;
            transform.position = new Vector2(0,0);
            targetPos = checkpoints[currentPoint].transform.position;
            moving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nextButton.WasPressedThisFrame() && !moving)
        {
            nextPoint();
        }
    }
    void FixedUpdate()
    {
        
        if (moving && Vector2.Distance(transform.position, targetPos) > 0.1)
        {
            transform.position = Vector2.MoveTowards(transform.position,targetPos,Time.fixedDeltaTime*10);
        }else if (moving &&Vector2.Distance(transform.position, targetPos) <= 0.1) {
            transform.position = targetPos;
            moving = false;
        }
        
    }
}
