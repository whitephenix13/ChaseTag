using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAIBrainMovement
{
    private const float MAX_SLIDE_TIME = 0.7f;
    private float startSlideTime;

    private Animator animator;
    public MouseAIBrainMovement()
    {
        animator = GameMainManager.Instance.mouse.GetComponent<Animator>();
    }
    public Actions DecideAction(List<Vector3> currentPath, ref int nextPathIndex)
    {
        //Move to target if you can
        float[] actions = new float[Actions.actionTableLength];

        updateCellIndex(currentPath, ref nextPathIndex);

        //TODO: 1) Make sure that the obstacle is crossed correctly: try to jump faster to avoid loosing time (1 cell ahead) 
        //TODO: 2) Try to slide ahead and stop sliding afterwards 
        //TODO: 3) Also, algorithm does not know that you can't change direction while sliding/jumping 

        Vector3? previousCellIndex = null;
        CellType? previousCellType = null;
        if (nextPathIndex > 0) {
            previousCellIndex = BoardManager.ConvertPositionToGridIndex(currentPath[nextPathIndex - 1]);
            previousCellType = BoardManager.Instance.board[(int)previousCellIndex.Value.x][(int)previousCellIndex.Value.z];
        }

        Vector3 targetCellIndex = BoardManager.ConvertPositionToGridIndex(currentPath[nextPathIndex]);
        CellType nextCellType = BoardManager.Instance.board[(int)targetCellIndex.x][(int)targetCellIndex.z];

        Vector3 direction = (currentPath[nextPathIndex] - GameMainManager.Instance.mouse.transform.position).normalized;

        actions[0] = direction.x;
        actions[1] = direction.z;

        bool shouldSlide = false;
        //if (previousCellType != null)
            //shouldSlide = previousCellType == CellType.BAR || (previousCellType == CellType.STEP_BAR);
        shouldSlide = shouldSlide || (nextCellType == CellType.BAR || (nextCellType == CellType.STEP_BAR));
        bool isSliding = animator.GetCurrentAnimatorStateInfo(0).IsName("Base.slide");

        if (shouldSlide)
        {
            if (!isSliding)
                startSlideTime = Time.time;
            if ((Time.time - startSlideTime) < MAX_SLIDE_TIME)
                actions[2] = 1;
        }

        if (nextCellType == CellType.STEP)
            actions[3] = 1;

        //TODO: when to dash? 
        return new Actions(actions);
   
    }

    private void updateCellIndex(List<Vector3> currentPath, ref int nextPathIndex) {
        if ((nextPathIndex+1) == currentPath.Count)
            return;


        //Index = -1 is when a new path is set
        if (nextPathIndex == -1) {
            nextPathIndex = 0;
        }

        Vector3 nextCell = currentPath[nextPathIndex];
        Vector3? nextNextCellToReach = getNextCellToReached(nextPathIndex, currentPath);

        while (IsNextCellReached(GameMainManager.Instance.mouse.transform.position, nextCell, nextNextCellToReach) && (nextPathIndex+1) < currentPath.Count)
        {
            nextPathIndex += 1;
            nextCell = currentPath[nextPathIndex];
            nextNextCellToReach = getNextCellToReached(nextPathIndex, currentPath);
        }
    }

    private bool IsNextCellReached(Vector3 avatar, Vector3 next, Vector3? nextNext)
    {
        //Note: a cell current is considered as reached if: 
        // 1) the avatar is on the path between current and next (current was already crossed)
        // 2) the avatar is close the to center of the current cell
        Vector2 avatarXZpos = new Vector2(avatar.x,avatar.z);
        Vector2 currentXZpos = new Vector2(next.x, next.z);
        Vector2? nextXZpos = null;
        if (nextNext.HasValue) nextXZpos = new Vector2(nextNext.Value.x, nextNext.Value.z);

        //Check case 1: being between two cells mean that the dot product is close to -1 
        if (nextXZpos.HasValue)
        {
            float dotProduct = Vector2.Dot((currentXZpos - avatarXZpos).normalized, (nextXZpos.Value - avatarXZpos).normalized);
            if (dotProduct < -0.9)
                return true;
        }

        //Check case 2
        return Vector2.Distance(avatarXZpos, currentXZpos) <= 0.1;
    }
    private Vector3? getNextCellToReached(int index, List<Vector3> currentPath) {
        Vector3? nextCellToReach = null;
        if ((index + 1) < currentPath.Count)
            nextCellToReach = currentPath[index + 1];
        return nextCellToReach;
    }
}
