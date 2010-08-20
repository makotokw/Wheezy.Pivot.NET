using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
