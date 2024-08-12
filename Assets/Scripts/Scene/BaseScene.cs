using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public abstract class BaseScene : MonoBehaviour
{
    public abstract Define.SceneType _sceneType { get; }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
            ResourceManager.Instance.Instantiate("UI/EventSystem").name = "@EventSystem";

    }

    public abstract void Clear();
}
