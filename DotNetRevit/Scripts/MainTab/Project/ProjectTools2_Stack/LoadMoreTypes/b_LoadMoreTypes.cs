using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack
{
    public static class b_LoadMoreTypes
    {
        public static PushButtonData buttonData = new PushButtonData("LoadMoreTypes", "Load More Types", App.this_assembly_path, "DotNetRevit.Scripts.MainTab.Project.ProjectTools2_Stack.LoadMoreTypes")
        {
            ToolTip = "Loads other types from selected family instance",
            Image = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Scripts/MainTab/Project/ProjectTools2_Stack/LoadMoreTypes/b_LoadMoreTypes.png")),
        };
    }
}