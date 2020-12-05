using System;
using System.Collections.Generic;
using System.Text;

namespace EventApp
{
    enum EventType
    {
        Coffee,
        Lecture,
        Concert,
        StudySession
    }
    class Event
    {
        public string Name { get; set; }
        //public enum EventType { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public EventType EventTip;
        public Event(string name, DateTime startTime, DateTime endTime, EventType eventTip)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            EventTip = eventTip; 
        }


    }
}
