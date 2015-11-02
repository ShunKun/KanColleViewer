using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

namespace Counter
{
	public abstract class CounterBase : NotificationObject
	{
		#region Text 変更通知プロパティ

		private string _Text;

		public string Text
		{
			get { return this._Text; }
			set
			{
				if (this._Text != value)
				{
					this._Text = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Count 変更通知プロパティ

		private int _Count;

		public int Count
		{
			get { return this._Count; }
			set
			{
				if (this._Count != value)
				{
					this._Count = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public void Reset()
		{
			this.Count = 0;
		}

        public void Plus()
        {
            this.Count++;
        }
        public void Minus()
        {
            if (this.Count > 0)
                this.Count--;
        }
	}

	public class ItemDestroyCounter : CounterBase
	{
		public ItemDestroyCounter(KanColleProxy proxy)
		{
			proxy.api_req_kousyou_destroyitem2
				.TryParse()
				.Where(x => x.IsSuccess)
				.Subscribe(_ => this.Count++);

			this.Text = "装備を破棄した回数";
		}
	}

	public class SupplyCounter : CounterBase
	{
		public SupplyCounter(KanColleProxy proxy)
		{
			proxy.api_req_hokyu_charge
				.TryParse()
				.Where(x => x.IsSuccess)
				.Subscribe(_ => this.Count++);

			this.Text = "艦娘に補給した回数";
		}
	}


	public class MissionCounter : CounterBase
	{
		public MissionCounter(KanColleProxy proxy)
		{
			proxy.api_req_mission_result
				.TryParse<kcsapi_mission_result>()
				.Where(x => x.IsSuccess)
				.Where(x => x.Data.api_clear_result == 1 || x.Data.api_clear_result == 2)
				.Subscribe(_ => this.Count++);

			this.Text = "遠征に成功した回数";
		}
	}

    public class KaisouCounter : CounterBase
    {
        public KaisouCounter(KanColleProxy proxy)
        {
            proxy.api_req_kaisou_powerup
                .TryParse()
                .Where(x => x.IsSuccess)
                .Subscribe(_ => this.Count++);

            this.Text = "艦娘を改装した回数";
        }
    }

    public class NyukyoCounter : CounterBase
    {
        public NyukyoCounter(KanColleProxy proxy)
        {
            proxy.api_req_nyukyo_start
                .TryParse()
                .Where(x => x.IsSuccess)
                .Subscribe(_ => this.Count++);

            this.Text = "艦娘を入渠した回数";
        }
    }

    public class BattleWinCounter : CounterBase
    {
        public BattleWinCounter(KanColleProxy proxy)
        {
            proxy.api_req_sortie_battleresult
                .TryParse<kcsapi_battleresult>()
                .Where(x => x.IsSuccess)
                .Where(x => x.Data.api_win_rank == "S" || x.Data.api_win_rank == "A" || x.Data.api_win_rank == "B")
                .Subscribe(_ => this.Count++);

            proxy.api_req_combined_battle_battleresult
                .TryParse<kcsapi_combined_battle_battleresult>()
                .Where(x => x.IsSuccess)
                .Where(x => x.Data.api_win_rank == "S" || x.Data.api_win_rank == "A" || x.Data.api_win_rank == "B")
                .Subscribe(_ => this.Count++);

            this.Text = "出撃で勝利した回数";
        }
    }

    public class EtcCounter : CounterBase
    {
        public EtcCounter()
        {
            this.Text = "任意カウンター";
        }
    }

}
