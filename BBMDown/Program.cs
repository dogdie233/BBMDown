using BBMDown;

using System.CommandLine;
using System.CommandLine.Binding;

#region Build Command
var rootCommandBinder = new RootCommandBinder();
var rootCommand = new RootCommand("下载bilibili漫画")
{
    rootCommandBinder.link,
    rootCommandBinder.pages
};

rootCommand.SetHandler<RootCommandOptions, IConsole>(HandleRootCommand, rootCommandBinder);

return rootCommand.Invoke(args);
#endregion

#region Command Handlers
void HandleRootCommand(RootCommandOptions options, IConsole console)
{
    var logger = new Logger(console);
    var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    if (version == null) { return; }
    logger.Info($"欢迎使用 BBMDown, 当前版本: {version.Major}.{version.Minor}.{version.Build}");

    logger.Info(options.Link);
    logger.Info(options.PagesString);
}
#endregion

#region Option Types
public class RootCommandOptions
{
    public RootCommandOptions(string link, string pages)
    {
        Link = link;
        PagesString = pages;
    }

    public string Link { get; set; }
    public string PagesString { get; set; }
}
public class RootCommandBinder : BinderBase<RootCommandOptions>
{
    public readonly Argument<string> link = new("link", "要下载的漫画的链接(或者是id)");
    public readonly Option<string> pages = new(new string[] { "--pages", "-p" }, "需要下载的，默认为所有");

    protected override RootCommandOptions GetBoundValue(BindingContext bindingContext) => new(
        bindingContext.ParseResult.GetValueForArgument(link),
        bindingContext.ParseResult.GetValueForOption(pages) ?? "all");
}
#endregion