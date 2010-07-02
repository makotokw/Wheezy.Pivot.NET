using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace iTunesPivotCollectionViewer
{
    public partial class MainPage : UserControl
    {
        public MainPage(IDictionary<string, string> initParams)
        {
            InitializeComponent();
            try
            {
                var collectionUrl = initParams["collectionUrl"];
                if (!Uri.IsWellFormedUriString(collectionUrl, UriKind.Absolute))
                {
                    collectionUrl = new Uri(Application.Current.Host.Source, collectionUrl).ToString();
                }
                Viewer.LoadCollection(collectionUrl, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
