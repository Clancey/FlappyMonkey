using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyMonkey
{
	class Player
	{
		public Vector2 StartLocation { get; set; }
		// Animation representing the player
		public Texture2D PlayerTexture;
		// Position of the Player relative to the upper left side of the screen
		public Vector2 Position;
		// State of the player
		public bool Active;
		// Amount of hit points that player has
		public int Health;
		// Get the width of the player ship
		public int Width {get;set;}
		// Get the height of the player ship
		public int Height {get;set;}
		// Animation representing the player
		public Texture2D Texture;
		Vector2 DrawOffset;
		Vector2 Origin;
		float Scale = 1f;
		//The height we want the player to always be
		const int DesiredHeight = 72;
		// Initialize the player
		public void Initialize (Texture2D animation, Vector2 position)
		{
			Texture = animation;
			Scale = DesiredHeight / Texture.Height;
			Width = (int)(Texture.Width * Scale);
			Height = (int)(Texture.Height * Scale);
			// Set the starting position of the player around the middle of the screen and to the back
			StartLocation = Position = position;

			// Set the player to be active
			Active = true;

			// Set the player health
			Health = 1;
			Origin = new Vector2 (Texture.Width / 2, Texture.Height / 2);
			DrawOffset = new Vector2 (Width / 2 , Height / 2);
		}

		public Rectangle Rectangle
		{
			//Make it a tad bit skinnier for a better hitbox
			get{ return new Rectangle ((int)Position.X, (int)Position.Y, (int)(Width *.9), Height); }
		}

		double jumpTimer = GamePhysics.PlayerJumpLength;
		double fallTimer = 0;
		bool isJumping = false;
		// Update the player animation
		public void Update (GameTime gameTime, bool shouldJump, float maxHeight, bool autoFly)
		{
			jumpTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

			if (Position.X < -Width || Health <= 0) {
				// active game list
				Active = false;
			}

			if (shouldJump && !autoFly) {
				isJumping = true;
				jumpTimer = 0;
			}

			if (Active && isJumping) {
				var sin = Math.Sin (jumpTimer * .5 * Math.PI / GamePhysics.PlayerJumpLength);
				//Console.WriteLine("sin {0}", sin);
				var height = (int)(GamePhysics.PlayerJumpHeight - GamePhysics.PlayerJumpHeight * sin);
				Position.Y += height;
				fallTimer = 0;
			} else {
				Position.Y += Convert.ToInt32 (GamePhysics.PlayerFallSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);
				fallTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
			}
			rotation = autoFly ? 0 : TurnToFace (rotation, Rotation (), TurnSpeed);
			Position.Y = MathHelper.Clamp (Position.Y, 0, maxHeight - Height);

			if (jumpTimer > GamePhysics.PlayerJumpLength) {
				isJumping = false;
			}
			if (autoFly && Position.Y >= StartLocation.Y) {
				isJumping = true;
				jumpTimer = 0;
			}
		}

		private static float TurnToFace (float currentAngle, float targetRotation, float turnSpeed)
		{

			// so now we know where we WANT to be facing, and where we ARE facing...
			// if we weren't constrained by turnSpeed, this would be easy: we'd just 
			// return desiredAngle.
			// instead, we have to calculate how much we WANT to turn, and then make
			// sure that's not more than turnSpeed.

			// first, figure out how much we want to turn, using WrapAngle to get our
			// result from -Pi to Pi ( -180 degrees to 180 degrees )
			float difference = WrapAngle (targetRotation - currentAngle);

			// clamp that between -turnSpeed and turnSpeed.
			difference = MathHelper.Clamp (difference, -turnSpeed, turnSpeed);

			// so, the closest we can get to our target is currentAngle + difference.
			// return that, using WrapAngle again.
			return WrapAngle (currentAngle + difference);
		}

		/// <summary>
		/// Returns the angle expressed in radians between -Pi and Pi.
		/// <param name="radians">the angle to wrap, in radians.</param>
		/// <returns>the input value expressed in radians from -Pi to Pi.</returns>
		/// </summary>
		private static float WrapAngle (float radians)
		{
			while (radians < -MathHelper.Pi) {
				radians += MathHelper.TwoPi;
			}
			while (radians > MathHelper.Pi) {
				radians -= MathHelper.TwoPi;
			}
			return radians;
		}
		// Draw the player
		public void Draw (SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(Texture, Position, Color.White);
			spriteBatch.Draw (Texture, Position + DrawOffset, null, Color.White, rotation, Origin, Scale, SpriteEffects.None, 0);
		}

		const float TurnSpeed = 0.50f;
		float rotation = 0f;

		public float Rotation ()
		{
			const float topAngle = -30;
			const float bottomAngle = 90;
			if (isJumping)
				return MathHelper.ToRadians (topAngle);

			if (!Active || fallTimer > GamePhysics.PlayerJumpLength)
				return MathHelper.ToRadians (bottomAngle);

			if (fallTimer < GamePhysics.PlayerJumpLength) {
				var sin = (float)Math.Sin (fallTimer * .5 * Math.PI / GamePhysics.PlayerJumpLength);
				return MathHelper.ToRadians (sin * bottomAngle);
			}
			return 0;
		}
	}
}