using MySql.Data.MySqlClient;
using System;
using System.Timers;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfMySqlBackuper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		System.Timers.Timer timer1 = new System.Timers.Timer();  //3 days
		int timerCounter = 0;

		public MainWindow()
		{
			InitializeComponent();

			timer1.Interval = 1000;        //259200000
			timer1.AutoReset = true;
			timer1.Elapsed += OnTimedEvent;

			
		}

		

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed) 
			{
				DragMove();
			}
		}

		private void btnMinimize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void btn_goBackup_Click(object sender, RoutedEventArgs e)
		{
			if ( !string.IsNullOrEmpty(serverTextBox.Text.ToString()) && !string.IsNullOrEmpty(userTextBox.Text.ToString()) && !string.IsNullOrEmpty(pwdTextBox.Text.ToString()) && !string.IsNullOrEmpty(dbTextBox.Text.ToString()))
			{
				string file = "backups\\backup1.sql";
				string constring = $"server={serverTextBox.Text};user={userTextBox.Text};pwd={pwdTextBox.Text};Persist Security Info=True;database={dbTextBox.Text};Convert Zero Datetime=True;";
				if (string.IsNullOrEmpty(pathTextBox.Text.ToString()) || pathTextBox.Text == file)
				{
					//file = "backups\\backup1.sql";

					for (int i = 1; File.Exists(file); i++)
					{
						/*все пропало, файла нет, надо срочно что-то делать*/
						file = $"backups\\backup{i}.sql";
					}
				}
				else
				{
					file = pathTextBox.Text.ToString();
				}


				try
				{

				
					using (MySqlConnection conn = new MySqlConnection(constring))
					{
						using (MySqlCommand cmd = new MySqlCommand())
						{
							using (MySqlBackup mb = new MySqlBackup(cmd))
							{
								try
								{

									cmd.Connection = conn;
									try
									{
										conn.Open();
									}
									catch (Exception exce)
									{
										consolelog_TextBlock.Text = exce.Message.ToString();
										//throw;
									}
									

									mb.ExportToFile(file);
									conn.Close();

									consolelog_TextBlock.Text = "file created sucsessful !";
								}
								catch (Exception ex)
								{
									consolelog_TextBlock.Text = ex.Message.ToString();
									
								}
							}
						}
					}
				}
				catch (Exception exc)
				{
					consolelog_TextBlock.Text = exc.Message.ToString();
					
				}
			}
		}


		UserBackupClass myUserBp = new UserBackupClass();
		private void button_save_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(serverTextBox.Text.ToString()) && !string.IsNullOrEmpty(userTextBox.Text.ToString()) && !string.IsNullOrEmpty(pwdTextBox.Text.ToString()) && !string.IsNullOrEmpty(dbTextBox.Text.ToString()))
			{
				myUserBp.serializeBackupData(serverTextBox.Text, userTextBox.Text, pwdTextBox.Text, dbTextBox.Text);
			}
			
		}

		private void button_load_Click(object sender, RoutedEventArgs e)
		{
			var deserialize = myUserBp.deserializeBackupData();
			serverTextBox.Text = deserialize.server;
			userTextBox.Text = deserialize.user;
			pwdTextBox.Text = deserialize.password;
			dbTextBox.Text = deserialize.database;
		}

		private void timerTick(object sender, EventArgs e)
		{
			int x = 0;
			//x++;
			consolelog_TextBlock.Text = $"{x+1}";
		}


		
		private void button_autoBackup_Click(object sender, RoutedEventArgs e)
		{
			//button_autoBackup.Effect. = DropShadowEffect.BlurRadiusProperty ;
			//System.Windows.Media.Effects.DropShadowEffect


			if (timer1.Enabled == false)
			{
				//timer1.Enabled = true;
				timer1.Start();
				timer1.Enabled = true;
				//timer1.
				
				//timer1 += new EventHandler(timerTick);
				//consolelog_TextBlock.Text = timer1.Interval.ToString();
				DropShadowEffect shadowEffect = new DropShadowEffect();
				shadowEffect.BlurRadius = 30;
				shadowEffect.Opacity = 1;
				shadowEffect.Color = Colors.Aqua;
				shadowEffect.ShadowDepth = 0;
				button_autoBackup.Effect = shadowEffect;
				
				//btn_goBackup.Pres;
				return;
			}
			if (timer1.Enabled == true)
			{
				//timer1.Enabled = false;
				timer1.Stop();
				DropShadowEffect shadowEffect = new DropShadowEffect();
				shadowEffect.BlurRadius = 0;
				shadowEffect.Opacity = 0;
				shadowEffect.Color = Colors.Aqua;
				shadowEffect.ShadowDepth = 0;
				button_autoBackup.Effect = shadowEffect;
				return;
			}
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			//consolelog_TextBlock.Text = (++timerCounter).ToString();
			this.Dispatcher.Invoke(() => { consolelog_TextBlock.Text = (++timerCounter).ToString(); });

			if (timerCounter == 259200000)
			{
				timerCounter = 0;
				this.btn_goBackup.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
			}
		}

		private void buttonToConsole_Click(object sender, RoutedEventArgs e)
		{
			//Frame frame1 = new Frame();
			//frame1.NavigationService.Navigate(new Page1());

			// NavigationService.Navigate(new Page1());
			if (consolelog.Visibility == 0)
			{
				consolelog.Visibility = (Visibility)1;
				//consolelog.IsEnabled = true;
				return;
			}
			else
			{
				consolelog.Visibility = 0;
				//consolelog.IsEnabled = false;
				return;
			}
			
		}
	}
}
