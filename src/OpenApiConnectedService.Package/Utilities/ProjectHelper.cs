using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace OpenApiConnectedService.Package.Utilities
{
    /// <summary>
    /// A utility class for working with Visual Studio project system.
    /// </summary>
    internal static class ProjectHelper
    {
        public static Project GetProject(this IVsHierarchy projectHierarchy)
        {
            int result = projectHierarchy.GetProperty(
                VSConstants.VSITEMID_ROOT,
                (int)__VSHPROPID.VSHPROPID_ExtObject,
                out object projectObject);
            ErrorHandler.ThrowOnFailure(result);
            return (Project)projectObject;
        }

        public static string GetNameSpace(this Project project)
        {
            return project.Properties.Item("DefaultNamespace").Value.ToString();
        }
    }
}
