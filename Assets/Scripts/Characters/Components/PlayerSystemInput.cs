using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerSystemInput : MonoBehaviour, IInput
{
    [SerializeField] int inputIndex;

    public int playerInputIndex { get => inputIndex; set => inputIndex = value; }

    public Vector2 axis { get; private set; }

    public Vector2 absAxis { get; private set; }

    public Vector2 aimAxis { get; private set; }

    private bool _isActive = true;

    private bool _isInteract = false;
    private bool _isAbility = false;
    private bool _isDash = false;
    private bool _isShoot = false;

    private bool _isInteractUp = false;
    private bool _isAbilityUp = false;
    private bool _isShootUp = false;

    internal void Start()
    {
        _isActive = true;

        InputEvents.instance.OnMove += MoveCallback;
        InputEvents.instance.OnAim += AimCallback;
        InputEvents.instance.OnInteract += InteractCallback;
        InputEvents.instance.OnInteractUp += InteractUpCallback;
        InputEvents.instance.OnAbility += AbilityCallback;
        InputEvents.instance.OnDash += DashCallback;
        InputEvents.instance.OnShoot += ShootCallback;

        InputEvents.instance.OnInputActivate += InputActivateCallback;
        InputEvents.instance.OnInputSwitch += InputSwitchCallback;
    }

    internal void OnDestroy()
    {
        InputEvents.instance.OnMove -= MoveCallback;
        InputEvents.instance.OnAim -= AimCallback;
        InputEvents.instance.OnInteract -= InteractCallback;
        InputEvents.instance.OnInteractUp -= InteractUpCallback;
        InputEvents.instance.OnAbility -= AbilityCallback;
        InputEvents.instance.OnDash -= DashCallback;
        InputEvents.instance.OnShoot -= ShootCallback;

        InputEvents.instance.OnInputActivate -= InputActivateCallback;
        InputEvents.instance.OnInputSwitch -= InputSwitchCallback;
    }

    //internal void Update()
    //{
    //    if (!_isActive) return;

    //    Vector2 ax = Vector2.zero;

    //    if(playerInputIndex == 0)
    //    {
    //        if (Input.GetKey(KeyCode.D)) ax.x += 1;
    //        if (Input.GetKey(KeyCode.A)) ax.x -= 1;

    //        if (Input.GetKey(KeyCode.W)) ax.y += 1;
    //        if (Input.GetKey(KeyCode.S)) ax.y -= 1;
    //    }
    //    else if (playerInputIndex == 1)
    //    {
    //        if (Input.GetKey(KeyCode.RightArrow)) ax.x += 1;
    //        if (Input.GetKey(KeyCode.LeftArrow)) ax.x -= 1;

    //        if (Input.GetKey(KeyCode.UpArrow)) ax.y += 1;
    //        if (Input.GetKey(KeyCode.DownArrow)) ax.y -= 1;
    //    }

    //    axis = TransformAxis(ax);
    //    if (axis.magnitude != 0)
    //    {
    //        absAxis = axis;
    //    }

    //    _isInteract = playerInputIndex == 0 ? Input.GetKeyDown(KeyCode.E) : Input.GetKeyDown(KeyCode.Keypad7);
    //    _isAbility = playerInputIndex == 0 ? Input.GetKeyDown(KeyCode.F) : Input.GetKeyDown(KeyCode.Keypad8);
    //    _isDash = playerInputIndex == 0 ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown(KeyCode.Keypad5);

    //    _isInteractUp = playerInputIndex == 0 ? Input.GetKeyUp(KeyCode.E) : Input.GetKeyUp(KeyCode.Keypad7);

    //}

    internal void LateUpdate()
    {
        _isInteractUp = false;
        _isAbilityUp = false;
        _isShootUp = false;

        _isInteract = false;
        _isAbility = false;
        _isDash = false;
        _isShoot = false;

    }

    private void InputActivateCallback(bool isActive)
    {
        _isActive = isActive;

        if (!_isActive)
        {
            axis = Vector2.zero;
        }
    }

    private void InputSwitchCallback()
    {
        playerInputIndex = (playerInputIndex == 0 ? 1 : 0);
    }

    private Vector2 TransformAxis(Vector2 a)
    {
        float radians = -Camera.main.transform.eulerAngles.y * Mathf.PI / 180;
        float cos = Mathf.Cos(radians), sin = Mathf.Sin(radians);
        var ax = a;
        ax.x = cos * a.x - sin * a.y;
        ax.y = cos * a.y + sin * a.x;
        return ax;
    }

    private void MoveCallback(int index, Vector2 a)
    {
        if (!_isActive) return;

        if (index >= 2)
        {
            index -= 2;
            index = index == 0 ? 1 : 0;
        }
        if (index == inputIndex)
        {
            axis = TransformAxis(a);
            if (axis.magnitude != 0)
            {
                absAxis = axis;
            }
        }
    }

    private void AimCallback(int index, Vector2 a)
    {
        if (!_isActive) return;


        if (index == inputIndex)
        {
            a = TransformAxis(a);
            if(a.magnitude != 0)
            {
                aimAxis = a;
            }
        }
    }

    private void InteractCallback(int index)
    {
        if (!_isActive) return;
        if (index >= 2)
        {
            index -= 2;
            index = index == 0 ? 1 : 0;
        }
        if (index == inputIndex)
        {
            _isInteract = true;
        }
    }
    private void InteractUpCallback(int index)
    {
        if (!_isActive) return;
        if (index >= 2)
        {
            index -= 2;
            index = index == 0 ? 1 : 0;
        }
        if (index == inputIndex)
        {
            _isInteractUp = true;
        }
    }

    private void DashCallback(int index)
    {
        if (!_isActive) return;
        if (index >= 2)
        {
            index -= 2;
            index = index == 0 ? 1 : 0;
        }

        if (index == inputIndex)
        {
            _isDash = true;
        }
    }

    private void AbilityCallback(int index)
    {
        if (!_isActive) return;

        if (index >= 2)
        {
            index -= 2;
            index = index == 0 ? 1 : 0;
        }
        if (index == inputIndex)
        {
            _isAbility = true;
        }
    }
    private void ShootCallback(int index)
    {
        if (!_isActive) return;
        if (index >= 2)
        {
            index -= 2;
            index = index == 0 ? 1 : 0;
        }
        if (index == inputIndex)
        {
            _isShoot = true;
        }
    }

    public bool IsKeyDown(string key)
    {
        switch (key)
        {
            case "interact": return _isInteract;
            case "ability": return _isAbility;
            case "dash": return _isDash;
            case "shoot": return _isShoot;

        }
        return false;
    }

    public bool IsKeyUp(string key)
    {
        switch (key)
        {
            case "interact": return _isInteractUp;
            //ability
            //shoot
        }
        return false;
    }
}
