using UnityEngine;
using Zenject;

namespace Runtime.Infrastructures
{
    public class ApplicationSettings : IInitializable
    {
        public void Initialize()
        {
            int targetFrameRate = 60;
            Application.targetFrameRate = targetFrameRate;
            Application.runInBackground = false;
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}