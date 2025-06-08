using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LightController : MonoBehaviour
{
    [SerializeField] private LayerMask lighteableObjectsMask;
    private float distance = 5f;
    [SerializeField] private GameObject lightedObject;
    private GameObject lastLightedObject;
    [SerializeField] private bool lightsOn;
    [SerializeField] private GameObject lanternLight;

    //Correccion de linterna
    [SerializeField] private int verticalRayCount = 5;
    [SerializeField] private int horizontalRayCount = 5;
    [SerializeField] private float coneAngle = 20f;
    [SerializeField] private bool showRays = true;
    private HashSet<GameObject> lastLightedObjects = new HashSet<GameObject>();

    public bool LightsOn { get => lightsOn; set => lightsOn = value; }

    private void Start()
    {
        lightsOn = false;
    }

    void Update()
    {
        if (lightsOn)
        {
            lanternLight.SetActive(true);
            DetectLighteableObjects();
        }
        else
        {
            lanternLight.SetActive(false);
            RemoveLightedObject();
        }
    }

    /// <summary>
    /// Function that detects if there is an object on the lantern light
    /// </summary>
    private void DetectLighteableObjects()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        HashSet<GameObject> currentLightedObjects = new HashSet<GameObject>();
        float verticalStep = coneAngle / Mathf.Max(verticalRayCount, -1, 1);
        float horizontalStep = coneAngle / Mathf.Max(horizontalRayCount, -1, 1);
        
        for(int i = 0; i < verticalRayCount; i++)
        {
            float verticalAngle = -coneAngle / 2 + verticalStep * i;
            for(int j = 0; j<horizontalRayCount; j++)
            {
                float horizontalAngle = -coneAngle / 2 + horizontalStep * j;
                Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                Vector3 direction = rotation * forward;
                if(Physics.Raycast(origin, direction, out RaycastHit hit, distance, lighteableObjectsMask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    currentLightedObjects.Add(hitObject);
                    if (showRays)
                        Debug.DrawRay(origin, direction * hit.distance, Color.green, 0.1f);
                }
                else if (showRays)
                {
                    Debug.DrawRay(origin, direction * distance, Color.red, 0.1f);
                }
                
            }
        }
        foreach(var obj in currentLightedObjects)
        {
            if (!lastLightedObjects.Contains(obj))
            {
                obj.GetComponent<LightedObjects>()?.StartFadeIn();
            }
        }

        foreach(var obj in lastLightedObjects)
        {
            if (!currentLightedObjects.Contains(obj))
            {
                obj.GetComponent<LightedObjects>()?.StartFadeOut();
            }
        }
        lastLightedObjects = currentLightedObjects;
    }

    //En este metodo tengo que sacar el error de la linterna. Dejo anotado para recordar. Los elementos no desaparecen al apagar la linterna. 
    void RemoveLightedObject()
    {
        if (lastLightedObjects.Count > 0)
        {
            foreach (var obj in lastLightedObjects)
            {
                obj.GetComponent<LightedObjects>()?.StartFadeOut();
            }
            lastLightedObjects.Clear();
        }
    }

    private void OnDrawGizmos()
    {

        if (!Application.isPlaying) return;

        Vector3 origin = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        float verticalStep = coneAngle / Mathf.Max(verticalRayCount - 1, 1);
        float horizontalStep = coneAngle / Mathf.Max(horizontalRayCount - 1, 1);

        for (int y = 0; y < verticalRayCount; y++)
        {
            float verticalAngle = -coneAngle / 2 + verticalStep * y;

            for (int x = 0; x < horizontalRayCount; x++)
            {
                float horizontalAngle = -coneAngle / 2 + horizontalStep * x;

                Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                Vector3 direction = rotation * forward;

                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(origin, direction * distance);
            }
        }


        Gizmos.color = new Color(0, 1, 1, 0.2f);
        Vector3 coneEnd = origin + forward * distance;
        Gizmos.DrawWireSphere(coneEnd, Mathf.Tan(coneAngle * Mathf.Deg2Rad / 2f) * distance);

    }
}
