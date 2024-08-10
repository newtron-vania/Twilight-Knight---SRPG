using System.Collections;
using System.Collections.Generic;
using Data;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, Data.AnimationDataSO> AnimationData { get; private set; } = new Dictionary<string, AnimationDataSO>();
    
    protected override void Init()
    {
        base.Init();
        AnimationData = LoadScriptableObjects<AnimationDataSO>("SO/AnimationData/");
    }
    
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
    
    Dictionary<string, T> LoadScriptableObjects<T>(string path) where T : ScriptableObject
    {
        Dictionary<string, T> dict = new Dictionary<string, T>();
        T[] assets = Resources.LoadAll<T>(path);
        foreach (T asset in assets)
        {
            dict.Add(asset.name, asset);
        }

        Debug.Log("SkillScriptableObject Load Complete");
        return dict;
    }
}
