using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SoundManager.Instance.Play("Happy", Define.Sound.effect, 1F);
        int sum = 0;
        var data = TestSingleton.Instance.GetData();

        if (data == null) return;
        while (data.Count > 0)
        {
            sum += data.Dequeue();
        }
        
        Debug.Log(sum);
    }

}