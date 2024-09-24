using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace EventServiceSystem
{
    public class EventService : MonoBehaviour
    {
        [SerializeField] private string _serverUrl;
        [SerializeField] private float _cooldownBeforeSend ;

        private List<Event> _events;
        private String _json;
        private Coroutine _sendRequestRoutine;

        private void Start()
        {
            if (SaveController.SaveRemainingEvents != null)
            {
                _events = SaveController.SaveRemainingEvents;

                if (_events.Count > 0)
                {
                    StartCoroutine(SendEventRoutine());
                }
            }
            else
            {
                _events = new List<Event>();
            }
        }
        
        private IEnumerator SendEventRoutine()
        {
            if (_events.Count > 0)
            {
                if (_sendRequestRoutine == null)
                {
                    _sendRequestRoutine = StartCoroutine(SendEventToServer());
                }
            }
            
            yield return new WaitForSeconds(_cooldownBeforeSend);
        }

        private IEnumerator SendEventToServer()
        {
            var oldEvents = _events;
             _json = JsonUtility.ToJson(_events);

            UnityWebRequest unityWebRequest = UnityWebRequest.Post(_serverUrl, _json);

            unityWebRequest.SetRequestHeader("Events", "application/json; charset=UFT-8");

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.responseCode == 200)
            {
                _events= (oldEvents.SelectMany(_ => _events, (oldEvent, newEvent) => new {oldEvent, newEvent})
                    .Where(arg => !_events.Contains(arg.oldEvent))
                    .Select(arg => arg.newEvent)).ToList();
            }

            _sendRequestRoutine = null;
        }

        public void TrackEvent(string type, string data)
        {
            Event @event = new Event(type, data);
            _events.Add(@event);
        }

        private void OnApplicationQuit()
        {
            SaveEvents();
        }

        private void SaveEvents()
        {
            SaveController.SaveRemainingEvents = _events;
        }
    }

    [Serializable]
    public class Event
    {
        public readonly string Type;
        public readonly string Data;

        public Event(string type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}
