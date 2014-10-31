using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for ColorStatusBar.xaml
	/// </summary>
	public partial class ColorStatusBar : UserControl {
		private ObservableCollection<StatusCommand> command_history = new ObservableCollection<StatusCommand>();

		private int command_history_max = 20;

		private Status current_status;
		public Status CurrentStatus {
			get {
				return current_status;
			}
		}

		private class StatusCommand {
			public string Text { get; set; }
			public Status Status { get; set; }
			public string Progress { get; set; }
			public DateTime Time { get; set; }
			public Brush Color { get; set; }
		}

		CancellationTokenSource task_ready_cancel_token;

		public enum Status {
			Idle,
			Working,
			Completed,
			Error
		}

		public ColorStatusBar() {
			InitializeComponent();

			_ItmHistory.ItemsSource = command_history;
		}

		

		public void SetStatus(string text, Status status) {
			SetStatus(text, status, -1);
		}

		


		public void SetStatus(string text, Status status, int progress) {
			var status_command = new StatusCommand() {
				Text = text,
				Status = status,
				Progress = (progress == -1)? "100%" : Math.Round((progress / 100d), 0).ToString() + "%",
				Time = DateTime.Now
			};

			_PrbProgress.Visibility = (progress == -1) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
			current_status = status;
			
			string hex_color = null;
			switch (status) {
				case Status.Completed:
				case Status.Idle:
					hex_color = "#FF2893EC";
					break;
				case Status.Working:
					hex_color = "#FF19C521";
					break;
				case Status.Error:
					hex_color = "#FFEC2828";
					break;
			}



			status_command.Color = _BdrStatusColor.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex_color));
			command_history.Insert(0, status_command);

			_TxbStatus.Text = text;

			if (status == Status.Completed) {
				if (task_ready_cancel_token != null) {
					task_ready_cancel_token.Cancel();
					task_ready_cancel_token = null;
				}

				task_ready_cancel_token = new CancellationTokenSource();

				Task.Run(async () => {
					await Task.Delay(10000, task_ready_cancel_token.Token);

					if (task_ready_cancel_token.Token.IsCancellationRequested == false) {
						this.Dispatcher.Invoke(() => {
							SetStatus("Ready", Status.Idle);
							_PrbProgress.Value = 0;
							_PrbProgress.Visibility = System.Windows.Visibility.Collapsed;
						});
					}
				});
			}
		}

		private void _BdrStatusColor_MouseDown(object sender, MouseButtonEventArgs e) {
			_PopHistory.IsOpen = true;
		}

		private void CopyLog_Click(object sender, RoutedEventArgs e) {
			StringBuilder sb = new StringBuilder();
			var history = command_history.Reverse().ToArray();

			int max_length = 15;
			foreach (var command in history) {
				max_length = Math.Max(command.Text.Length, max_length);
			}

			max_length += 2;
			string format = "{0,-10} {1,-" + max_length.ToString() + "} {2,-24} {3}\r\n";

			sb.AppendFormat(format, new object[] { "Status", "Event", "Time", "Progress" });

			foreach (var command in history) {
				sb.AppendFormat(format, new object[] { command.Status, command.Text, command.Time, command.Progress });
			}
			sb.Append("End Log ").AppendLine(DateTime.Now.ToString());

			Clipboard.SetText(sb.ToString());
		}

		private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Escape) {
				_PopHistory.IsOpen = false;
			}
		}


	}

}
