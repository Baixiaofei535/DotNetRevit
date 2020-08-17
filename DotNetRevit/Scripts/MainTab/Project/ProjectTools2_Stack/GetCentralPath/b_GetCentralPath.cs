using Autodesk.Revit.UI;
using System;
using System.Windows.Media.Imaging;

namespace DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack
{
    public static class B_GetCentralPath
    {
        public static PushButtonData buttonData = new PushButtonData("Get Central Path", "Get Central Path", App.this_assembly_path, "DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack.GetCentralPath")
        {
            ToolTip = "Print the full path to central model (if model is workshared).\n\nShift+Click:\nOpen central model path in file browser",
            Image = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Scripts/MainTab/Project/ProjectTools2_Stack/GetCentralPath/b_GetCentralPath.png")),
        };
    }
}