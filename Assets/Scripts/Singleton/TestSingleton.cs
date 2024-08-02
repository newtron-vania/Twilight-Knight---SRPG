using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton : Singleton<TestSingleton>
{
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

   protected TestSingleton()
   {
      
   }
   private Queue<int> data = null;

   private Queue<int> LoadData()
   {
      data = new Queue<int>();
      for (int i = 1; i < 50000; i++)
      {
         data.Enqueue(i);
      }

      return data;
   }
   public Queue<int> GetData()
   {
      return data;
   }
}
