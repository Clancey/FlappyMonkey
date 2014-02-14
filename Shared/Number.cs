using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace FlappyMonkey
{
	public static class Number
	{
		public enum Alignment
		{
			Left,
			Center,
			Right
		}

		public static int Height {
			get{ return textures ["0"].Height;}
		}

		static Dictionary<string,Texture2D> textures;

		public static void Initialize (ContentManager content)
		{
			if (textures != null)
				return;
			textures = new Dictionary<string, Texture2D> ();
			Enumerable.Range (0, 10).Select (x => x.ToString ()).ToList ().ForEach (x => textures.Add (x, content.Load<Texture2D> (x)));

		}
		const int padding = 5;
		public static void Draw (SpriteBatch spriteBatch,int number, Alignment aligment ,Rectangle rect, int scale = 1)
		{
			var numbers = number.ToString ().ToCharArray ().Select (n => n.ToString ());
			var images = numbers.Select (n => textures [n]).ToList ();
			var width = images.Sum (n => n.Width* scale) + ((images.Count - 1) * padding*scale);

			int x = rect.Left;
			if (aligment == Alignment.Right)
				x = rect.Right - width;
			else if (aligment == Alignment.Center)
				x = (rect.Right / 2) - (width / 2);
			int y = rect.Top;
			var drawRect = new Rectangle (x, y, 0, 0);

			images.ForEach (i => {
				drawRect.X =x;
				drawRect.Width = i.Width*scale;
				drawRect.Height = i.Height*scale;
				spriteBatch.Draw (
					i,
					null,
					drawRect,
					null,
					null,
					0,
					new Vector2(scale),
					Color.White,
					SpriteEffects.None,
					0.0f);


				spriteBatch.Draw(i,drawRect,Color.White);
				x += i.Width * scale + padding*scale;
			});
		}
	}
}

