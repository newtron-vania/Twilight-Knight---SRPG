using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Action KeyAction = null;

    private void Update()
    {
        if (Input.anyKey == false)
            return;

        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
