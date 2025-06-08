using UnityEngine;


public class MirrorReflect : MonoBehaviour
{
    [SerializeField] private GameObject mirrorLight;
    [SerializeField] private LightController lightController;
    private float lightOffset = 0.05f;

    public GameObject MirrorLight { get => mirrorLight; set => mirrorLight = value; }

    private MirrorPuzzleManager puzzleManager;

    public bool IsLitThisFrame { get; set; } 

    public void SetPuzzleManager(MirrorPuzzleManager m) => puzzleManager = m;

    public void ReflectLight(Vector3 incomingDir, Vector3 hitPoint, int bounceRemain)
    {
        if (!lightController.LightsOn || bounceRemain <= 0) return;

        IsLitThisFrame = true;
        puzzleManager.NotifyMirrorLit(this);
        bounceRemain--;

        Vector3 normalMirror = transform.forward;
        Vector3 reflectDir = Vector3.Reflect(incomingDir, normalMirror);

        if (mirrorLight)
        {
            mirrorLight.SetActive(true);
            mirrorLight.transform.position = hitPoint + normalMirror * lightOffset;
            mirrorLight.transform.rotation = Quaternion.LookRotation(reflectDir);
        }

        if (Physics.Raycast(hitPoint + normalMirror * 0.01f,
                            reflectDir,
                            out RaycastHit hit,
                            100f))
        {
            Debug.DrawRay(hitPoint, reflectDir * hit.distance, Color.red, Time.deltaTime);

            if (hit.collider.TryGetComponent(out MirrorReflect nextMirror))
            {
                nextMirror.ReflectLight(reflectDir, hit.point, bounceRemain);
            }
            else
            {
                Door doorHit = hit.collider.GetComponentInParent<Door>();

                if (doorHit != null)
                {
                    puzzleManager.TryUnlockDoor();
                }
            }
        }
    }

    public void DisableReflection()
    {
        MirrorReflect[] mReflects = FindObjectsOfType<MirrorReflect>();
        foreach (MirrorReflect mReflect in mReflects)
        {
            if(mReflect.mirrorLight != null)
                mReflect.mirrorLight.SetActive(false);
        }
    }

}
