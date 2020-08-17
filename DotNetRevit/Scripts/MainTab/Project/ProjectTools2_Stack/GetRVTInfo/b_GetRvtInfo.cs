using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack
{
    public static class b_GetRvtInfo
    {
        public static PushButtonData buttonData = new PushButtonData("Get RVT Info", "Get RVT Info", App.this_assembly_path, "DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack.GetRvtInfo")
        {
            ToolTip = "Get information from RVT file.",
            Image = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Scripts/MainTab/Project/ProjectTools2_Stack/GetRVTInfo/b_GetRvtInfo.png")),
        };
    }
}