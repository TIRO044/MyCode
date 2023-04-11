using System;
using System.Collections;
using System.IO;
using System.Net;
using NUnit.Framework;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UniRxTest
{
    using UniRx;
    public class NewTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IntReactiveProperty()
        {
            var reactive = new IntReactiveProperty(10);
            reactive.Where(v => v == 15).Subscribe(v => Debug.Log($"intReactive _ {v}"));
            reactive.Where(v => v == 13).Subscribe(v => Debug.Log($"intReactive _ {v}"));
        }

        [Test]
        public void ReactiveCollection()
        {
            var collection = new ReactiveCollection<string>();
            collection.ObserveAdd().Subscribe(x => {Debug.Log($"add _ {x}");});
            collection.ObserveRemove().Subscribe(x => { Debug.Log($"remove _ {x}"); });

            collection.Add("t1");
            collection.Add("t2");
            collection.Add("t3");
            collection.Add("t4");
            collection.Remove("t4");
        }

        [Test]
        public void ButtonOnClickASObservable()
        {
            var go = new GameObject();
            var button = go.AddComponent<Button>();
            button.onClick.AsObservable().Subscribe(_ => Debug.Log("Click Button")).AddTo(go);
            button.onClick.Invoke();

            Object.DestroyImmediate(go);
        }
        
        [Test]
        public void ObservableCreate()
        {
            // 이거는 뭐라고 해야하나.. Reactive한 개체가 없더라도 만들어 쓸 수 있다.
            Observable.Create<int>(ob =>
            {
                Debug.Log($"{ob}");

                for (var i = 0; i < 100; i++)
                {
                    ob.OnNext(i);
                }

                Debug.Log("Finished");

                return Disposable.Create(() =>
                {
                    Debug.Log("Dispose");
                });
            }).Subscribe(x => Debug.Log(x));
        }

        [Test]
        public void ObservableStart()
        {
            // 다른 쓰레드에서 실행
            // Start를 따라가보면, 스케줄러에서 ThreadPool을 사용하는 걸 볼 수 있다.
            Observable.Start(() =>
            {
                var req = (HttpWebRequest)WebRequest.Create("https://google.com");
                var res = (HttpWebResponse)req.GetResponse();
                using var reader = new StreamReader(res.GetResponseStream());
                return reader.ReadToEnd();
            })
            .ObserveOnMainThread()  // 이건 다른 쓰레드에서 실행할 때 필수적으로 해야 한다는디, 스케줄러를 보면 IObservable의 스케줄러를 AsyncConversions로 바꾸는뎅 이걸 다시 메인 쓰레드로 가져오네
            .Subscribe(x => Debug.Log($"{x}")); // 오 ㅋ

            Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(_ => Debug.Log($"5초 경과"));
            Observable.Timer(TimeSpan.FromSeconds(7)).Subscribe(_ => Debug.Log($"7초 경과"));
        }

        [Test]
        public void Trigger()
        {
            var isForced = true;
            var gameobject = new GameObject();
            gameobject.name = "ddddddddddddddddddddddddddd";
            var rb = gameobject.AddComponent<Rigidbody>();

            gameobject.FixedUpdateAsObservable()
                .Where(_ => isForced)
                .Subscribe(_ => rb.AddForce(Vector3.up * 20));

            gameobject.FixedUpdateAsObservable()
                .Where(_ => gameobject.CompareTag("WarpZone"))
                .Subscribe(_ => isForced = true);


            gameobject.FixedUpdateAsObservable()
                .Where(_ => gameobject.CompareTag("WarpZone"))
                .Subscribe(_ => isForced = false);

            Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(_ => Object.DestroyImmediate(gameobject));
        }

        [Test]
        public void FromCoroutine()
        {
            IEnumerator GetTimerCoroutine(IObserver<int> Observer, int initialCount)
            {
                var count = initialCount;
                while (count > 0)
                {
                    Observer.OnNext(count--);

                    yield return new WaitForSeconds(1);
                }

                Observer.OnNext(0);
                Observer.OnCompleted();
            }

            Observable
                .FromCoroutine<int>(observe => GetTimerCoroutine(observe, 100))
                .Subscribe(t => Debug.Log(t));
        }
    }
}
