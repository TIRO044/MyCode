using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace JobTest
{
    public static class JobEventHandler
    {
        private static Dictionary<int, object> _jobs = new Dictionary<int,object>();

        private static int _handle;
        
        public static int Register(object e)
        {
            var handle = _handle;
            _jobs.Add(handle, e);
            _handle++;
            return handle;
        }

        public static T GetCallBack<T>(int handle)
        {
            if(_jobs.TryGetValue(handle, out var e))
            {
                return (T)e;
            }

            return default;
        } 
    }

    public interface IJobImpl
    {
        int Handle { get; }
    }

    public class JobEventManagerTest<T> : IJobImpl where T : struct
    {
        private readonly HashSet<CallBack2.DataDelegate2<IJobResult<T>>> _delegate = new();
        
        public JobEventManagerTest()
        {
            var callback = new CallBack2.DataDelegate2<IJobResult<T>>(Excute);
            Handle = JobEventHandler.Register(callback);
        }
        
        private void Excute(in IJobResult<T> jobResult)
        {
            foreach (var t in _delegate)
            {
                t(jobResult);
            }
        }

        public int Handle { get; private set; }
    }
    
    public struct CallBack2
    {
        public delegate void DataDelegate2<T>(in T data);
    }
    
    public interface IJobResult<T> where T : struct
    {
        NativeArray<T> Result { get; }
    }
    
    
    public struct JobEvent1<T> : IJob, IJobResult<T> where T : struct
    {
        public NativeArray<T> Result { get; set; }
        public JobHandle JobHandle;
        public int EventHandle;
        
        // 결과만 invoke
        public void Execute()
        {
            // var callback = JobEventHandler.GetCallBack<CallBack2.DataDelegate2<T>>(EventHandle);
            // callback.Invoke(Result);
        }

        public bool IsDone()
        {
            return JobHandle.IsCompleted;
        }
    }
    
    public struct JobEvent<T> : IJob, IJobResult<T> where T : struct
    {
        public NativeArray<T> Result { get; set; }
        public JobHandle JobHandle;
        public int EventHandle;
        public Action Action;
        
        public void Execute()
        {

            //......
            Action?.Invoke();
            
            var callback = JobEventHandler.GetCallBack<CallBack2.DataDelegate2<T>>(EventHandle);
            // callback.Invoke();
        }

        public void RegisterJob()
        {
            JobHandle = this.Schedule();
            JobHandle.Complete();
        }
        
        public bool IsDone()
        {
            return JobHandle.IsCompleted;
        }
    }

    public class Test
    {
        public void Do()
        {
            var t = new JobEvent<int>();
            t.RegisterJob();
        }
    }
}
