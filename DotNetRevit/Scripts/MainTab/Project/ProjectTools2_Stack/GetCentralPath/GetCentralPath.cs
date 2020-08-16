using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetRevit.Utilities.WPF;

namespace DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class GetCentralPath : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            var modelPath = doc.GetWorksharingCentralModelPath();

            var centralPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(modelPath);

            if (System.Windows.Forms.Control.ModifierKeys == Keys.Shift)
            {
                if (File.Exists(centralPath))
                {
                    Process.Start("explorer.exe", Path.GetDirectoryName(centralPath));
                }
            }
            else
            {
                if (!doc.IsWorkshared)
                {
                    TaskDialog.Show("Get Central Path", "Model is Not Workshared");
                    return Result.Cancelled;
                }

                Query queryWindow = new Query("Get RVT Info");
                queryWindow.Show();
                queryWindow.InfoBox.Text = centralPath;
            }

            return Result.Succeeded;
        }
    }
}