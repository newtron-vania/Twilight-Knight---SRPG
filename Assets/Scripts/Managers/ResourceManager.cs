using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            var name = path;
            var index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);
        }

        return Resources.Load<T>(path);
    }

    public Sprite LoadSprite(string name)
    {
        var path = $"Prefabs/SpriteIcon/{name}";

        var original = Resources.Load<Sprite>(path);
        if (original == null)
        {
            Debug.Log($"Faild to sprite : {path}");
            return null;
        }

        return original;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        var original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Faild to load prefab : {path}");
            return null;
        }


        var go = Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public GameObject Instantiate(string path, Vector3 position, Transform parent = null)
    {
        var original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Faild to load prefab : {path}");
            return null;
        }

        var go = Instantiate(original, position, Quaternion.identity, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject obj, float time = 0)
    {
        if (obj == null) return;

        Object.Destroy(obj, time);
    }
}