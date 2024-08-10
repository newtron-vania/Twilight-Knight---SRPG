using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class UI_Base : MonoBehaviour
{
    private readonly Dictionary<Type, Object[]> _objects = new();

    private void Awake()
    {
        Init();
    }

    public abstract void Init();

    protected void Bind<T>(Type type) where T : Object
    {
        var name = Enum.GetNames(type);

        if (_objects.ContainsKey(typeof(T)))
            return;

        var objects = new Object[name.Length];

        _objects.Add(typeof(T), objects);


        for (var i = 0; i < name.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, name[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, name[i], true);

            if (objects[i] == null)
                Debug.Log($"Fail to Bind {name[i]}!");
        }
    }

    protected T Get<T>(int idx) where T : Object
    {
        Object[] objects = null;
        _objects.TryGetValue(typeof(T), out objects);
        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx)
    {
        return Get<GameObject>(idx);
    }

    protected TextMeshProUGUI GetText(int idx)
    {
        return Get<TextMeshProUGUI>(idx);
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Image GetImage(int idx)
    {
        return Get<Image>(idx);
    }

    public static void BindUIEvent(GameObject go, Action<PointerEventData> action,
        Define.UIEvent type = Define.UIEvent.Click)
    {
        var evt = Util.GetOrAddComponent<EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}