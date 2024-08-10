using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<Define.PopupUIGroup, Stack<UI_Popup>> _popupStackDict = new();
    private int _order = 10;
    private UI_Scene _sceneUI;

    public GameObject Root()
    {
        var root = GameObject.Find("@UI_Root");
        if (root == null)
            root = new GameObject { name = "@UI_Root" };

        return root;
    }

    public void SetCanvas(GameObject go, bool sorting = false)
    {
        var canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sorting)
            canvas.sortingOrder = _order++;
        else
            canvas.sortingOrder = 0;
    }

    public T MakeWorldSpaceUI<T>(Transform parent, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        var go = ResourceManager.Instance.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        //return go.GetOrAddComponent<T>();
        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        var go = ResourceManager.Instance.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;


        var go = ResourceManager.Instance.Instantiate($"UI/Scene/{name}");

        var sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;


        go.transform.SetParent(Root().transform);
        return sceneUI;
    }

    public UI_Scene getSceneUI()
    {
        return _sceneUI;
    }

    public T ShowPopupUI<T>(string name = null, bool forEffect = false) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        var go = ResourceManager.Instance.Instantiate($"UI/Popup/{name}");

        go.transform.SetParent(Root().transform);

        var popup = go.GetOrAddComponent<T>();
        var popupType = popup.PopupID;

        if (!_popupStackDict.ContainsKey(popupType))
            _popupStackDict.Add(popupType, new Stack<UI_Popup>());

        _popupStackDict[popupType].Push(popup);

        if (forEffect)
            popup.GetComponent<Canvas>().sortingOrder = -1;


        return popup;
    }

    public void ClosePopupUI(Define.PopupUIGroup popupType)
    {
        if (!_popupStackDict.TryGetValue(popupType, out var popupStack)
            || _popupStackDict[popupType].Count == 0)
            return;

        var popup = _popupStackDict[popupType].Pop();
        ResourceManager.Instance.Destroy(popup.gameObject);
        _order--;
        popup = null;

        CheckPopupUICountAndRemove();
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        var popupType = popup.PopupID;
        if (!_popupStackDict.TryGetValue(popupType, out var popupStack)
            || _popupStackDict[popupType].Count == 0)
            return;

        if (popup != popupStack.Peek())
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI(popupType);
    }

    public void CloseAllPopupUI()
    {
        foreach (var kv in _popupStackDict)
        {
            var popupType = kv.Key;
            var popupStack = kv.Value;
            while (popupStack.Count != 0)
            {
                var popup = popupStack.Pop();
                ResourceManager.Instance.Destroy(popup.gameObject);
                _order--;
                popup = null;
            }
        }

        CheckPopupUICountAndRemove();
    }

    public void CloseAllGroupPopupUI(Define.PopupUIGroup popupType)
    {
        if (!_popupStackDict.TryGetValue(popupType, out var popupStack)
            || _popupStackDict[popupType].Count == 0)
            return;

        while (popupStack.Count != 0)
        {
            var popup = popupStack.Pop();
            ResourceManager.Instance.Destroy(popup.gameObject);
            _order--;
            popup = null;
        }

        CheckPopupUICountAndRemove();
    }


    private void CheckPopupUICountAndRemove()
    {
        var popupType = new List<Define.PopupUIGroup>();
        foreach (var popupUI in _popupStackDict.Keys) popupType.Add(popupUI);
        for (var i = 0; i < _popupStackDict.Count; i++)
            if (_popupStackDict.GetValueOrDefault(popupType[i]).Count == 0)
                _popupStackDict.Remove(popupType[i]);
        CheckPopupUICountInScene();
    }

    private void CheckPopupUICountInScene()
    {
        Debug.Log($"popupCount : {_popupStackDict.Count}");
        foreach (var popupKey in _popupStackDict.Keys) Debug.Log($"popupList : {popupKey}");
    }

    public int GetPopupUICount()
    {
        return _popupStackDict.Count;
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}