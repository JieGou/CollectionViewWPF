using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ViewJumperWPF.Model
{
    /// <summary>
    /// Revit视图包装类
    /// </summary>
    public class RevitViewWrap
    {
        /// <summary>
        /// Revit视图
        /// </summary>
        public View View { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName => View?.Name;

        /// <summary>
        /// 视图id
        /// </summary>
        public int Id => View.Id.IntegerValue;

        /// <summary>
        /// 视图类别名称
        /// </summary>
        public string ViewTypeName => View?.get_Parameter(BuiltInParameter.VIEW_TYPE).AsString();
    }
}