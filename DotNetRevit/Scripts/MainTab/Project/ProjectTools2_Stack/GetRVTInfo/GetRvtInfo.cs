using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DotNetRevit.Utilities.WPF;

namespace DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class GetRvtInfo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            return Result.Succeeded;
        }
    }
}