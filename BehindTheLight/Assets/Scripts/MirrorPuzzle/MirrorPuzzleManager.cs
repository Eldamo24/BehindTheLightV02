using System.Linq;
using UnityEngine;

public class MirrorPuzzleManager : MonoBehaviour
{
    [SerializeField] private Door door;

    private MirrorReflect[] mirrors;
    private bool solved;

    void Awake()
    {
        mirrors = GetComponentsInChildren<MirrorReflect>(true);
        foreach (var m in mirrors) m.SetPuzzleManager(this);
    }

    public void NotifyMirrorLit(MirrorReflect mirror)
    {
        mirror.IsLitThisFrame = true;
    }

    public void TryUnlockDoor()
    {
        if (solved) return;

        if (mirrors.All(m => m.IsLitThisFrame))
        {
            door.UnlockDoor();
            solved = true;
        }
    }

    void LateUpdate()
    {
        if (solved) return;
        foreach (var m in mirrors) m.IsLitThisFrame = false;
    }
}
