using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IInputManager
{
    private Action<Vector3> _onPointerSecondChangeHandler;
    private Action _onPointerSecondUpHandler;
    private Action<Vector3> _onPointerDownHandler;
    private Action _onPointerUpHandler;
    private Action<Vector3> _onPointerChangeHandler;
    private LayerMask _mouseInputMask;

    public LayerMask MouseInputMask {get => _mouseInputMask; set => _mouseInputMask = value;}
    
    // Update is called once per frame
    void Update()
    {
        GetPointerPosition();
        GetPanningPointer();
    }

    private void GetPointerPosition()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            CallActionOnPointer((position) => _onPointerDownHandler?.Invoke(position));
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            CallActionOnPointer((position) => _onPointerChangeHandler?.Invoke(position));
        }
        if(Input.GetMouseButtonUp(0))
        {
            _onPointerUpHandler?.Invoke();
        }

    }

    private void CallActionOnPointer(Action<Vector3> action)
    {
        Vector3? position = GetMousePosition();
        if (position.HasValue)
        {
            action(position.Value);
            position = null;
        }
    }

    private Vector3? GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3? position = null;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, _mouseInputMask))
        {
            position = hit.point - transform.position;
        }

        return position;
    }

    private void GetPanningPointer()
    {
        if (Input.GetMouseButton(1))
        {
            var position = Input.mousePosition;
            _onPointerSecondChangeHandler?.Invoke(position);
        }
        if (Input.GetMouseButtonUp(1))
        {
            _onPointerSecondUpHandler?.Invoke();
        }
    }

    //Pointer down Events
    public void AddListenerOnPointerDownEvent(Action<Vector3> listener)
    {
        _onPointerDownHandler += listener;
    }

    public void RemoveListenerOnPointerDownEvent(Action<Vector3> listener)
    {
        _onPointerDownHandler -= listener;
    }

    //Pointer Second Change Events
    public void AddListenerOnPointerSecondChangeEvent(Action<Vector3> listener)
    {
        _onPointerSecondChangeHandler += listener;
    }

    public void RemoveListenerOnPointerSecondChangeEvent(Action<Vector3> listener)
    {
        _onPointerSecondChangeHandler -= listener;
    }

    //Pointer Second Up Events
    public void AddListenerOnPointerSecondUpEvent(Action listener)
    {
        _onPointerSecondUpHandler += listener;
    }

    public void RemoveListenerOnPointerSecondUpEvent(Action listener)
    {
        _onPointerSecondUpHandler -= listener;
    }

    //Pointer Up Events
    public void AddListenerOnPointerUpEvent(Action listener)
    {
        _onPointerUpHandler += listener;
    }

    public void RemoveListenerOnPointerUpEvent(Action listener)
    {
        _onPointerUpHandler -= listener;
    }

    //Pointer Change Events
    public void AddListenerOnPointerChangeEvent(Action<Vector3> listener)
    {
        _onPointerChangeHandler += listener;
    }

    public void RemoveListenerOnPointerChangeEvent(Action<Vector3> listener)
    {
        _onPointerChangeHandler -= listener;
    }
}
