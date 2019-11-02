using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class EventHandler
    {
        public enum Events
        {
            OnBuildMenuToggle,
            ChooseTower_Ranged,
            ChooseTower_Melee,

            Wave_SpawnNew,
            Wave_EnemyDied,
            Wave_Finished
        }

        private Dictionary<Events, List<Action>> Subscriptions { get; set; }

        EventHandler()
        {
            Subscriptions = new Dictionary<Events, List<Action>>();
        }

        public void SubscribeToEvent(Events eventName, Action handler)
        {
            if (!Subscriptions.ContainsKey(eventName))
            {
                Subscriptions[eventName] = new List<Action>();
            }

            Subscriptions[eventName].Add(handler);
        }

        public void Dispatch(Events eventToDispatch)
        {
            if (Subscriptions.ContainsKey(eventToDispatch))
            {
                Subscriptions[eventToDispatch].ForEach(e => e());
            }
        }
        
        #region Singleton

        private static EventHandler _instance;

        public static EventHandler Current
        {
            get
            {
                if (_instance == null)
                    _instance = new EventHandler();

                return _instance;

            }
        }

        #endregion
    }
}
