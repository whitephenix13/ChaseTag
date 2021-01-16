using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehaviour : StateMachineBehaviour
{
    private Vector3 lastRelativeWalkDirection = Vector3.forward;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if the avatar is standing up
        animator.SetBool("canSlide", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FaceLastWalkDirection(lastRelativeWalkDirection, animator);
        //Desactivate slide as you need to wait for the character to be fully standing up 
        animator.SetBool("canSlide", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 relativeWalkDirection = new Vector3(animator.GetFloat("catXSpeed"), 0, animator.GetFloat("catZSpeed"));
        if (!relativeWalkDirection.Equals(Vector3.forward))
        {
            FaceLastWalkDirection(relativeWalkDirection,animator);
            lastRelativeWalkDirection = Vector3.forward;
        }

    }

    private void FaceLastWalkDirection(Vector3 relativeWalkDirection, Animator animator) {
        //Rotate the object based on its lastRelativeWalkDirection so that it faces z axis (towards postive values)
        //Example: if the avatar faces z position (0,0,1), the avatar should turn 90° to the right 

        //Get the angle between lastRelativeWalkDirection and z.forward
        float angle = Vector3.Angle(Vector3.forward, relativeWalkDirection);
        if (relativeWalkDirection.x < 0)
            angle *= -1;

        //This gives the rotation angle for rotation.y. As the movement is relative to the current rotation, the rotation should be relative (so use Rotate instead of setting rotation value)
        animator.gameObject.transform.Rotate(0, angle, 0);

    }
}
