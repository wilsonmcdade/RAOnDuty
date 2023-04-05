using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Devcade;

namespace RAOnDuty
{
	public class Player {
		public enum Movements {
			WalkRight,
			WalkLeft,
			Jump,
			Forward,
			Backward
		}
		public Vector3 playerPosition;
		private Texture2D man;
		private Texture2D manwalk1;
		private Texture2D manwalk2;
		private TileMap tileMap;
		private AnimationManager animationManager;
		public SpriteEffects spriteEffect;
		public Player(TileMap _tileMap) {
			tileMap = _tileMap;
			playerPosition = new Vector3(0,-10,0);
			animationManager = new AnimationManager();
			spriteEffect = SpriteEffects.None;
		}
		private Texture2D getCurrentTexture() {
			return animationManager.GetCurrentFrame("Walk");
		}
		public void Move(Movements Direction) {
			switch (Direction) {
				case Movements.WalkRight:
					playerPosition.X -= tileMap.SCALE/20;
					animationManager.PlayAnimation("Walk");
					spriteEffect = SpriteEffects.None;
					break;
				case Movements.WalkLeft:
					playerPosition.X += tileMap.SCALE/20;
					animationManager.PlayAnimation("Walk");
					spriteEffect = SpriteEffects.FlipHorizontally;
					break;
				case Movements.Jump:
					playerPosition.X += tileMap.SCALE/20;
					animationManager.PlayAnimation("Walk");
					break;
				case Movements.Forward:
					if (playerPosition.Z == 0) {
						playerPosition.Z--;
					}
					break;
				case Movements.Backward:
					if (playerPosition.Z != 0) {
						playerPosition.Z++;
					}
					break;
				default:
					break;
			}
		}
		public Rectangle getCurrentRectangle(GraphicsDeviceManager graphics) {
			Vector2 size = new Vector2(7*getCurrentTexture().Width,7*getCurrentTexture().Height);
			return new Rectangle((graphics.PreferredBackBufferWidth-(int)size.X)/2,(graphics.PreferredBackBufferHeight-(int)size.X)/2,(int) size.X,(int) size.Y);
		}
		public void Update(GameTime gameTime) {

			if (gameTime.TotalGameTime.Milliseconds % 100 == 0) {
				if (Keyboard.GetState().IsKeyDown(Keys.W)) {
					Move(Player.Movements.Forward);
				}
				if (Keyboard.GetState().IsKeyDown(Keys.S)) {
					Move(Player.Movements.Backward);
				}
				if (Keyboard.GetState().IsKeyDown(Keys.D)) {
					Move(Player.Movements.WalkRight);
				}
				if (Keyboard.GetState().IsKeyDown(Keys.A)) {
					Move(Player.Movements.WalkLeft);
				}
			}
			animationManager.Update(gameTime);
		}
		public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
			spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.None,null,null);
			spriteBatch.Draw(getCurrentTexture(), getCurrentRectangle(graphics), null, Color.White, 0f, new Vector2(0,0),spriteEffect, 0);
			spriteBatch.End();
		}
		public void LoadContent(ContentManager Content) {
			man = Content.Load<Texture2D>("man");
			manwalk1 = Content.Load<Texture2D>("man_walk1");
			manwalk2 = Content.Load<Texture2D>("man_walk2");

			animationManager.AddAnimation("Walk",200,new List<Texture2D>(){man, manwalk1, manwalk2});
		}
	}

	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private SpriteFont pixelfont;
		private Texture2D blank;
		private ToolTips toolTips;
		private TileMap tileMap;
		private Player player1;
		private EventManager eventManager;
		private NotificationManager notification;

		/// <summary>
		/// Game constructor
		/// </summary>
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
		}

		/// <summary>
		/// Does any setup prior to the first frame that doesn't need loaded content.
		/// </summary>
		protected override void Initialize()
		{
			Input.Initialize(); // Sets up the input library

			// Set window size if running debug (in release it will be fullscreen)
			#region
#if DEBUG
			_graphics.PreferredBackBufferWidth = 420;
			_graphics.PreferredBackBufferHeight = 980;
			_graphics.ApplyChanges();
			
#else
			_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			_graphics.ApplyChanges();
#endif
			#endregion

			tileMap = new TileMap();
			toolTips = new ToolTips();
			player1 = new Player(tileMap);
			eventManager = new EventManager();
			notification = new NotificationManager();

			// TODO: Add your initialization logic here

			tileMap.Initialize();

			base.Initialize();
		}

		/// <summary>
		/// Does any setup prior to the first frame that needs loaded content.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			blank = Content.Load<Texture2D>("blank");
            pixelfont = Content.Load<SpriteFont>("pixelfont");
			player1.LoadContent(this.Content);
			tileMap.LoadContent(this.Content);
			toolTips.LoadContent(this.Content);
			notification.LoadContent(this.Content);

			// TODO: use this.Content to load your game content here
			// ex.
			// texture = Content.Load<Texture2D>("fileNameWithoutExtention");
		}

		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
			Input.Update(); // Updates the state of the input library

			// Exit when both menu buttons are pressed (or escape for keyboard debuging)
			// You can change this but it is suggested to keep the keybind of both menu
			// buttons at once for gracefull exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Exit();
			}

			if (gameTime.TotalGameTime.Milliseconds % 500 == 0) {
				if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
					toolTips.ShowToolTip(Input.ArcadeButtons.StickRight, ToolTips.Position.Middle, "Use Stick to Move Around", _graphics);
					toolTips.ShowToolTip(Input.ArcadeButtons.A1, ToolTips.Position.Middle, "Use A1 to Do Drugs", _graphics);
				}
				if (Keyboard.GetState().IsKeyDown(Keys.W)) {
					notification.ShowNotification("Your resident is drunk!\n You should kill them!");
				}

			}

			player1.Update(gameTime);
			toolTips.Update(gameTime);
			notification.Update(gameTime);
			tileMap.Update(gameTime, player1);

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			tileMap.Draw(_spriteBatch, _graphics, player1);

			_spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.None,null,null);
			//_spriteBatch.Draw(blank,new Rectangle(0,5*(_graphics.PreferredBackBufferHeight/8)+1, _graphics.PreferredBackBufferWidth,3*_graphics.PreferredBackBufferHeight/8), Color.SlateGray);
			#if DEBUG
			_spriteBatch.DrawString(pixelfont, player1.playerPosition.ToString(),new Vector2(5,5), Color.Black);
			#endif
			// TODO: Add your drawing code here
			_spriteBatch.End();

			player1.Draw(_spriteBatch, _graphics);
			toolTips.Draw(_spriteBatch, _graphics);
			notification.Draw(_spriteBatch, _graphics);
			

			base.Draw(gameTime);
		}
	}
}