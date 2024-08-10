using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        var component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        var transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (!recursive)
            for (var i = 0; i < go.transform.childCount; i++)
            {
                var transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    var component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        else
            foreach (var component in go.GetComponentsInChildren<T>(true))
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;

        return null;
    }

    //public static bool CheckContainsKey(Dictionary<object, object> dict, object key, object value)
    //{
    //    if (dict.Keys.GetType() != key.GetType())
    //    {
    //        Debug.Log("unVaild type of key");
    //        return false;
    //    }
    //    else if(dict.Values.GetType() != value.GetType())
    //    {
    //        Debug.Log("unVaild type of value");
    //        return false;
    //    }
    //    else if (!dict.ContainsKey(key))
    //    {
    //        Debug.Log($"There's no key in weaponDict");
    //        return false;
    //    }
    //    else
    //    {
    //        Debug.Log("Fine");
    //        return true;
    //    }
    //}
}