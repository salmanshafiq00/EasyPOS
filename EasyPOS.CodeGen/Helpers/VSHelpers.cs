using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;
using System.IO;

namespace CodeGen.Helpers;

public static class VSHelpers
{
    private static DTE2 DTE { get; } = Package.GetGlobalService(typeof(DTE)) as DTE2;

    public static ProjectItem GetProjectItem(string fileName)
    {
        return DTE.Solution.FindProjectItem(fileName);
    }

    public static void CheckFileOutOfSourceControl(string file)
    {
        if (!File.Exists(file) || DTE.Solution.FindProjectItem(file) == null)
            return;

        if (DTE.SourceControl.IsItemUnderSCC(file) && !DTE.SourceControl.IsItemCheckedOut(file))
            DTE.SourceControl.CheckOutItem(file);
    }

   

    internal static readonly Guid outputPaneGuid = new();

    internal static void WriteOnOutputWindow(string text)
    {
        WriteOnOutputWindow("TypeScript Definition Generator: " + text, outputPaneGuid);
    }
    internal static void WriteOnBuildOutputWindow(string text)
    {
        WriteOnOutputWindow(text, Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid);
    }

    internal static void WriteOnOutputWindow(string text, Guid guidBuildOutput)
    {
        if (!text.EndsWith(Environment.NewLine))
        {
            text += Environment.NewLine;
        }

        // At first write the text on the debug output.
        Debug.Write(text);

        // Now get the SVsOutputWindow service from the service provider.
        if (ServiceProvider.GlobalProvider.GetService(typeof(SVsOutputWindow)) is not IVsOutputWindow outputWindow)
        {
            // If the provider doesn't expose the service there is nothing we can do.
            // Write a message on the debug output and exit.
            Debug.WriteLine("Can not get the SVsOutputWindow service.");
            return;
        }

        // We can not write on the Output window itself, but only on one of its panes.
        // Here we try to use the "General" pane.
        if (Microsoft.VisualStudio.ErrorHandler.Failed(outputWindow.GetPane(ref guidBuildOutput, out IVsOutputWindowPane windowPane)) ||
            (null == windowPane))
        {
            if (Microsoft.VisualStudio.ErrorHandler.Failed(outputWindow.CreatePane(ref guidBuildOutput, "TypeScript Definition Generator", 1, 0)))
            {
                // Nothing to do here, just debug output and exit
                Debug.WriteLine("Failed to create the Output window pane.");
                return;
            }
            if (Microsoft.VisualStudio.ErrorHandler.Failed(outputWindow.GetPane(ref guidBuildOutput, out windowPane)) ||
            (null == windowPane))
            {
                // Again, there is nothing we can do to recover from this error, so write on the
                // debug output and exit.
                Debug.WriteLine("Failed to get the Output window pane.");
                return;
            }
        }
        if (Microsoft.VisualStudio.ErrorHandler.Failed(windowPane.Activate()))
        {
            Debug.WriteLine("Failed to activate the Output window pane.");
            return;
        }

        // Finally we can write on the window pane.
        if (Microsoft.VisualStudio.ErrorHandler.Failed(windowPane.OutputString(text)))
        {
            Debug.WriteLine("Failed to write on the Output window pane.");
        }
    }
}


