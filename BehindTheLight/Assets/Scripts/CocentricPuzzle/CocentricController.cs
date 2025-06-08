using UnityEngine;

public class CocentricController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour ring1Behaviour; // aquí arrastramos el GameObject que tenga CocentricRing o ContinuousCocentricRing
    [SerializeField] private MonoBehaviour ring2Behaviour;
    [SerializeField] private MonoBehaviour ring3Behaviour;

    private ICocentricRing ring1 => ring1Behaviour as ICocentricRing;
    private ICocentricRing ring2 => ring2Behaviour as ICocentricRing;
    private ICocentricRing ring3 => ring3Behaviour as ICocentricRing;

    [SerializeField] private Door doorToUnlock;
    [SerializeField] private ChangeAlbedoOnTrigger onAlbedoOnTrigger;

    private Color gold = new Color(0.9f, 0.8f, 0.2f);

    internal bool IsSolved { get; private set; }

    internal void CheckSolution()
    {
        if (IsSolved) return;

        bool ok1 = IsRingCorrect(ring1);
        bool ok2 = IsRingCorrect(ring2);
        bool ok3 = IsRingCorrect(ring3);

        if (ok1 && ok2 && ok3)
        {
            IsSolved = true;
            OnSolved();
        }
    }

    private bool IsRingCorrect(ICocentricRing ring)
    {
        if (ring == null) return false;

        float target = ring.CorrectAngle;
        float actual = ring.CurrentAngleY;

        if (Mathf.Approximately(target, 360f))
        {
            float diff0 = Mathf.Abs(Mathf.DeltaAngle(actual, 0f));
            if (diff0 < 0.1f) return true;
        }

        if (Mathf.Approximately(target, 0f))
        {
            float diff0 = Mathf.Abs(Mathf.DeltaAngle(actual, 0f));
            if (diff0 < 0.1f) return true;
        }

        return ring.IsCorrect;
    }

    private void OnSolved()
    {
        Debug.Log("¡Co­centric ring solved!");

        ring1.PaintSolved(gold);
        ring2.PaintSolved(gold);
        ring3.PaintSolved(gold);
        doorToUnlock.UnlockDoor();
        onAlbedoOnTrigger.ChangeAlbedoOnActivate();
    }
}
