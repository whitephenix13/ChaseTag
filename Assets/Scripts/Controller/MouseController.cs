using System.Collections.Generic;
using UnityEngine;
using static Brain;

public class MouseController : AvatarController
{

    [Header("Ai properties")]
    [SerializeField]
    private float aiFearStrength = 0;
    [SerializeField]
    [Range(1,1000)]
    private float aiFearArea = 1;
    [SerializeField]
    private List<CellType> travelCostKeys;
    [SerializeField]
    private List<float> travelCostValues;
    [SerializeField]
    private float sameCellTypePenalty;

    [Header("Debug properties")]
    [SerializeField]
    private bool shouldDebugAiPath = false;
    [SerializeField]
    private GameObject debugArrowObject;
    [SerializeField]
    private GameObject debugTargetObject;
    [SerializeField]
    private GameObject debugArrowContainer;
    private float debugLastAiUpdate = -1;
    private List<Vector3> debugAiPath = new List<Vector3>();

    protected new void Awake()
    {
        base.Awake();
        if (brainType.Equals(BrainType.PLAYER))
            brain = new PlayerBrain();
        else
            brain = new MouseAIBrain();
    }

    private bool areListEquals<T>(List<T> list1, List<T> list2) {
        if (list1 == null && list2 == null)
            return true;
        else if (list1 == null || list2 == null)
            return false;
        else if (list1.Count != list2.Count)
            return false;
        else {
            for (int i = 0; i < list1.Count; ++i)
                if (!list1[i].Equals(list2[i]))
                    return false;
            return true;
        }
    }

    private void instantiateDebugArrows(Brain currentBrain) {
        if (currentBrain is MouseAIBrain)
        {
            MouseAIBrain mouseAIBrain = ((MouseAIBrain)currentBrain);
            if (debugLastAiUpdate != mouseAIBrain.GetLastUpdate())
            {
                List<Vector3> newAiPath = ((MouseAIBrain)currentBrain).GetCurrentPath();
                if (!areListEquals(debugAiPath, newAiPath)) {
                    debugAiPath = newAiPath;
                    foreach (Transform child in debugArrowContainer.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    if (newAiPath.Count > 1)
                        for (int i = 0; i < newAiPath.Count - 1; ++i)
                        {
                            Quaternion arrowRotation = Quaternion.FromToRotation(Vector3.right,newAiPath[i+1] - newAiPath[i]);
                            Instantiate(debugArrowObject, new Vector3(newAiPath[i].x, 0.08f, newAiPath[i].z), arrowRotation, debugArrowContainer.transform);
                        }
                    Instantiate(debugTargetObject, new Vector3(newAiPath[newAiPath.Count - 1].x, 0.08f, newAiPath[newAiPath.Count - 1].z), Quaternion.identity, debugArrowContainer.transform);

                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlayer1 = false;
        if (brainType.Equals(BrainType.AI))
            ((MouseAIBrain)brain).poolParameters(aiFearStrength, aiFearArea, travelCostKeys, travelCostValues, sameCellTypePenalty);
        UpdateAvatar(isPlayer1);
        if (shouldDebugAiPath) {
            instantiateDebugArrows(brain);
        }
    }
}
