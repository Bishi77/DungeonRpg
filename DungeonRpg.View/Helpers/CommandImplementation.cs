using System;
using System.Windows.Input;

namespace DungeonRpg.Helpers
{
	public class CommandImplementation : ICommand
	{
		/// <summary>
		/// A parancs végrehajtásakor elvégzendő művelet(eket) tartalmazza. Nem lehet null az értéke, annak nincs értelme.
		/// </summary>
		private readonly Action<object> executeAction;
		/// <summary>
		/// A parancs végrehajthatóságának a lekérdezését tartalmazza.
		/// </summary>
		private readonly Func<object, bool> canExecuteAction;

		/// <summary>
		/// A WPF környezetre rábízzuk, hogy mikor dob ilyen eseményt
		/// 
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			///Amikor valaki feliratkozik az eseményünkre, akkor ez meghívódik,
			///és a value paraméterben megkapjuk a feliratkozót.
			///Ezt feliratjuk a központi eseményre.
			add { CommandManager.RequerySuggested += value; }
			///Hasonlón, ha leiratkozik valaki a miénkről, leiratkoztatjuk a sajátunkról. 
			remove { CommandManager.RequerySuggested += value; }
		}

		/// <summary>
		/// Dependency Injection az elvégzendő műveletek paraméterezhetőségének
		/// A beküldött feladatok delegate-en keresztül érkeznek, ez egy startégia minta implementáció 
		/// </summary>
		/// <param name="executeAction"></param>
		public CommandImplementation(Action<object> executeAction, Func<object, bool> canExecuteAction)
		{
			this.executeAction = executeAction ?? 
				throw new ArgumentNullException(nameof(executeAction));
			this.canExecuteAction = canExecuteAction ?? 
				throw new ArgumentNullException(nameof(canExecuteAction));
		}

		/// <summary>
		/// Alternatív megoldás, ha csak a parancs meghívásakor végrehajtandó függvényt hívjuk meg
		/// MINDIG végrehajtható a parancs
		/// </summary>
		/// <param name="executeAction"></param>
		public CommandImplementation(Action<object> executeAction) : this(executeAction, parameter => true)
		{ }

		/// <summary>
		/// Megmondja, hogy végrehajtható-e a parancs. True ha igen, False, ha nem.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object parameter)
		{
			return canExecuteAction.Invoke(parameter);
		}

		/// <summary>
		/// A parancs által végzendő művelet
		/// </summary>
		/// <param name="parameter"></param>
		public void Execute(object parameter)
		{
			executeAction.Invoke(parameter);
		}
	}
}