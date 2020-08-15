using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace DotNetRevit
{
    public class App : IExternalApplication
    {
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
        }
    }
}