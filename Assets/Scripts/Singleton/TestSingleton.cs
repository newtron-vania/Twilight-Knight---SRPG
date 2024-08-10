using System.Collections.Generic;

public class TestSingleton : Singleton<TestSingleton>
{
    private Queue<int> data;

    protected TestSingleton()
    {
    }

    public static TestSingleton Instance
    {
        get
        {
            var instance = Singleton<TestSingleton>.Instance;

            if (instance.data == null)
                instance.LoadData();

            return instance;
        }
    }

    private Queue<int> LoadData()
    {
        data = new Queue<int>();
        for (var i = 1; i < 50000; i++) data.Enqueue(i);

        return data;
    }

    public Queue<int> GetData()
    {
        return data;
    }
}