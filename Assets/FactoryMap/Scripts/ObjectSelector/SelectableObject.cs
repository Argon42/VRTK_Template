using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zinnia.Pointer;

public class SelectableObject : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public UnityEvent onSelect;

    public void Enter(ObjectPointer.EventData eventData, ObjectSelector objectSelector) => onEnter?.Invoke();
    public void Exit(ObjectPointer.EventData eventData, ObjectSelector objectSelector) => onExit?.Invoke();
    public void Select(ObjectPointer.EventData eventData, ObjectSelector objectSelector) => onSelect?.Invoke();
}