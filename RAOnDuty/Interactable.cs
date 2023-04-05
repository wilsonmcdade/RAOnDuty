using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RAOnDuty {
    public class Interactable {
        public Rectangle Position;
        private Animation Animations;
        public Interactable(Rectangle _position, List<Animation> _animations) {
            Position = _position;
        }
    } 
    public class InteractableManager {
        private List<Interactable> Interactables;

        public InteractableManager() {
            Interactables = new List<Interactable>();
        }

        public void AddInteractable(Rectangle _position, List<Animation> _animation) {
            Interactables.Add(new Interactable(_position, _animation));
        }

        public List<Interactable> GetInteractable(Rectangle Position) {
            return Interactables;
        }

        public void Draw(SpriteBatch spriteBatch){

        }
    }
}