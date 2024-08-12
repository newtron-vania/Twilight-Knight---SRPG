using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Modules.EventSystem
{
    public interface IEventListener
    {
        // 특정 이벤트가 발생했을 때 호출되는 메서드
        void OnEvent(EventType eventType, Component sender, object parameter = null);
    }
    
    public class EventManager : Singleton<EventManager>
    {
        // 이벤트 리스너들을 관리하는 딕셔너리
        private Dictionary<EventType, List<IEventListener>> listeners = new Dictionary<EventType, List<IEventListener>>();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// 씬이 로드되었을 때 호출되는 메서드
        /// </summary>
        /// <param name="scene">로드된 씬</param>
        /// <param name="mode">로드 모드</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬 변경 시 리스너 목록을 업데이트
            RefreshListeners();
        }

        /// <summary>
        /// 이벤트 리스너를 등록합니다.
        /// </summary>
        /// <param name="eventType">등록할 이벤트 타입</param>
        /// <param name="listener">이벤트 리스너</param>
        public void AddListener(EventType eventType, IEventListener listener)
        {
            if (listeners.TryGetValue(eventType, out var listenerList))
            {
                // 해당 이벤트 타입에 리스너 추가
                listenerList.Add(listener);
            }
            else
            {
                listenerList = new List<IEventListener> { listener };
                listeners.Add(eventType, listenerList);
            }
        }

        /// <summary>
        /// 특정 이벤트를 모든 리스너에게 알립니다.
        /// </summary>
        /// <param name="eventType">알릴 이벤트 타입</param>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="parameter">전달할 추가 데이터</param>
        public void PostNotification(EventType eventType, Component sender, object parameter = null)
        {
            if (!listeners.TryGetValue(eventType, out var listenerList))
                return;

            // 각 리스너에게 이벤트를 전송
            foreach (var listener in listenerList)
            {
                listener?.OnEvent(eventType, sender, parameter);
            }
        }

        /// <summary>
        /// 특정 이벤트 타입에 대한 리스너들을 제거합니다.
        /// </summary>
        /// <param name="eventType">제거할 이벤트 타입</param>
        public void RemoveEvent(EventType eventType)
        {
            listeners.Remove(eventType);
        }

        /// <summary>
        /// 현재 리스너 목록을 새로 고칩니다.
        /// </summary>
        private void RefreshListeners()
        {
            // 유효한 리스너만 남기기 위한 임시 딕셔너리
            var tempListeners = new Dictionary<EventType, List<IEventListener>>();

            // null이 아닌 리스너만 남기고 나머지를 제거
            foreach (var item in listeners)
            {
                item.Value.RemoveAll(listener => listener == null);

                if (item.Value.Count > 0)
                    tempListeners.Add(item.Key, item.Value);
            }
            
            // 새로 고친 리스너 목록을 적용
            listeners = tempListeners;
        }
    }
}