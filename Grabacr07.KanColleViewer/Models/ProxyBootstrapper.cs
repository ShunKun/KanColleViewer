﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;

namespace Grabacr07.KanColleViewer.Models
{
	public enum ProxyBootstrapResult
	{
		None,

		Success,

		/// <summary>
		/// 10048
		/// </summary>
		WsaEAddrInUse,

		UnexpectedException,
	}

	public class ProxyBootstrapper
	{
		public int ListeningPort { get; private set; }

		public ProxyBootstrapResult Result { get; private set; }

		public Exception Exception { get; private set; }

		public ProxyBootstrapper()
		{
			if (Settings.Current.LocalProxyPort < 1 || 65535 < Settings.Current.LocalProxyPort)
			{
				Settings.Current.LocalProxyPort = Properties.Settings.Default.DefaultLocalProxyPort;
			}

			KanColleClient.Current.Proxy.UpstreamProxySettings = Settings.Current.ProxySettings;

			this.Result = ProxyBootstrapResult.None;
		}

		public void Try()
		{
			this.ListeningPort = Settings.Current.IsEnableChangeLocalProxyPort
				? Settings.Current.LocalProxyPort
				: Properties.Settings.Default.DefaultLocalProxyPort;

			try
			{
				KanColleClient.Current.Proxy.Startup(this.ListeningPort);

				this.Result = ProxyBootstrapResult.Success;
			}
			catch (SocketException ex)
			{
				// 参照: Windows ソケットのエラー コード、値、および意味 https://support.microsoft.com/en-us/kb/819124/ja
				if (ex.ErrorCode != 10048) throw;

				this.Result = ProxyBootstrapResult.WsaEAddrInUse;
				this.Exception = ex;
			}
			catch (Exception ex)
			{
				this.Result = ProxyBootstrapResult.UnexpectedException;
				this.Exception = ex;
			}
		}
	}
}