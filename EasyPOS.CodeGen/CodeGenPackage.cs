global using System;
global using System.Text;
global using System.Linq;
using EasyPOS.CodeGen.Helpers;
using EasyPOS.CodeGen.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using Microsoft;
using EnvDTE;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio;
using EasyPOS.CodeGen;

namespace EasyPOS.CodeGen;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version, IconResourceID = 400)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[Guid(PackageGuids.CodeGenString)]
public sealed class CodeGenPackage : AsyncPackage
{
    public const string DOMAINPROJECT = "EasyPOS.Domain";
    public const string UIPROJECT = "EasyPOS.WebApi";
    public const string INFRASTRUCTUREPROJECT = "EasyPOS.Infrastructure";
    public const string APPLICATIONPROJECT = "EasyPOS.Application";
    //public const string ClientApp = "EasyPOS.Client";

    private const string _solutionItemsProjectName = "Solution Items";
    private static readonly Regex _reservedFileNamePattern = new($@"(?i)^(PRN|AUX|NUL|CON|COM\d|LPT\d)(\.|$)");
    private static readonly HashSet<char> _invalidFileNameChars = new(Path.GetInvalidFileNameChars());

    public static string DomainRootNs = "";
    public static string ApplicaitonRootNs = "";
    public static string InfrastructureRootNs = "";
    public static string WebRootNs = "";

    public static DTE2 _dte;

    protected async override Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync();

        _dte = await GetServiceAsync(typeof(DTE)) as DTE2;
        Assumes.Present(_dte);

        Logger.Initialize(this, Vsix.Name);

