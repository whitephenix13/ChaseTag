using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Brain;
public class AvatarController : MonoBehaviour
{
    public enum BrainType { PLAYER, AI }

    public BrainType brainType;
    public Brain brain;
    [Header("Movement speed")]
    public float speed;
    public float jumpSpeed;
    public float slideSpeed;
    [Range(0, 1)]
    public float slideDrag = 0.05f;
    public float dashSpeed;


    protected float slideSpeedForce;

    private Vector3 lastWalkDirection = new Vector3(0, 0, 1);

    protected Animator animator;
    protected Rigidbody rigidbody;
    public void Awake()
    {
        if (brainType.Equals(BrainType.PLAYER))
            brain = new PlayerBrain();
        else
            brain = new CatAIBrain();

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        slideSpeedForce = slideSpeed;
    }

    public void OnGameReset() {
        rigidbody.velocity = new Vector3();

        animator.SetFloat("catXSpeed", 0);
        animator.SetFloat("catZSpeed", 0);
        animator.SetBool("jumpOver", false);
        animator.SetBool("slide", false);
        animator.SetBool("dash", false);

        animator.Play("walk");
    }

    // Update is called once per frame
    protected void UpdateAvatar(bool isPlayer1)
    {
        Actions actions = brain.brainUpdate(isPlayer1);
        //Movement
        changeState(actions);

        string playerPostfix = isPlayer1 ? "P1" : "P2";

        //If controlled by AI, simulate a key press 

        Vector3 direction = new Vector3(actions.getHorizontalAxis(), 0, actions.getVerticalAxis());
        bool isDirectionPressed = false;
        if (brainType.Equals(BrainType.PLAYER))
            isDirectionPressed = (Input.GetAxisRaw("Horizontal" + playerPostfix) != 0 || Input.GetAxisRaw("Vertical" + playerPostfix) != 0) && direction.magnitude >= 0.25;//consider a lower threshold to avoid imprecisions (especially for rotations whn exiting animation)
        else
            isDirectionPressed = actions.getHorizontalAxis() != 0 || actions.getVerticalAxis() != 0;

        bool isSliding = animator.GetCurrentAnimatorStateInfo(0).IsName("Base.slide");
        bool isJumpOver = animator.GetCurrentAnimatorStateInfo(0).IsName("Base.jumpOver");
        bool isDash = animator.GetCurrentAnimatorStateInfo(0).IsName("Base.dash");
        bool isDashLag = animator.GetCurrentAnimatorStateInfo(0).IsName("Base.dashLag");
        if (!isSliding)
            slideSpeedForce = slideSpeed;

        if (isDash)
        {
            dashMove();
        }
        else if (isDashLag)
        {
            dashLagMove();
        }
        else if (isJumpOver)
        {
            jumpOverMove();
        }
        else if (isSliding)
        {
            slideMove();
        }
        else if (isDirectionPressed)
        {
            walkMove(direction);
        }

        if (!isDirectionPressed)
        {
            //Rotate towards last walk direction before stopping as we move relative to the world coordinates
            animator.SetFloat("catXSpeed", 0);
            animator.SetFloat("catZSpeed", 0);
        }
    }

    private void changeState(Actions actions)
    {
        if (actions.isJump())
            animator.SetBool("jumpOver", true);
        

        if (actions.isSlide())
            animator.SetBool("slide", true);
        else
            animator.SetBool("slide", false);

        if (actions.isDash())
            animator.SetBool("dash", true);
        
    }

    private void walkMove(Vector3 direction)
    {
        //We have to set the speed for the animation as well as moving towards the correct direction
        //The movement based on the key pressed is absolute (pressing forward key ALWAYS means we go towards z axis)
        //The speed used to determine the animation should be relative to the player (if the player faces right and that we move right, we want the forward walk animation) 

        //We are interested in the x, z speed in the facedDirection referential (so we project movementDirection on those axis) 

        Vector3 directionNormalized = direction.normalized;
        direction = direction.normalized * speed * Time.deltaTime;


        float facedDirectionYAngle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        Vector3 facedDirection = new Vector3(Mathf.Sin(facedDirectionYAngle), 0, Mathf.Cos(facedDirectionYAngle));
        Vector3 facedDirectionRotated90 = new Vector3(facedDirection.z, facedDirection.y, -facedDirection.x);
        animator.SetFloat("catZSpeed", Vector3.Dot(directionNormalized, facedDirection));
        animator.SetFloat("catXSpeed", Vector3.Dot(directionNormalized, facedDirectionRotated90));

        lastWalkDirection = direction.normalized;

        //Direction is relative to the world coordinate space
        rigidbody.AddForce(direction.normalized * speed, ForceMode.Impulse);
    }
    private void jumpOverMove()
    {
        rigidbody.AddForce(lastWalkDirection * jumpSpeed, ForceMode.Impulse);
    }
    private void slideMove()
    {
        rigidbody.AddForce(lastWalkDirection * slideSpeedForce, ForceMode.Impulse);
        slideSpeedForce *= (1 - slideDrag);
    }
    private void dashMove()
    {
        rigidbody.AddForce(lastWalkDirection * dashSpeed, ForceMode.Impulse);
    }
    private void dashLagMove()
    {
    }
}
