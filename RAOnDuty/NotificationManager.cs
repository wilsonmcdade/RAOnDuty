using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace RAOnDuty {
    public class Notification {
        public int timeLeft;
        public string message;
        public Notification(string _message) {
            timeLeft = 500;
            message = _message;
        }
    }
    public class NotificationManager {
        private List<Notification> activeNotifications;
        private SpriteFont pixelfont;
        private Texture2D bubbleleft;
        private Texture2D bubbleright;
        private Texture2D bubblemiddle;
        private int BUFFER = 6;
        private int SCALE = 22;

        public NotificationManager() {
            activeNotifications = new List<Notification>();
        }

        public void LoadContent(ContentManager Content) {
            pixelfont = Content.Load<SpriteFont>("pixelfont");
            bubbleleft = Content.Load<Texture2D>("bubbleleft");
            bubbleright = Content.Load<Texture2D>("bubbleright");
            bubblemiddle = Content.Load<Texture2D>("bubblemiddle");
        }

        public void ShowNotification(string message) {
            activeNotifications.Add(new Notification(message));
        }

        public void Update(GameTime gameTime) {
            for(int i = 0; i < activeNotifications.Count; i++) {
                if(activeNotifications[i].timeLeft == 0) {
                    activeNotifications.Remove(activeNotifications[i]);
                    break;
                }
                activeNotifications[i].timeLeft--;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
			spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.None,null,null);
            for(int i = 0; i < activeNotifications.Count; i++) {
                Vector2 fontsize = new Vector2((int) pixelfont.MeasureString(activeNotifications[i].message).X-SCALE/2,pixelfont.MeasureString(activeNotifications[i].message).Y);
                spriteBatch.Draw(
                    bubbleright,
                    new Rectangle(graphics.PreferredBackBufferWidth-BUFFER-SCALE,3*graphics.PreferredBackBufferHeight/4,SCALE,SCALE*3),
                    Color.SkyBlue);
                spriteBatch.Draw(
                    bubblemiddle,
                    new Rectangle(graphics.PreferredBackBufferWidth-BUFFER-SCALE-(int)fontsize.X,3*graphics.PreferredBackBufferHeight/4,(int)fontsize.X,SCALE*3),
                    Color.SkyBlue);
                spriteBatch.Draw(
                    bubbleleft,
                    new Rectangle(graphics.PreferredBackBufferWidth-BUFFER-SCALE-SCALE/2-(int)fontsize.X,3*graphics.PreferredBackBufferHeight/4,SCALE/2,SCALE*3),
                    Color.SkyBlue);
                spriteBatch.DrawString(
                    pixelfont,
                    activeNotifications[i].message,
                    new Vector2(graphics.PreferredBackBufferWidth-SCALE/4-SCALE-(int)fontsize.X,
                    3*graphics.PreferredBackBufferHeight/4+BUFFER),
                    Color.Black);
            }
            spriteBatch.End();
        }
    }
}