using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject cellHighlight;
    public GameObject catchHighlight;

    public float catchCooldown = 1;

    private float lastCatchDown = 0;

    private void Start()
    {
        Vector2[] catchHighlightPositions = getCatchGridPositions(new Vector3());
        Debug.Log(catchHighlightPositions);
        for (int i = 0; i < catchHighlightPositions.Length; ++i) {
            Instantiate(cellHighlight, new Vector3(catchHighlightPositions[i].x,cellHighlight.transform.position.y, catchHighlightPositions[i].y),
                catchHighlight.transform.rotation,catchHighlight.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

    }

    void HandleInput() {
        //Movement
        float zTranslation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float xTranslation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(xTranslation, 0, zTranslation);
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
        //Special actions
        if (Input.GetButtonDown("Fire1") && (Time.time - lastCatchDown)>catchCooldown) {
            StartCoroutine(TriggerCatchCoroutine());
        }

    }

    public Vector2[] getCatchGridPositions(Vector3 objectPosition) {
        Vector2[] positions= new Vector2[5];
        int index = 0;
        Vector2 objectGridPosition = BoardManager.GridPosition(objectPosition);
        float cellSize = BoardConfiguration.Instance.cellSize;
        for (int i = -1; i <= 1; ++i) {
            for (int j = -1; j <= 1; ++j){
                if (i * j == 0) {
                    positions[index] = new Vector2(objectGridPosition.x + cellSize * i, objectGridPosition.y + cellSize * j);
                    index += 1;
                }
                
            }
         }
                    return positions;
    }

    IEnumerator TriggerCatchCoroutine() {
        lastCatchDown = Time.time;
        Vector2 gridPosition = BoardManager.GridPosition(transform.position);
        catchHighlight.transform.position = new Vector3(gridPosition.x,catchHighlight.transform.position.y,gridPosition.y);
        catchHighlight.SetActive(true);
        for (int i = 0; i < 14; ++i)
        {
            bool isCatch = BoardManager.Instance.positionsContainsMouse(getCatchGridPositions(catchHighlight.transform.position));
            if (isCatch)
            {
                GameManager.Instance.OnMouseCatch();
                yield break;
            }
            yield return new WaitForSeconds(0.017f*i);//check each fps
        }
        catchHighlight.SetActive(false);
    }
}
