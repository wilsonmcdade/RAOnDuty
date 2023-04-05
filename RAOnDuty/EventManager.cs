using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace RAOnDuty {
    public class Event {
        public Event(EventManager.Events type) {

        }
    }
    public class EventManager {
        Random rand = new Random();
        public enum Events {
            ServerFire,
            DrinkingParty,
            IguanaRoom,
            //AlligatorLoose,
            //CapybaraServerRoom,
            //Hooligans,
            //Flood,
            //NerfWar,
            //PowerOutage,
        }
        public EventManager() {

        }

        public void GenerateEvent() {
            
        }
    }
}