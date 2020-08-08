using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static Brain;

public class MouseController : MonoBehaviour
{
    public enum BrainType { PLAYER, AI }

    public BrainType brainType;
    public Brain brain;
    public float speed;

    float[] actionCooldowns = new float[3];
    float[] lastActivationsTime = new float[3];

    [DllImport("ChaseTagAI", EntryPoint = "Negate")] //Use this to specify the name of the dll to import as well as the name of the function
    public static extern bool Negate(bool val);

    private void Awake()
    {
        if (brainType.Equals(BrainType.PLAYER))
            brain = new MousePlayerBrain();
        else
            brain = new MouseAIBrain();
    }
    // Update is called once per frame
    void Update()
    {
        //TODO: clean duplicated code with cat (create a new object for composition)
        Actions actions = brain.brainUpdate(actionCooldowns, lastActivationsTime);

        //Movement
        float zTranslation = actions.getVerticalAxis() * speed * Time.deltaTime;
        float xTranslation = actions.getHorizontalAxis() * speed * Time.deltaTime;
        transform.Translate(xTranslation, 0, zTranslation);

        //Move the object back to the center of the grid
        if (xTranslation == 0 && zTranslation == 0)
        {
            Vector2 gridPosition = BoardManager.GridPosition(transform.position);
            Vector3 deltaGridPosition = new Vector3(gridPosition.x - transform.position.x, 0, gridPosition.y - transform.position.z);
            float moveMagnitude = speed * Time.deltaTime;
            if (deltaGridPosition.magnitude <= moveMagnitude)
            {
                this.transform.position = new Vector3(gridPosition.x, transform.position.y, gridPosition.y);
            }
            else
            {
                Vector3 normalizedGridCenterDirection = deltaGridPosition.normalized;
                this.transform.Translate(normalizedGridCenterDirection.x * moveMagnitude, 0, normalizedGridCenterDirection.z * moveMagnitude);
            }
        }
    }
}
