
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RompecabezasCCA
{
	[Activity (Label = "JuegoDificil")]			
	public class JuegoDificil : Microsoft.Xna.Framework.AndroidGameActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var g = new Rompecabezas(5);
			SetContentView((View) g.Services.GetService(typeof(View)));
			g.Run();
		}

		public void Start()
		{
			var intent = new Intent (Android.App.Application.Context, typeof(JuegoDificil));
			intent.SetFlags (ActivityFlags.NewTask);
			Android.App.Application.Context.StartActivity (intent);
		}
	}
}

