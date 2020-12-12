using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using ViewJumperWPF.Model;

namespace ViewJumperWPF
{
    /// <summary>
    /// ViewJumperView.xaml 的交互逻辑
    /// </summary>
    public partial class ViewJumperView : Window
    {
        private Document _doc;
        private UIDocument _uidoc;

        public RevitViewWrapDatas Datas { get; set; }

        private ICollectionView Source { get; set; }

        public ViewJumperView(UIDocument uidoc)
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            this._uidoc = uidoc;
            this._doc = this._uidoc.Document;

            Datas = new RevitViewWrapDatas(this._doc);

            this.Source = CollectionViewSource.GetDefaultView(this.Datas.WrapViewDatas);
            this.grdMain.DataContext = this.Datas;
            this.lvItems.DataContext = this.Source;
        }

        /// <summary>
        /// 载入——进行分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Source.GroupDescriptions.Clear();

            PropertyInfo pinfo = typeof(RevitViewWrap).GetProperty("ViewTypeName");
            if (pinfo != null)
            {
                this.Source.GroupDescriptions.Add(new PropertyGroupDescription(pinfo.Name));
            }
        }

        /// <summary>
        /// 单击列-排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader currentHeader = e.OriginalSource as GridViewColumnHeader;
            if (currentHeader != null && currentHeader.Role != GridViewColumnHeaderRole.Padding)
            {
                using (this.Source.DeferRefresh())
                {
                    Func<SortDescription, bool> lamda = item => item.PropertyName.Equals(currentHeader.Column.Header.ToString());
                    if (this.Source.SortDescriptions.Count(lamda) > 0)
                    {
                        SortDescription currentSortDescription = this.Source.SortDescriptions.First(lamda);
                        ListSortDirection sortDescription = currentSortDescription.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                        currentHeader.Column.HeaderTemplate = currentSortDescription.Direction == ListSortDirection.Ascending ?
                            this.Resources["HeaderTemplateArrowDown"] as DataTemplate : this.Resources["HeaderTemplateArrowUp"] as DataTemplate;

                        this.Source.SortDescriptions.Remove(currentSortDescription);
                        this.Source.SortDescriptions.Insert(0, new SortDescription(currentHeader.Column.Header.ToString(), sortDescription));
                    }
                    else
                        this.Source.SortDescriptions.Add(new SortDescription(currentHeader.Column.Header.ToString(), ListSortDirection.Ascending));
                }
            }
        }

        /// <summary>
        /// 双击元素-视图跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = this.lvItems.SelectedItem as RevitViewWrap;

            var view = item?.View;
            if (view != null)
            {
                _uidoc.ActiveView = view;

                this.Close();
            }
        }

        private void JumpView()
        {
            var item = this.lvItems.SelectedItem as RevitViewWrap;

            var view = item?.View;
            if (view != null)
            {
                _uidoc.ActiveView = view;

                this.Close();
            }
        }

        private void ListViewItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && this.lvItems.SelectedItem != null)
            {
                JumpView();
            }
        }
    }
}