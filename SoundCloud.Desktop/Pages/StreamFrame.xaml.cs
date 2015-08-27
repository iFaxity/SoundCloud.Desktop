﻿using Streamer.Net.SoundCloud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SoundCloud.Desktop {
    /// <summary>
    /// Interaction logic for Stream.xaml
    /// </summary>
    public partial class StreamFrame : Page {
        public StreamFrame() {
            InitializeComponent();

            scroll.ScrollChanged += (sender, e) => {
                var s = sender as ScrollViewer;
                if(s.VerticalOffset >= (s.ScrollableHeight - 100) && !busy) GetNext();
            };

            Refresh();
        }

        static List<int> tracks = new List<int>();
        static bool busy = true;

        void AddCovers(List<int> list) {
            for(int i = 0; i < list.Count; i++) {
                tracks.Add(list[i]);
                Dispatcher.Invoke(() => {
                    var cover = new CoverItem(list[i]);
                    cover.Margin = new Thickness(10, 0, 10, 10);
                    cover.Cursor = Cursors.Hand;
                    cover.MouseDown += (sender, e) => { if(Player.Tracks != tracks) Player.Tracks = tracks; Player.Play(panel.Children.IndexOf((CoverItem)sender)); };

                    panel.Children.Add(cover);
                });
            }
            busy = false;
        }

        public void GetNext() {
            Task.Factory.StartNew(() => {
                busy = true;
                UICore.ToggleSpinner(grid);

                AddCovers(Me.GetNextDashboard(DashboardType.All));

                UICore.ToggleSpinner(grid);
                busy = false;
            });
        }
        public void Refresh() {
            // Clear items
            tracks.Clear();
            panel.Children.Clear();

            GetNext();
        }
    }
}
