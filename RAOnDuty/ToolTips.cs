using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Devcade;

namespace RAOnDuty
{
  public class ToolTip {
    public string message;
    public ToolTips.Position position;
    public Input.ArcadeButtons control;
    public bool active;
    public int timeLeft;
    private int screenWidth;
    private int screenHeight;
    public int offset;
    private int SCALE = 16;
    private int BUFFER = 10;

    public ToolTip(Input.ArcadeButtons _control, ToolTips.Position _position, string _message, GraphicsDeviceManager _graphics, int _offset) {
      control = _control;
      message = _message;
      position = _position;
      screenWidth = _graphics.PreferredBackBufferWidth;
      screenHeight = _graphics.PreferredBackBufferHeight;
      active = true;
      timeLeft = 500;
      offset = _offset;
    }

    public Rectangle GetIconRectangle() {
      Rectangle tipRectangle = GetRectangle();
      return new Rectangle(tipRectangle.X + BUFFER/2, tipRectangle.Y + BUFFER/2, SCALE, SCALE);
    }

    public Rectangle GetRectangle() {
      Vector2 fontsize = ToolTips.pixelfont.MeasureString(message);
      switch(position){
        case ToolTips.Position.UpperLeft:
          return new Rectangle(
            BUFFER,
            BUFFER + ((int) fontsize.Y + 2*BUFFER) * (offset - 1),
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        case ToolTips.Position.UpperRight:
          return new Rectangle(
            screenWidth - (int) fontsize.X - SCALE - 3*BUFFER,
            BUFFER + ((int) fontsize.Y + 2*BUFFER) * (offset - 1),
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        case ToolTips.Position.UpperMiddle:
          return new Rectangle(
            screenWidth/2 - ((int) fontsize.X + SCALE + 2*BUFFER)/2,
            BUFFER + ((int) fontsize.Y + 2*BUFFER) * (offset - 1),
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        case ToolTips.Position.BottomRight:
          return new Rectangle(
            screenWidth - (int) fontsize.X - SCALE - 3*BUFFER,
            screenHeight - ((int) fontsize.Y + 2*BUFFER) * offset,
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        case ToolTips.Position.BottomLeft:
          return new Rectangle(
            BUFFER,
            screenHeight - ((int) fontsize.Y + 2*BUFFER) * offset,
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        case ToolTips.Position.BottomMiddle:
          return new Rectangle(
            screenWidth/2 - ((int) fontsize.X + SCALE + 2*BUFFER)/2,
            screenHeight - ((int) fontsize.Y + 2*BUFFER) * offset,
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        case ToolTips.Position.Middle:
          return new Rectangle(
            screenWidth/2 - ((int) fontsize.X + SCALE + 2*BUFFER)/2,
            screenHeight/2 - ((int) fontsize.Y - 2*BUFFER)/2 - ((int) fontsize.Y + 2*BUFFER) * offset,
            (int) fontsize.X + SCALE + 2*BUFFER,
            (int) fontsize.Y + BUFFER);
        default:
          return new Rectangle(0,0,0,0);
      }
    }
    public Vector2 GetStringPosition() {
      return new Vector2(GetIconRectangle().X + GetIconRectangle().Width + BUFFER,GetIconRectangle().Y);
    }
  }
  public class ToolTips
  {
    private List<ToolTip> activeToolTips;
    private Texture2D blank;
    private Texture2D A1;
    private Texture2D A2;
    private Texture2D A3;
    private Texture2D A4;
    private Texture2D B1;
    private Texture2D B2;
    private Texture2D B3;
    private Texture2D B4;
    private Texture2D P1Stick;
    private Texture2D P1StickDown;
    private Texture2D P1StickLeft;
    private Texture2D P1StickUp;
    private Texture2D P1StickRight;
    private Texture2D P2Stick;
    private Texture2D P2StickDown;
    private Texture2D P2StickLeft;
    private Texture2D P2StickUp;
    private Texture2D P2StickRight;
    public static SpriteFont pixelfont;

    public ToolTips() {
      activeToolTips = new List<ToolTip>();
    }

    public enum Position 
    {
      UpperRight,
      UpperLeft,
      UpperMiddle,
      BottomRight,
      BottomLeft,
      BottomMiddle,
      Middle
    }

    public void Initialize() {

    }

    public void LoadContent(ContentManager Content) {
      blank = Content.Load<Texture2D>("blank");
      pixelfont = Content.Load<SpriteFont>("pixelfont");
      A1 = Content.Load<Texture2D>("tooltips/A1");
      A2 = Content.Load<Texture2D>("tooltips/A2");
      A3 = Content.Load<Texture2D>("tooltips/A3");
      A4 = Content.Load<Texture2D>("tooltips/A4");
      B1 = Content.Load<Texture2D>("tooltips/P1B");
      B2 = Content.Load<Texture2D>("tooltips/P1B");
      B3 = Content.Load<Texture2D>("tooltips/P1B");
      B4 = Content.Load<Texture2D>("tooltips/P1B");
      P1StickLeft = Content.Load<Texture2D>("tooltips/P1StickLeft");
      P1StickRight = Content.Load<Texture2D>("tooltips/P1StickRight");
      P1StickUp = Content.Load<Texture2D>("tooltips/P1StickUp");
      P1StickDown = Content.Load<Texture2D>("tooltips/P1StickDown");

    }

    public Texture2D GetIcon(Input.ArcadeButtons control) {
      switch (control) {
        case(Input.ArcadeButtons.A1):
          return A1;
        case(Input.ArcadeButtons.A2):
          return A2;
        case(Input.ArcadeButtons.A3):
          return A3;
        case(Input.ArcadeButtons.A4):
          return A4;
        case(Input.ArcadeButtons.B1):
          return B1;
        case(Input.ArcadeButtons.B2):
          return B2;
        case(Input.ArcadeButtons.B3):
          return B3;
        case(Input.ArcadeButtons.B4):
          return B4;
        case(Input.ArcadeButtons.StickLeft):
          return P1StickLeft;
        case(Input.ArcadeButtons.StickRight):
          return P1StickRight;
        case(Input.ArcadeButtons.StickUp):
          return P1StickUp;
        case(Input.ArcadeButtons.StickDown):
          return P1StickDown;
        default:
          return blank;
      }
    }

    public void Update(GameTime gameTime) {
      for(int i = 0; i < activeToolTips.Count; i++) {
        if(activeToolTips[i].timeLeft == 0) {
          activeToolTips.Remove(activeToolTips[i]);
        } else {
          activeToolTips[i].timeLeft--;
        }
      }
    }

    public void ShowToolTip(Input.ArcadeButtons control, Position position, string message, GraphicsDeviceManager graphics) {
      int offset = 1;
      foreach(ToolTip tip in activeToolTips) {
        if(tip.position == position) {
          offset++;
        }
      }
      activeToolTips.Add(new ToolTip(control, position, message, graphics, offset));
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
      spriteBatch.Begin();
      foreach(ToolTip tip in activeToolTips) {
        spriteBatch.Draw(blank, tip.GetRectangle(), Color.SkyBlue);
        spriteBatch.Draw(GetIcon(tip.control), tip.GetIconRectangle(), Color.White);
        spriteBatch.DrawString(pixelfont,tip.message, tip.GetStringPosition(), Color.Black);
      }
      spriteBatch.End();
    }

  }
}