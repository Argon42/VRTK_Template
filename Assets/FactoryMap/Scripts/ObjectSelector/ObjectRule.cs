using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Rule;
using Zinnia.Rule.Collection;

public class ObjectRule : MonoBehaviour, IRule
{
    public bool Accepts(object target)
    {
        var result = target is GameObject component ? component.GetComponent<SelectableObject>() : false;
        Debug.Log(result);
        return result;
    }
}