using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Brain;

public class CatController : MonoBehaviour
{
    public enum BrainType { PLAYER,AI}

    public BrainType brainType;
    public Brain brain;
    public float speed;
    public GameObject cellHighlightPrefab;
    public GameObject catchHighlight;

    float[] actionCooldowns = new float[3];
    float[] lastActivationsTime = new float[3];

    private void Awake()
    {
        if (brainType.Equals(BrainType.PLAYER))
            brain = new CatPlayerBrain();
        else
            brain = new CatAIBrain();
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector2[] catchHighlightPositions = getCatchGridPositions(new Vector3());
        for (int i = 0; i < catchHighlightPositions.Length; ++i)
        {
            Instantiate(cellHighlightPrefab, new Vector3(catchHighlightPositions[i].x, cellHighlightPrefab.transform.position.y, catchHighlightPositions[i].y),
                catchHighlight.transform.rotation, catchHighlight.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        if (actions.isSpecialAction1() && (Time.time-lastActivationsTime[0])>actionCooldowns[0])
            StartCoroutine(TriggerCatchCoroutine());
    }

    private Vector2[] getCatchGridPositions(Vector3 objectPosition)
    {
        Vector2[] positions = new Vector2[5];
        int index = 0;
        Vector2 objectGridPosition = BoardManager.GridPosition(objectPosition);
        float cellSize = BoardConfiguration.Instance.cellSize;
        for (int i = -1; i <= 1; ++i)
        {
            for (int j = -1; j <= 1; ++j)
            {
                if (i * j == 0)
                {
                    positions[index] = new Vector2(objectGridPosition.x + cellSize * i, objectGridPosition.y + cellSize * j);
                    index += 1;
                }

            }
        }
        return positions;
    }
    IEnumerator TriggerCatchCoroutine()
    {
        lastActivationsTime[0] = Time.time;
        Vector2 gridPosition = BoardManager.GridPosition(transform.position);
        catchHighlight.transform.position = new Vector3(gridPosition.x, catchHighlight.transform.position.y, gridPosition.y);
        catchHighlight.SetActive(true);
        for (int i = 0; i < 14; ++i)
        {
            bool isCatch = BoardManager.Instance.positionsContainsMouse(getCatchGridPositions(catchHighlight.transform.position));
            if (isCatch)
            {
                GameManager.Instance.OnMouseCatch();
                yield break;
            }
            yield return new WaitForSeconds(0.017f * i);//check each fps
        }
        catchHighlight.SetActive(false);
    }
}
