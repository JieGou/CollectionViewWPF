using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewJumperWPF.Model
{
    /// <summary>
    /// Revit视图数据
    /// </summary>
    public class RevitViewWrapDatas
    {
        private readonly Document _doc;

        private ObservableCollection<RevitViewWrap> _views;

        public RevitViewWrapDatas(Document doc)
        {
            this._doc = doc;
        }

        public ObservableCollection<RevitViewWrap> WrapViewDatas
        {
            get
            {
                var observableCollection = this.GetViewDatas(_doc, ViewType.EngineeringPlan);

                //var newCollection = this.GetViewDatas(_doc, ViewType.FloorPlan);

                //foreach (var e in observableCollection)
                //{
                //    var tmp = newCollection.FirstOrDefault(x => x.Id == e.Id);
                //    if (tmp == null)
                //    {
                //        newCollection.Add(e);
                //    }
                //}

                this._views = this._views ?? observableCollection;
                return this._views;
            }
        }

        /// <summary>
        /// 视图的属性列
        /// </summary>
        public IEnumerable<string> Columns =>
            from propertyInfo in typeof(RevitViewWrap).GetProperties()
            select propertyInfo.Name;

        /// <summary>
        /// 获取视图数据
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<RevitViewWrap> GetViewDatas(Document doc, ViewType type = ViewType.Undefined)
        {
            List<Element> viewElementList = new FilteredElementCollector(doc)
                .OfClass(typeof(View))
                .ToList();

            if (type == ViewType.Undefined)
            {
                var collection = viewElementList
                    .Select(v => new RevitViewWrap() { View = v as View })
                    .Where(wrapV => !string.IsNullOrEmpty(wrapV.ViewTypeName) && !string.IsNullOrEmpty(wrapV.ViewName))
                    .ToList();
                ;
                return new ObservableCollection<RevitViewWrap>(collection);
            }

            var viewWrapList = (from e in viewElementList
                                let v = e as View
                                where v.ViewType == type
                                select new RevitViewWrap() { View = v })
                .Where(wrapV => !string.IsNullOrEmpty(wrapV.ViewTypeName) && !string.IsNullOrEmpty(wrapV.ViewName))
                                .ToList();

            return new ObservableCollection<RevitViewWrap>(viewWrapList);
        }
    }
}