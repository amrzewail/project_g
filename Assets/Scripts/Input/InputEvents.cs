using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : Singleton<InputEvents>
{
    public Action<bool> OnInputActivate;

    public Action OnInputSwitch;

    public Action<int, Vector2> OnMove;

    public Action<int, Vector2> OnAim;

    public Action<int> OnInteract;

    public Action<int> OnAbility;
    public Action<int> OnDash;

    public Action<int> OnShoot;

    public Action<int> OnClearInput;

    public Action<int> OnInteractUp;

}
