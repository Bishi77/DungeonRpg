using System;
using System.Threading.Tasks;
using System.Windows;

namespace DungeonRpg.Views
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;			
			Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
			//AppDomain.CurrentDomain.UnhandledException += ...;
			//TaskScheduler.UnobservedTaskException += ...;
		}

		void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
			MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			// OR whatever you want like logging etc. MessageBox it's just example
			// for quick debugging etc.
			e.Handled = true;
		}
	}
}
