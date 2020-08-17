using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace DotNetRevit
{
    public class App : IExternalApplication
    {
        public static string this_assembly_path = Assembly.GetExecutingAssembly().Location;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AddRibbonPanel(application);
            return Result.Succeeded;
        }

        private static void AddRibbonPanel(UIControlledApplication application)
        {
            string tab_name = "DotNetRevit";
            application.CreateRibbonTab(tab_name);

            RibbonPanel projectRibbonPanel = application.CreateRibbonPanel(tab_name, "Project");

            projectRibbonPanel.AddStackedItems(Scripts.MainTab.Project.ProjectTools2_Stack.B_GetCentralPath.buttonData,
                                               Scripts.MainTab.Project.ProjectTools2_Stack.b_GetRvtInfo.buttonData,
                                               Scripts.MainTab.Project.ProjectTools2_Stack.b_LoadMoreTypes.buttonData);
        }
    }
}