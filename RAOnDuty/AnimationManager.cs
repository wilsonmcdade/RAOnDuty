using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace RAOnDuty
{
    public class Animation {
        public string Name;
        public List<Texture2D> Animations;
        public int CurrentFrame;
        public bool Active;
        public int TimeDelay;
        public Animation(string _name, int _timeDelay, List<Texture2D> _animations) {
            Name = _name;
            Animations = _animations;
            CurrentFrame = 0;
            Active = false;
            TimeDelay = _timeDelay;
        }
        public void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.Milliseconds % TimeDelay == 0) {
                NextFrame();
            }
        }

        public void NextFrame() {
            if (!Active)
                StopAnimation();
            CurrentFrame = (CurrentFrame + 1) % Animations.Count;
            if (CurrentFrame == Animations.Count - 1) {
                Active = false;
            }
        }

        public void PlayAnimation() {
            Active = true;
        }
        
        public void StopAnimation() {
            Active = false;
            CurrentFrame = 0;
        }

        public Texture2D GetCurrentFrame() {
            //Console.WriteLine(CurrentFrame);
            return Animations[CurrentFrame];
        }
    }
	public class AnimationManager {
        private List<Animation> Animations;
        public AnimationManager() {
            Animations = new List<Animation>();
        }

        public void AddAnimation(string _name, int _timeDelay, List<Texture2D> _animations) {
            Animations.Add(new Animation(_name, _timeDelay, _animations));
        }

        public Texture2D GetCurrentFrame(string _name) {
            foreach(Animation anim in Animations) {
                if(anim.Name == _name) {
                    return anim.GetCurrentFrame();
                }
            }
            return null;
        }

        private Animation GetAnimation(string _name) {
            foreach(Animation anim in Animations) {
                if(anim.Name == _name) {
                    return anim;
                }
            }
            return null;

        }

        public void PlayAnimation(string _name) {
            Animation anim = GetAnimation(_name);
            anim.PlayAnimation();                
        }

        public Texture2D GetNextFrame(string _name) {
            Animation anim = GetAnimation(_name);
            anim.NextFrame();
            return anim.GetCurrentFrame();
        }

        public void Update(GameTime gameTime)
        {
            foreach(Animation anim in Animations) {
                anim.Update(gameTime);
            }
        }
    }
}