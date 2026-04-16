using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DrinkWaterAnnoying
{
    public partial class Form1 : Form
    {
        private Timer reminderTimer;
        private Timer WaitingTimer;
        private NotifyIcon notifyIcon;
        private string[] drinkWaterSentences;
        private bool completed = false;
        private int sentenceIndex;

        [DllImport("powrprof.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public Form1()
        {
            InitializeComponent();
            
            // Initialize the timer
            reminderTimer = new Timer();
            reminderTimer.Interval = 10000;
            reminderTimer.Tick += ReminderTimer_Tick;

            WaitingTimer = new Timer();
            WaitingTimer.Interval = 1800000;
            WaitingTimer.Tick += WaitingTimer_Tick;

            // Initialize the NotifyIcon
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = System.Drawing.SystemIcons.Information;
            notifyIcon.Visible = true;
            
            drinkWaterSentences = new string[]
            {
                "Hello stay hydrated by drinking water.",
                "Did you drink water?",
                "Hay Are you listening to me or not?",
                "I will make this pc goto sleep if you don't listen to me",
                "Drink water Now! this is the last chance :(",
                "Okay this is enough :("
            };
            
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Please set the focus assist of notifications to off to know the notifications all the time.");
            this.Hide();
            sentenceIndex = 0;
            reminderTimer.Start();
            WaitingTimer.Start();
        }

        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            // Show a notification with the next drink water sentence
            if (sentenceIndex < drinkWaterSentences.Length)
            {
                if (completed == true)
                {
                    completed = false;
                    ShowNotification("Okay", "Ok GoodWork :)");
                    reminderTimer.Stop();
                    textBox1.Text = textBox1.Text + Environment.NewLine + "Ok GoodWork :)";
                    return;
                }
                ShowNotification("Drink Water!Now", drinkWaterSentences[sentenceIndex]);
                if (sentenceIndex == 5 && completed == false)
                {
                    SetSuspendState(false, false, false);
                }
                sentenceIndex++;
            }
            else
            {
                reminderTimer.Stop();
            }
        }

        private void WaitingTimer_Tick(object sender, EventArgs e)
        {
            sentenceIndex = 0;
            completed = false;
            reminderTimer.Start();
        }

        private void ShowNotification(string title, string message)
        {
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(5000); // Display the notification for 5 seconds
            textBox1.Text = textBox1.Text + Environment.NewLine + drinkWaterSentences[sentenceIndex];
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon.Dispose();
        }

        private void yesDidItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            completed = true;
        }
    }
}
