using System.Collections.Generic;
using Data;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, AnimationDataSO> AnimationData { get; private set; } = new();

    protected override void Init()
    {
        base.Init();
        AnimationData = LoadScriptableObjects<AnimationDataSO>("SO/AnimationData/");
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        var textAsset = ResourceManager.Instance.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    private Dictionary<string, T> LoadScriptableObjects<T>(string path) where T : ScriptableObject
    {
        var dict = new Dictionary<string, T>();
        var assets = Resources.LoadAll<T>(path);
        foreach (var asset in assets) dict.Add(asset.name, asset);

        Debug.Log("SkillScriptableObject Load Complete");
        return dict;
    }
}