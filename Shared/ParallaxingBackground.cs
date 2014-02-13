// ParallaxingBackground.cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyMonkey
{
	class ParallaxingBackground
	{
		// The image representing the parallaxing background
		Texture2D texture;
		// An array of positions of the parallaxing background
		Vector2[] positions;
		// The speed which the background is moving
		float speed;
		Vector2 scale = new Vector2 (1, 1);

		public void Initialize (ContentManager content, String texturePath, int y, int screenWidth, int screenHeight, float speed, bool shouldFill = true, bool subtractTextureHeight = false)
		{
			// Load the background texture we will be using
			texture = content.Load<Texture2D> (texturePath);
			if (subtractTextureHeight)
				y -= texture.Height;
			var s = shouldFill ? (float)screenHeight / ((float)texture.Height - y) : 1f;
			scale = new Vector2 (s, s);
			// Set the speed of the background
			this.speed = speed;

			// If we divide the screen with the texture width then we can determine the number of tiles need.
			// We add 1 to it so that we won't have a gap in the tiling
			positions = new Vector2[(int)((screenWidth * s) / texture.Width + 2)];

			// Set the initial positions of the parallaxing background
			for (int i = 0; i < positions.Length; i++) {
				// We need the tiles to be side by side to create a tiling effect
				positions [i] = new Vector2 (i * texture.Width * s, y);
			}
		}

		public void Update ()
		{
			// Update the positions of the background
			for (int i = 0; i < positions.Length; i++) {
				// Update the position of the screen by adding the speed
				positions [i].X += speed;
				// If the speed has the background moving to the left
				if (speed <= 0) {
					// Check the texture is out of view then put that texture at the end of the screen
					if (positions [i].X <= -texture.Width * scale.X) {
						positions [i].X = texture.Width * scale.X * (positions.Length - 1);
					}
				}

                // If the speed has the background moving to the right
                else {
					// Check if the texture is out of view then position it to the start of the screen
					if (positions [i].X >= texture.Width * scale.X * (positions.Length - 1)) {
						positions [i].X = -texture.Width * scale.X;
					}
				}
			}
		}

		public void Draw (SpriteBatch spriteBatch)
		{
			for (int i = 0; i < positions.Length; i++) {
				//spriteBatch.Draw(texture, positions[i], Color.White);

				spriteBatch.Draw (
					texture,
					positions [i],
					null,
					null,
					null,
					0,
					scale,
					Color.White,
					SpriteEffects.None,
					0.0f);
			}
		}
	}
}
