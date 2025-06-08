using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int rows = 3;
    [SerializeField] private int cols = 3;
    [SerializeField] private float tileSpacing = 1.0f;
    [SerializeField] private Color defaultColor = Color.gray;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color solvedColor = new Color(1f, 0.84f, 0f);
    [SerializeField] private Door doorToUnlock;
    [SerializeField] private int shuffleSteps = 50;

    private Tile[,] grid;
    private Vector2Int emptySlot;
    private bool tileMovingInProgress = false; 

    private int[,] initialLayout = new int[3, 3] {
        { 1, 2, 3 },
        { 4, 5, 6 },
        { 0, 7, 8 }
    };

    void Start()
    {
        grid = new Tile[rows, cols];
        SpawnTiles();
        ShuffleTiles(shuffleSteps);
        UpdateTileColors();
    }

    private void SpawnTiles()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int tileId = initialLayout[r, c];
                if (tileId == 0)
                {
                    emptySlot = new Vector2Int(c, r);
                    grid[r, c] = null;
                }
                else
                {
                    GameObject tileObj = Instantiate(tilePrefab);
                    tileObj.name = "Tile_" + tileId; 
                    tileObj.transform.SetParent(this.transform);

                    Vector3 worldPos = GridToWorldPosition(c, r);
                    tileObj.transform.position = worldPos;

                    Tile tile = tileObj.GetComponent<Tile>();
                    tile.Initialize(tileId, this);
                    tile.CurrentCoord = new Vector2Int(c, r);

                    grid[r, c] = tile;
                }
            }
        }
    }

    private void ShuffleTiles(int steps)
    {
        System.Random rng = new System.Random();
        Vector2Int[] dirs = {
        new Vector2Int( 0, 1),    // up
        new Vector2Int( 0,-1),    // down
        new Vector2Int(-1, 0),    // left
        new Vector2Int( 1, 0)     // right
    };

        for (int i = 0; i < steps; i++)
        {
            var neighbors = new System.Collections.Generic.List<Tile>();

            foreach (var d in dirs)
            {
                Vector2Int n = emptySlot + d;
                if (n.x >= 0 && n.x < cols && n.y >= 0 && n.y < rows)
                {
                    Tile t = grid[n.y, n.x];
                    if (t != null) neighbors.Add(t);
                }
            }

            if (neighbors.Count == 0) continue;

            Tile chosen = neighbors[rng.Next(neighbors.Count)];

            Vector2Int originCoord = chosen.CurrentCoord;
            Vector2Int targetCoord = emptySlot;

            Vector3 targetPos = GridToWorldPosition(targetCoord.x, targetCoord.y);

            grid[targetCoord.y, targetCoord.x] = chosen;
            grid[originCoord.y, originCoord.x] = null;

            chosen.CurrentCoord = targetCoord;
            chosen.transform.position = targetPos;

            emptySlot = originCoord;
        }
    }

    private Vector3 GridToWorldPosition(int col, int row)
    {
        float xOffset = (cols - 1) * tileSpacing / 2f;
        float yOffset = (rows - 1) * tileSpacing / 2f;
        float x = col * tileSpacing - xOffset;
        float y = yOffset - row * tileSpacing;
        float z = 0f;
        return transform.TransformPoint(new Vector3(x, y, z));
    }

    public bool TryMoveTile(Tile tile)
    {
        if (tileMovingInProgress)
            return false;

        Vector2Int tileCoord = tile.CurrentCoord;
        if (IsAdjacent(tileCoord, emptySlot))
        {
            Vector3 targetWorldPos = GridToWorldPosition(emptySlot.x, emptySlot.y);
            grid[tileCoord.y, tileCoord.x] = null;
            grid[emptySlot.y, emptySlot.x] = tile;
            Vector2Int oldEmpty = emptySlot;
            emptySlot = tileCoord;
            tile.CurrentCoord = new Vector2Int(oldEmpty.x, oldEmpty.y);

            tileMovingInProgress = true;
            tile.BeginMove(targetWorldPos);
            return true;
        }
        return false;
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1);
    }

    public void OnTileMoved()
    {
        tileMovingInProgress = false;
        UpdateTileColors();
        CheckPuzzleSolved();
    }

    private void UpdateTileColors()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Tile tile = grid[r, c];
                if (tile == null) continue;

                int correctId = (r * cols + c + 1);

                Renderer rend = tile.GetComponent<Renderer>();

                if (tile.GetID() == correctId)
                    rend.material.color = correctColor;
                else
                    rend.material.color = defaultColor;
            }
        }
    }

    private void CheckPuzzleSolved()
    {
        bool allCorrect = true;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (r == rows - 1 && c == cols - 1) continue;
                Tile tile = grid[r, c];
                if (tile == null) { allCorrect = false; break; }
                int correctId = (r * cols + c + 1);
                if (tile.GetID() != correctId)
                {
                    allCorrect = false;
                    break;
                }
            }
            if (!allCorrect) break;
        }

        if (allCorrect)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Tile tile = grid[r, c];
                    if (tile == null) continue;
                    tile.GetComponent<Renderer>().material.color = solvedColor;
                }
            }
            doorToUnlock.UnlockDoor();
            Debug.Log("Puzzle Solved! All tiles are in correct position.");
        }
    }
}
