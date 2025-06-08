using UnityEngine;

public class PuzzleRingsMatchController : MonoBehaviour
{
    [SerializeField] private Transform[] refRings;
    [SerializeField] private RingRotator[] playerRings;
    [SerializeField] private Door doorToUnlock;
    [SerializeField] private GameObject enemy;
    private bool completed = false;
    [SerializeField] private LightController lightController;

    private float[] targetAngles;

    private void Start()
    {
        GenerateReferenceAngles();
    }

    private void Update()
    {
        if (completed)
        {
            if (lightController.LightsOn)
            {
                enemy.SetActive(true);
            }
            else
            {
                enemy.SetActive(false);
            }
        }
    }

    private void GenerateReferenceAngles()
    {
        targetAngles = new float[refRings.Length];

        for (int i = 0; i < refRings.Length; i++)
        {
            int steps = Random.Range(0, 12);
            float angle = steps * 30f;

            targetAngles[i] = angle;

            Vector3 currentRot = refRings[i].localEulerAngles;
            refRings[i].localEulerAngles = new Vector3(currentRot.x, currentRot.y, angle);
        }
    }

    public void CheckPuzzle()
    {
        bool allCorrect = true;

        for (int i = 0; i < playerRings.Length; i++)
        {
            float currentZ = playerRings[i].transform.localEulerAngles.z;
            float angleA = targetAngles[i];                    
            float angleB = (angleA + 180f) % 360f;           

            float diffA = Mathf.Abs(Mathf.DeltaAngle(currentZ, angleA));
            float diffB = Mathf.Abs(Mathf.DeltaAngle(currentZ, angleB));

            bool isCorrect = (diffA < 0.1f || diffB < 0.1f);

            Renderer rend = playerRings[i].GetComponentInChildren<Renderer>();
            if (rend != null)
                rend.material.color = isCorrect ? Color.green : Color.gray;

            if (!isCorrect)
                allCorrect = false;
        }

        if (allCorrect)
        {
            PuzzleSolved();
        }
    }

    private void PuzzleSolved()
    {
        Debug.Log("¡You solved the puzzle!");

        foreach (var ring in playerRings)
        {
            Renderer rend = ring.GetComponentInChildren<Renderer>();
            if (rend != null)
                rend.material.color = Color.yellow;
        }
        doorToUnlock.UnlockDoor();
        completed = true;
    }
}