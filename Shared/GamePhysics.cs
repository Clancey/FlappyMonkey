using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlappyMonkey
{
	static class GamePhysics
	{
		public const double PlayerJumpLength = 500;
		public const double PlayerJumpHeight = -10;
		public const double PlayerFallSpeed = .6;

		//Milliseconds
		public const double MinimumWallSpawnRate = 2000;
		//Used to gradually make it more dificult
		public const double StartWallSpawnRate = 2000;

		public const float WallSpeed = 2.5f;

		//Pixels
		public const int MinimumGapSize = 214;
		public const int MaximumGapSize = 300;
	}
}
