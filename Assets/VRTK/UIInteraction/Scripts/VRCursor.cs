using UnityEngine;
using Zinnia.Action;

public class VRCursor : VRTKCursor
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform point;

    public void SetActive(bool value)
    {
        lineRenderer.enabled = value;
        point.gameObject.SetActive(value);
    }

    public override void SetCursorRay(Transform ray)
    {
        transform.position = ray.position;
        transform.rotation = ray.rotation;
        SetActive(false);
    }

    public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
    {
        var countOfPoints = 50;
        for (var i = 0; i < countOfPoints; i++)
        {
            lineRenderer.SetPosition(i, Vector3.Lerp(start, dest, (float) i / (countOfPoints - 1)));
        }

        point.transform.position = dest;
        point.transform.rotation = Quaternion.LookRotation(normal, Vector3.up);
        SetActive(true);
    }
}