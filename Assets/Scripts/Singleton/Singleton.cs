using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new();
    private static bool _applicationQuit;

    public static T Instance
    {
        get
        {
            // 고스트 객체 생성 방지용
            // 위에서 설명한 leak을 방지한다.
            if (_applicationQuit)
                // null 리턴
                return null;

            // thread-safe
            lock (_lock)
            {
                if (_instance == null)
                {
                    // 현재 씬에 싱글톤이 있나 찾아본다.
                    _instance = FindObjectOfType<T>();

                    // 없으면
                    if (_instance == null)
                    {
                        // 해당 컴포넌트 이름을 가져온다.
                        var componentName = typeof(T).ToString();

                        // 해당 컴포넌트 이름으로 게임 오브젝트 찾기
                        var findObject = GameObject.Find(componentName);

                        // 없으면
                        if (findObject == null)
                            // 생성
                            findObject = new GameObject(componentName);

                        // 생성된 오브젝트에, 컴포넌트 추가
                        _instance = findObject.AddComponent<T>();
                        // 씬이 변경되어도 객체가 유지되도록 설정
                        DontDestroyOnLoad(_instance);
                    }

                    (_instance as Singleton<T>).Init();
                }

                // 객체 리턴
                return _instance;
            }
        }
    }

    // 객체가 파괴될때 호출
    public virtual void OnDestroy()
    {
        _applicationQuit = true;
    }

    // 앱이 종료될때 호출
    protected virtual void OnApplicationQuit()
    {
        _applicationQuit = true;
    }

    // 원칙적으로 싱글톤은 응용 프로그램이 종료될때, 소멸되어야 한다.
    // 유니티에서 응용 프로그램이 종료되면 임의 순서대로 오브젝트가 파괴된다.
    // 만약 싱글톤 오브젝트가 파괴된 이후, 싱글톤 오브젝트가 호출된다면
    // 앱의 재생이 정지된 이후에도, 에디터 씬에서 고스트 객체가 생성된다.
    // 고스트 객체의 생성을 방지하기 위해서 상태를 관리한다.

    protected virtual void Init()
    {
    }
}