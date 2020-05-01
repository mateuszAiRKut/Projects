using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WpfAlarmClock
{
    public class ClassTrySystem
    {
        public ClassTrySystem(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            setContextMenu();
            setNotifyIcon();
            addContextMenuToNotifyIcon();
        }

        public void ShowTrayInformation(string Title, string Content)
        {
            notifyIcon.BalloonTipTitle = Title;
            notifyIcon.BalloonTipText = Content;
            notifyIcon.BalloonTipIcon = ToolTipIcon.None;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(30000);
            notifyIcon.BalloonTipClicked += (s, e) => openAction(s, e);
        }

        private void setNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Text = "Alarm Clock";
            notifyIcon.Visible = true;
            addActionDoubleClickToNotifyIcon(openAction);
        }

        private void setContextMenu()
        {
            contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(0, new MenuItem("Open", openAction));
            contextMenu.MenuItems.Add(1, new MenuItem("Close", (o, e) => Environment.Exit(0)));
        }

        private void addContextMenuToNotifyIcon()
        {
            if(contextMenu != null)
                notifyIcon.ContextMenu = contextMenu;
        }

        private void addActionDoubleClickToNotifyIcon(EventHandler action)
        {
            notifyIcon.DoubleClick += action;
        }

        private void openAction(object sender, EventArgs e)
        {
            mainWindow.Show();
            mainWindow.Activate();
            if (mainWindow.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;
        }

        private MainWindow mainWindow;
        private ContextMenu contextMenu;
        private NotifyIcon notifyIcon;
    }
}
