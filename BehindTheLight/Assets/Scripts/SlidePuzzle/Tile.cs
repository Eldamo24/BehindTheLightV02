using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IInteractable
{
    [SerializeField] private int id;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private string onInteractMsg = "Press E to move tile {0}";

    private bool isMoving = false;
    private Vector3 targetPosition;  

    public Vector2Int CurrentCoord { get; set; }
    public string OnInteractMsg => string.Format(onInteractMsg, id);

    public void OnInteract()
    {

        if (isMoving) return;
        if (puzzleManager == null) return;

        bool moved = puzzleManager.TryMoveTile(this);
    }

    public void BeginMove(Vector3 targetWorldPos)
    {
        isMoving = true;
        targetPosition = targetWorldPos;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
                if (puzzleManager != null)
                {
                    puzzleManager.OnTileMoved();
                }
            }
        }
    }

    public void Initialize(int tileId, PuzzleManager manager)
    {
        id = tileId;
        puzzleManager = manager;
    }

    public int GetID()
    {
        return id;
    }
}
