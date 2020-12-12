using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ViewJumperWPF
{
    [Transaction(TransactionMode.Manual)]
    public class CmdViewJump : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiapp = commandData.Application;
                var uidoc = uiapp.ActiveUIDocument;

                ViewJumperView viewJumperView = new ViewJumperView(uidoc);

                if (viewJumperView.Datas.WrapViewDatas.Count == 0)
                {
                    TaskDialog.Show("提示", "当前项目没有结构平面，请先手动创建!");
                    return Result.Failed;
                }
                viewJumperView.Show();
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }

            return Result.Succeeded;
        }
    }
}