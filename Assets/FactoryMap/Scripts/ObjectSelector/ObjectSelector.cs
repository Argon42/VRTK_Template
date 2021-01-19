using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Pointer;

public class ObjectSelector : MonoBehaviour
{
    public void Entered(ObjectPointer.EventData eventData)
    {
        var selectableObject = eventData.CollisionData.transform.GetComponent<SelectableObject>();
        if (selectableObject)
            selectableObject.Enter(eventData, this);
    }

    public void Exit(ObjectPointer.EventData eventData)
    {
        var selectableObject = eventData.CollisionData.transform.GetComponent<SelectableObject>();
        if (selectableObject)
            selectableObject.Exit(eventData, this);
    }

    public void Select(ObjectPointer.EventData eventData)
    {
        var selectableObject = eventData.CollisionData.transform.GetComponent<SelectableObject>();
        if (selectableObject)
            selectableObject.Select(eventData, this);
    }
}