        if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
        {
            CommandID menuCommandID = new(PackageGuids.CodeGen, PackageIds.MyCommand);
            OleMenuCommand menuItem = new(Execute, menuCommandID);
            mcs.AddCommand(menuItem);
        }
    }

    private void Execute(object sender, EventArgs e)
    {
        NewItemTarget target = NewItemTarget.Create(_dte);
        NewItemTarget domain = NewItemTarget.Create(_dte, DOMAINPROJECT);
        NewItemTarget infrastructure = NewItemTarget.Create(_dte, INFRASTRUCTUREPROJECT);
        NewItemTarget ui = NewItemTarget.Create(_dte, UIPROJECT);
        NewItemTarget application = NewItemTarget.Create(_dte, APPLICATIONPROJECT);
        //NewItemTarget clientApp = NewItemTarget.Create(_dte, ClientApp);

        DomainRootNs = domain.Project.GetRootNamespace();
        ApplicaitonRootNs = application.Project.GetRootNamespace();
        InfrastructureRootNs = infrastructure.Project.GetRootNamespace();
        WebRootNs = ui.Project.GetRootNamespace();

        var includes = new string[] { "IEntity", "BaseEntity", "BaseAuditableEntity", "BaseAuditableSoftDeleteEntity", "AuditTrail", "OwnerPropertyEntity" };
        var objectlist = ProjectHelpers.GetEntities(domain.Project)
            .Where(x => includes.Contains(x.BaseName) && !includes.Contains(x.Name));
        var entities = objectlist.Select(x => x.Name).Distinct().ToArray();
        if (target == null && target.Project.Name == APPLICATIONPROJECT)
        {
            MessageBox.Show(
                    "Unable to determine the location for creating the new file. Please select a folder within the Application Project in the Explorer and try again.",
                    Vsix.Name,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            return;
        }

        string input = PromptForFileName(target.Directory, entities).TrimStart('/', '\\').Replace("/", "\\");

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        string[] parsedInputs = GetParsedInput(input);

        foreach (string inputname in parsedInputs)
        {
            try
            {
                var name = Path.GetFileNameWithoutExtension(inputname);
                var nameofPlural = ProjectHelpers.Pluralize(name);
                var objectClass = objectlist.Where(x => x.Name == name).First();

                // For Domain Event
                //var events = new List<string>() {
                //        $"Events/{name}CreatedEvent.cs",
                //        $"Events/{name}DeletedEvent.cs",
                //        $"Events/{name}UpdatedEvent.cs",
                //        };
                //foreach (var item in events)
                //{
                //    AddItemAsync(objectClass, item, name, domain).Forget();
                //}

                var configurations = new List<string>() {
                         $"Persistence/Configurations/{name}Configuration.cs"
                        };
                foreach (var item in configurations)
                {
                    AddItemAsync(objectClass, item, name, infrastructure).Forget();
                }

                var list = new List<string>()
                {
                    $"{nameofPlural}/Commands/Create/Create{name}Command.cs",
                    $"{nameofPlural}/Commands/Create/Create{name}CommandValidator.cs",
                    $"{nameofPlural}/Commands/Delete/Delete{name}Command.cs",
                    $"{nameofPlural}/Commands/MultipleDel/Delete{nameofPlural}Command.cs",
                    $"{nameofPlural}/Commands/Update/Update{name}Command.cs",
                    $"{nameofPlural}/Commands/Update/Update{name}CommandValidator.cs",

                    $"{nameofPlural}/Queries/GetAll/Get{nameofPlural}ListQuery.cs",
                    $"{nameofPlural}/Queries/GetById/Get{name}ByIdQuery.cs",
                    $"{nameofPlural}/Queries/Model/{name}Model.cs"


                    //$"{nameofPlural}/Commands/AddEdit/AddEdit{name}Command.cs",
                    //$"{nameofPlural}/Commands/AddEdit/AddEdit{name}CommandValidator.cs",

                    //$"{nameofPlural}/Commands/Import/Import{nameofPlural}Command.cs",
                    //$"{nameofPlural}/Commands/Import/Import{nameofPlural}CommandValidator.cs",
                    //$"{nameofPlural}/Caching/{name}CacheKey.cs",
                    //$"{nameofPlural}/DTOs/{name}Dto.cs",
                    //$"{nameofPlural}/EventHandlers/{name}CreatedEventHandler.cs",
                    //$"{nameofPlural}/EventHandlers/{name}UpdatedEventHandler.cs",
                    //$"{nameofPlural}/EventHandlers/{name}DeletedEventHandler.cs",
                    //$"{nameofPlural}/Specifications/{name}AdvancedFilter.cs",
                    //$"{nameofPlural}/Specifications/{name}AdvancedSpecification.cs",
                    //$"{nameofPlural}/Specifications/{name}ByIdSpecification.cs",
                    //$"{nameofPlural}/Queries/Export/Export{nameofPlural}Query.cs",


                    //$"{nameofPlural}/Queries/Pagination/{nameofPlural}PaginationQuery.cs",

                };
                foreach (var item in list)
                {
                    AddItemAsync(objectClass, item, name, target).Forget();
                }

                // Add Endpoints
                var pages = new List<string>()
                {
                    $"Endpoints/{nameofPlural}.cs",
                };
                foreach (var item in pages)
                {
                    AddItemAsync(objectClass, item, name, ui).Forget();
                }

                // Generated Client
                var generatedClientPages = new List<string>()
                {
                    $"GeneratedClient/List/{name.ToLower()}-list/{name.ToLower()}-list.component.html",
                    $"GeneratedClient/List/{name.ToLower()}-list/{name.ToLower()}-list.component.scss",
                    $"GeneratedClient/List/{name.ToLower()}-list/{name.ToLower()}-list.component.ts",
                    $"GeneratedClient/Detail/{name.ToLower()}-detail/{name.ToLower()}-detail.component.html",
                    $"GeneratedClient/Detail/{name.ToLower()}-detail/{name.ToLower()}-detail.component.scss",
                    $"GeneratedClient/Detail/{name.ToLower()}-detail/{name.ToLower()}-detail.component.ts",
                };
                foreach (var item in generatedClientPages)
                {
                    AddItemAsync(objectClass, item, name, ui).Forget();
                }
            }
            catch (Exception ex) when (!ErrorHandler.IsCriticalException(ex))
            {
                Logger.Log(ex);
                MessageBox.Show(
                        $"Error creating file '{inputname}':{Environment.NewLine}{ex.Message}",
                        Vsix.Name,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
        }
    }

    private async Task AddItemAsync(IntellisenseObject classObject, string name, string itemname, NewItemTarget target)
    {
        // The naming rules that apply to files created on disk also apply to virtual solution folders,
        // so regardless of what type of item we are creating, we need to validate the name.
        ValidatePath(name);

        if (name.EndsWith("\\", StringComparison.Ordinal))
        {
            if (target.IsSolutionOrSolutionFolder)
            {
                GetOrAddSolutionFolder(name, target);
            }
            else
            {
                AddProjectFolder(name, target);
            }
        }
        else
        {
            await AddFileAsync(classObject, name, itemname, target);
        }
    }

    private void ValidatePath(string path)
    {
        do
        {
            string name = Path.GetFileName(path);

            if (_reservedFileNamePattern.IsMatch(name))
            {
                throw new InvalidOperationException($"The name '{name}' is a system reserved name.");
            }

            if (name.Any(c => _invalidFileNameChars.Contains(c)))
            {
                throw new InvalidOperationException($"The name '{name}' contains invalid characters.");
            }

            path = Path.GetDirectoryName(path);
        } while (!string.IsNullOrEmpty(path));
    }

    private async Task AddFileAsync(IntellisenseObject classObject, string name, string itemname, NewItemTarget target)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync();
        FileInfo file;
        FileInfo orgFile = null;

        // If the file is being added to a solution folder, but that
        // solution folder doesn't have a corresponding directory on
        // disk, then write the file to the root of the solution instead.
        if (target.IsSolutionFolder && !Directory.Exists(target.Directory))
        {
            file = new FileInfo(Path.Combine(Path.GetDirectoryName(_dte.Solution.FullName), Path.GetFileName(name)));
        }
        else
        {
            var removedFolderName = RemoveFolderNameFromFile(name);
            file = new FileInfo(Path.Combine(target.Directory, removedFolderName));
            orgFile = new FileInfo(Path.Combine(target.Directory, name));
        }

        // Make sure the directory exists before we create the file. Don't use
        // `PackageUtilities.EnsureOutputPath()` because it can silently fail.
        //var removedFolderName = "";
        //try
        //{
        //     removedFolderName = RemoveFolderNameFromFile(file.DirectoryName, 0);
        //}
        //catch (Exception ex)
        //{

        //    throw;
        //}
        Directory.CreateDirectory(file.DirectoryName);

        if (!file.Exists && orgFile is not null)
        {
            Project project;

            if (target.IsSolutionOrSolutionFolder)
            {
                project = GetOrAddSolutionFolder(Path.GetDirectoryName(name), target);
            }
            else
            {
                project = target.Project;
            }

            int position = await WriteFileAsync(project, classObject, orgFile.FullName, itemname, target.Directory);
            if (target.ProjectItem != null && target.ProjectItem.IsKind(Constants.vsProjectItemKindVirtualFolder))
            {
                target.ProjectItem.ProjectItems.AddFromFile(file.FullName);
            }
            else
            {
                project.AddFileToProject(file);
            }

            VsShellUtilities.OpenDocument(this, file.FullName);

            // Move cursor into position.
            if (position > 0)
            {
                Microsoft.VisualStudio.Text.Editor.IWpfTextView view = ProjectHelpers.GetCurentTextView();

                view?.Caret.MoveTo(new SnapshotPoint(view.TextBuffer.CurrentSnapshot, position));
            }

            ExecuteCommandIfAvailable("SolutionExplorer.SyncWithActiveDocument");
            _dte.ActiveDocument.Activate();
        }
        else
        {
            Console.WriteLine($"The file '{file}' already exists.");
        }
    }

    private static async Task<int> WriteFileAsync(Project project, IntellisenseObject classObject, string file, string itemname, string selectFolder)
    {
        string template = await TemplateMap.GetTemplateFilePathAsync(project, classObject, file, itemname, selectFolder);

        if (!string.IsNullOrEmpty(template))
        {
            int index = template.IndexOf('$');
            string modifiedFile = RemoveFolderNameFromFile(file);
            await WriteToDiskAsync(modifiedFile, template);
            return index;
        }

        await WriteToDiskAsync(file, string.Empty);

        return 0;
    }

    public static string RemoveFolderNameFromFile(string file, int tailSlashRemove = 1)
    {
        string folderName = string.Empty;

        bool containsCommands = file.Contains("Commands");
        bool containsQueries = file.Contains("Queries");

        if (!containsCommands && !containsQueries)
        {
            return file;
        }

        if (file.Contains("Create"))
        {
            folderName = "Create";
        }
        if (file.Contains("Update"))
        {
            folderName = "Update";
        }
        if (file.Contains("Delete"))
        {
            folderName = "Delete";
        }
        if (file.Contains("MultipleDel"))
        {
            folderName = "MultipleDel";
        }
        if (file.Contains("GetAll"))
        {
            folderName = "GetAll";
        }
        if (file.Contains("GetById"))
        {
            folderName = "GetById";
        }
        if (file.Contains("Model"))
        {
            folderName = "Model";
        }
        int indexOfFolderName = file.IndexOf(folderName);
        StringBuilder modifiedFile = new(file.Substring(0, indexOfFolderName));
        modifiedFile.Append(file.Substring(indexOfFolderName + folderName.Length + tailSlashRemove));
        return modifiedFile.ToString();
    }

    private static async Task WriteToDiskAsync(string file, string content)
    {
        using StreamWriter writer = new(file, false, GetFileEncoding(file));
        await writer.WriteAsync(content);
    }

    private static Encoding GetFileEncoding(string file)
    {
        string[] noBom = [".cmd", ".bat", ".json"];
        string ext = Path.GetExtension(file).ToLowerInvariant();
        return noBom.Contains(ext) ? new UTF8Encoding(false) : (Encoding)new UTF8Encoding(true);
    }

    private Project GetOrAddSolutionFolder(string name, NewItemTarget target)
    {
        if (target.IsSolution && string.IsNullOrEmpty(name))
        {
            // An empty solution folder name means we are not creating any solution 
            // folders for that item, and the file we are adding is intended to be 
            // added to the solution. Files cannot be added directly to the solution,
            // so there is a "Solution Items" folder that they are added to.
            return _dte.Solution.FindSolutionFolder(_solutionItemsProjectName)
                    ?? ((Solution2)_dte.Solution).AddSolutionFolder(_solutionItemsProjectName);
        }

        // Even though solution folders are always virtual, if the target directory exists,
        // then we will also create the new directory on disk. This ensures that any files
        // that are added to this folder will end up in the corresponding physical directory.
        if (Directory.Exists(target.Directory))
        {
            // Don't use `PackageUtilities.EnsureOutputPath()` because it can silently fail.
            Directory.CreateDirectory(Path.Combine(target.Directory, name));
        }

        Project parent = target.Project;

        foreach (string segment in SplitPath(name))
        {
            // If we don't have a parent project yet, 
            // then this folder is added to the solution.
            if (parent == null)
            {
                parent = _dte.Solution.FindSolutionFolder(segment) ?? ((Solution2)_dte.Solution).AddSolutionFolder(segment);
            }
            else
            {
                parent = parent.FindSolutionFolder(segment) ?? ((SolutionFolder)parent.Object).AddSolutionFolder(segment);
            }
        }

        return parent;
    }

    private void AddProjectFolder(string name, NewItemTarget target)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        // Make sure the directory exists before we add it to the project. Don't
        // use `PackageUtilities.EnsureOutputPath()` because it can silently fail.
        Directory.CreateDirectory(Path.Combine(target.Directory, name));

        // We can't just add the final directory to the project because that will 
        // only add the final segment rather than adding each segment in the path.
        // Split the name into segments and add each folder individually.
        ProjectItems items = target.ProjectItem?.ProjectItems ?? target.Project.ProjectItems;
        string parentDirectory = target.Directory;

        foreach (string segment in SplitPath(name))
        {
            parentDirectory = Path.Combine(parentDirectory, segment);

            // Look for an existing folder in case it's already in the project.
            ProjectItem folder = items
                    .OfType<ProjectItem>()
                    .Where(item => segment.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                    .Where(item => item.IsKind(Constants.vsProjectItemKindPhysicalFolder, Constants.vsProjectItemKindVirtualFolder))
                    .FirstOrDefault();

            folder ??= items.AddFromDirectory(parentDirectory);
            items = folder.ProjectItems;
        }
    }

    private static string[] SplitPath(string path)
    {
        return path.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] GetParsedInput(string input)
    {
        Regex pattern = new(@"[,]?([^(,]*)([\.\/\\]?)[(]?((?<=[^(])[^,]*|[^)]+)[)]?");
        List<string> results = [];
        Match match = pattern.Match(input);

        while (match.Success)
        {
            // Always 4 matches w. Group[3] being the extension, extension list, folder terminator ("/" or "\"), or empty string
            string path = match.Groups[1].Value.Trim() + match.Groups[2].Value;
            string[] extensions = match.Groups[3].Value.Split(',');

            foreach (string ext in extensions)
            {
                string value = path + ext.Trim();

                // ensure "file.(txt,,txt)" or "file.txt,,file.txt,File.TXT" returns as just ["file.txt"]
                if (value != "" && !value.EndsWith(".", StringComparison.Ordinal) && !results.Contains(value, StringComparer.OrdinalIgnoreCase))
                {
                    results.Add(value);
                }
            }
            match = match.NextMatch();
        }
        return [.. results];
    }

    private string PromptForFileName(string folder, string[] entities)
    {
        DirectoryInfo dir = new(folder);
        FileNameDialog dialog = new(dir.Name, entities)
        {
            Owner = Application.Current.MainWindow
        };

        bool? result = dialog.ShowDialog();
        return (result.HasValue && result.Value) ? dialog.Input : string.Empty;
    }

    private void ExecuteCommandIfAvailable(string commandName)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        Command command;

        try
        {
            command = _dte.Commands.Item(commandName);
        }
        catch (ArgumentException)
        {
            // The command does not exist, so we can't execute it.
            return;
        }
        if (command.IsAvailable)
        {
            _dte.ExecuteCommand(commandName);
        }
    }
}
