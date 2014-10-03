using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Xna.Framework;
using Amazon.Device.Notification;
using Android;

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

			// Create our OpenGL view, and display it
			Game1.Activity = this;
			var g = new Game1 ();

			g.NotifyScore = (s) => {
				var builder = new AmazonNotification.Builder (ApplicationContext);
				builder.SetSmallIcon (Resource.Drawable.IcPopupReminder);
				builder.SetContentTitle ("New High Score!!!");
				builder.SetContentText (string.Format("{0}",s));
				builder.SetType (BuilderType.Info);

				var notificationManager = GetSystemService (Context.NotificationService)
					.JavaCast <AmazonNotificationManager> ();

				notificationManager.Notify (1, builder.Build ());
			};

			SetContentView (g.Window);
			g.Run ();
		}
	}
}


