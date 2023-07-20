using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace JobTest
{
    public static class JobEventHandler
    {
        private static Dictionary<int, object> _jobs = new Dictionary<int, object>();

        private static int _handle;
        
        public static int Register(IJobImpl jobImpl)
        {
            var handle = _handle;
            _jobs.Add(handle, jobImpl);
            _handle++;
            return handle;
        }

        public static T GetCallBack<T>(int handle) where T : class
        {
            if(_jobs.TryGetValue(handle, out var e))
            {
                return (T)e;
            }

            return null;
        } 
    }

    public interface IJobImpl
    {
        void AddEvent();
        void RemoveEvent();

        void Excute();
        
        int Handle { get; }
    }
    
    public class JobEventManagerTest : IJobImpl
    {
        public JobEventManagerTest()
        {
            Handle = JobEventHandler.Register(this);
        }

        public void AddEvent()
        {
        }

        public void RemoveEvent()
        {
        }

        public void Excute()
        {
        }

        public int Handle { get; private set; }
    }

    public class Test
    {
        
    }
    
    public class CallBack
    {
        public delegate void VoidDelegate();
        public delegate void DataDelegate<T>(T data);
        public delegate void DataDelegate2<T>(T data) where T : struct;
    }
    
    public interface IJobResult<T> where T : struct
    {
        NativeArray<T> Result { get; set; }
    }
    
    public struct JobEvent<T> : IJob, IJobResult<T> where T : struct
    {
        public bool Used;
        public NativeArray<T> Result { get; set; }
        public JobHandle JobHandle;
        public int EventHandle;
        
        // 결과만 invoke
        public void Execute()
        {
            var callback = JobEventHandler.GetCallBack<CallBack.DataDelegate<IJobResult<T>>>(EventHandle);
            callback?.Invoke(this);
        }

        public bool IsDone()
        {
            return JobHandle.IsCompleted;
        }
    }
    
    public class JobEventManager
    {
        
    }
    
    public class JobSystemTest : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            NativeArray<float> result = new NativeArray<float>();
            
            var jobData = new MyJob();
            jobData.A = 10;
            jobData.B = 10;
            jobData.Result = result;

            var jobHandle = jobData.Schedule();

            // wait for jobs
            jobHandle.Complete();

            var plusB = result[0];
            
            result.Dispose();
        }
    }
    
    public struct MyJob : IJob
    {
        public float A;
        public float B;
        public NativeArray<float> Result;

        public void Execute()
        {
            Result[0] = A + B;
        }
    }
}
