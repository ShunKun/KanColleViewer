using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;

namespace Counter
{
	[Export(typeof(IPlugin))]
	[Export(typeof(ITool))]
	[Export(typeof(IRequestNotify))]
	[ExportMetadata("Guid", "65BE3E80-8EC1-41BD-85E0-78AEFD45A757")]
	[ExportMetadata("Title", "KanColleCounter")]
	[ExportMetadata("Description", "シンプルな回数カウント機能を提供します。")]
	[ExportMetadata("Version", "1.1")]
	[ExportMetadata("Author", "@Grabacr07")]
	public class KanColleCounter : IPlugin, ITool, IRequestNotify
	{
		private CounterViewModel viewModel;

		string ITool.Name => "Counter";

		object ITool.View => new CounterView { DataContext = this.viewModel, };

		public event EventHandler<NotifyEventArgs> NotifyRequested;

		public void Initialize()
		{
			this.viewModel = new CounterViewModel
			{
				Counters = new ObservableCollection<CounterBase>
				{
					new ItemDestroyCounter(KanColleClient.Current.Proxy),
                    new NyukyoCounter(KanColleClient.Current.Proxy),
                    new SupplyCounter(KanColleClient.Current.Proxy),
                    new KaisouCounter(KanColleClient.Current.Proxy),
                    new MissionCounter(KanColleClient.Current.Proxy),
                    new BattleWinCounter(KanColleClient.Current.Proxy),
                    new EtcCounter(),
				}
			};
		}
	}
}
