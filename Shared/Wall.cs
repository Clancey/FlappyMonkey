using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyMonkey
{
	public class Wall
	{
		// The position of the Wall relative to the top left corner of thescreen
		public Vector2 Position;
		// The state of the Wall
		public bool Active;
		// Get the width of the Wall
		public int Width {
			get{ return Texture.Width; }
		}
		// Get the height of the Wall
		public int Height { get; set; }
		// The speed at which the Wall moves
		float wallMoveSpeed;
		Texture2D Texture, TopCapTexture, BottomCapTexture;
		Rectangle Gap;
		Rectangle topRect;
		Rectangle BottomRect;
		Rectangle topCapRect;
		Rectangle BottomRectCap;
		int topCapOffset, bottomCapOffset;

		public void Initialize (Texture2D texture, Texture2D topCapTexture, Texture2D bottomCapTexture, Rectangle gap, int height, float screenHeight)
		{
			Position = new Vector2 (gap.X, 0);
			Height = height;

			Gap = gap;

			Texture = texture;
			TopCapTexture = topCapTexture;
			BottomCapTexture = bottomCapTexture;

			topRect = new Rectangle ((int)Position.X, (int)Position.Y, Width, Gap.Top);
			BottomRect = new Rectangle ((int)Position.X, Gap.Bottom, Width, height - Gap.Bottom);
			//We need to center the caps
			if (TopCapTexture != null) {
				topCapOffset = (TopCapTexture.Width - texture.Width) / 2;
				topCapRect = new Rectangle (0, topRect.Height - topCapTexture.Height, topCapTexture.Width, topCapTexture.Height);
			}
			if (BottomCapTexture != null) {
				bottomCapOffset = (BottomCapTexture.Width - texture.Width) / 2;
				BottomRectCap = new Rectangle (0, BottomRect.Y, BottomCapTexture.Width, BottomCapTexture.Height);
			}


			// We initialize the Wall to be active so it will be update in the game
			Active = true;

			// Set how fast the Wall moves
			wallMoveSpeed = GamePhysics.WallSpeed;


		}

		public void Update (GameTime gameTime)
		{
			// The Wall always moves to the left so decrement it's xposition
			Position.X -= wallMoveSpeed;
			topRect.X = BottomRect.X = Gap.X = (int)Position.X;

			BottomRectCap.X = BottomRect.X - bottomCapOffset;
			topCapRect.X = topRect.X - topCapOffset;


		}

		public void Draw (SpriteBatch spriteBatch)
		{
			if (Active) {
				// Draw the top part of the wall
				spriteBatch.Draw (
					Texture,
					topRect,
					Color.White);
				if (TopCapTexture != null) {
					spriteBatch.Draw (
						TopCapTexture,
						topCapRect,
						Color.White);
				}

				//Draw the bottom
				spriteBatch.Draw (
					Texture,
					BottomRect,
					Color.White);
				if (BottomCapTexture != null) {
					spriteBatch.Draw (
						BottomCapTexture,
						BottomRectCap,
						Color.White);
				}
			}
		}

		int points { get; set; }

		bool PointCollected { get; set; }

		public bool Collides (Rectangle rect)
		{
			points = (!PointCollected && Gap.Intersects (rect)) ? 1 : 0;

			return rect.Intersects (topRect) || rect.Intersects (BottomRect);
		}

		public int CollectPoints ()
		{
			if (points > 0)
				PointCollected = true;
			return points;
		}
	}
}
