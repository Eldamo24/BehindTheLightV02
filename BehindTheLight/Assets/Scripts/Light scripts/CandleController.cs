using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class CandleController : MonoBehaviour
{
    [Header("Candle Settings")]
    [SerializeField] private LayerMask lighteableObjectsMask;
    [SerializeField] private float range = 5f;
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private GameObject candleFlame;

    private HashSet<GameObject> lastLightedObjects = new HashSet<GameObject>();

    void Update()
    {
        // Aquí podrías alternar candleFlame.active = true/false
        // en función de si enciendes/apagas la vela,
        // por ejemplo con una tecla:
        // if (Input.GetKeyDown(KeyCode.F)) candleFlame.SetActive(!candleFlame.activeSelf);

        if (candleFlame != null && candleFlame.activeSelf)
            DetectLightableObjects();
        else
            RemoveLightedObjects();
    }

    private void DetectLightableObjects()
    {
        // 1) Haz un OverlapSphere para encontrar todos los colliders en el rango
        Collider[] hits = Physics.OverlapSphere(transform.position, range, lighteableObjectsMask);
        var current = new HashSet<GameObject>();

        foreach (var col in hits)
        {
            // 2) Calcula dirección desde la vela al centro del objeto
            Vector3 dir = (col.transform.position - transform.position).normalized;

            // 3) Lanza un SphereCast en esa dirección
            if (Physics.SphereCast(transform.position, sphereRadius, dir, out RaycastHit hit, range, lighteableObjectsMask))
            {
                var obj = hit.collider.gameObject;
                current.Add(obj);

                // 4) Si es nuevo, haz fade-in
                if (!lastLightedObjects.Contains(obj))
                    obj.GetComponent<LightedObjects>()?.StartFadeIn();
            }
        }

        // 5) Los que ya no estén en current, hazles fade-out
        foreach (var obj in lastLightedObjects)
        {
            if (!current.Contains(obj))
                obj.GetComponent<LightedObjects>()?.StartFadeOut();
        }

        lastLightedObjects = current;
    }

    private void RemoveLightedObjects()
    {
        foreach (var obj in lastLightedObjects)
            obj.GetComponent<LightedObjects>()?.StartFadeOut();

        lastLightedObjects.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        // Te ayuda a visualizar el rango en la escena
        Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
