using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Xna.Framework;
#if __FIRE__
using Amazon.Device.GameController;
#endif

namespace FlappyMonkey
{
	[Activity (Label = "FlappyMonkey.Droid", 
		MainLauncher = true,
		Icon = "@drawable/icon",
		Theme = "@style/Theme.Splash",
		AlwaysRetainTaskState = true,
		LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
		ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation |
		Android.Content.PM.ConfigChanges.KeyboardHidden |
		Android.Content.PM.ConfigChanges.Keyboard)]
	public class Activity1 : AndroidGameActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			#if __FIRE__
			GameController.Init (this);
			#endif

			TopScore.OnNewTopScore += HandleOnNewTopScore;

			// Create our OpenGL view, and display it
			Game1.Activity = this;
			var g = new Game1 ();
			SetContentView (g.Window);
			g.Run ();
		}

		#if __FIRE__
		public override bool OnGenericMotionEvent (MotionEvent e)
		{
			Console.WriteLine ("OnGenericMotionEvent");
			var handled = false;
			try {
				handled = GameController.OnGenericMotionEvent(e);
			} catch (GameController.DeviceNotFoundException ex) {
			}

			return handled || base.OnGenericMotionEvent(e);
		}

		int notificationId = 0;
		void HandleOnNewTopScore (int oldScore, int newScore)
		{
			#if __FIRE__
			var b = new Amazon.Device.Notification.AmazonNotification.Builder(this);
			b.SetSmallIcon(FlappyMonkey.Ouya.Resource.Drawable.trophy);
			b.SetContentTitle("New Top Score! " + newScore);
			b.SetContentText("You've just beat the old score of " + oldScore);
			b.SetType(Amazon.Device.Notification.BuilderType.Info);

			var m = Amazon.Device.Notification.AmazonNotificationManager.FromContext(this);
			m.Notify(notificationId++, b.Build());
			#endif
		}

		public override bool OnKeyUp (Keycode keyCode, KeyEvent e)
		{
			Console.WriteLine ("OnKeyUp: " + keyCode);
			var handled = false;
			try {
				handled = GameController.OnKeyUp((int)keyCode, e);
			}
			catch (GameController.DeviceNotFoundException ex) {
			}
			return handled || base.OnKeyUp(keyCode, e);
		}

		public override bool OnKeyDown (Keycode keyCode, KeyEvent e)
		{
			Console.WriteLine ("OnKeyDown: " + keyCode);
			var handled = false;
			try {
				handled = GameController.OnKeyDown((int)keyCode, e);
			}
			catch (GameController.DeviceNotFoundException ex) {
			}
			return handled || base.OnKeyDown(keyCode, e);
		}
		#endif
	}
}


