using BBMDown;
using BBMDown.Requests;

using System.CommandLine;
using System.CommandLine.Binding;

#region Build Command
var rootCommandBinder = new RootCommandBinder();
var rootCommand = new RootCommand("下载bilibili漫画")
{
    rootCommandBinder.link,
    rootCommandBinder.pages,
    rootCommandBinder.sessdata
};

rootCommand.SetHandler<RootCommandOptions, IConsole>(HandleRootCommand, rootCommandBinder);

var qrCodeLoginCommand = new Command("qrlogin", "通过客户端扫描二维码的方式登录");
qrCodeLoginCommand.SetHandler<IConsole>(HandleQRLoginCommand);
rootCommand.AddCommand(qrCodeLoginCommand);

return rootCommand.Invoke(args);
#endregion

#region Command Handlers
void HandleRootCommand(RootCommandOptions options, IConsole console)
{
    var logger = new Logger(console);
    var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    if (version == null) return;
    logger.Info($"欢迎使用 BBMDown, 当前版本: {version.Major}.{version.Minor}.{version.Build}");
    if (options.Sessdata != null) HttpHelper.SetSessdata(options.Sessdata);

    if (!Utils.TryGetComicIdByLink(options.Link, out var comicId))
    {
        logger.Error($"无法在{options.Link}中找到漫画id");
        return;
    }
    logger.Info($"待下载的漫画Id: {comicId}");

    logger.Info($"正在获取漫画信息...");
    var comicDetail = HttpHelper.SendAsync(new ComicDetailRequest(comicId)).Result;
    if (comicDetail.Data == null)
    {
        logger.Error($"在获取漫画信息时返回了空数据, StatusCode:{comicDetail.StatusCode}, Msg:{comicDetail.Msg}");
        return;
    }
    logger.Info($"漫画名: {comicDetail.Data.Title}");
}

void HandleQRLoginCommand(IConsole console)
{
    var logger = new Logger(console);
    var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    if (string.IsNullOrEmpty(dataPath)) return;
    logger.Info(dataPath);
}
#endregion

#region Option Types
public class RootCommandOptions
{
    public string Link { get; set; } = string.Empty;
    public string PagesString { get; set; } = string.Empty;
    public string? Sessdata { get; set; } = null;
}
public class RootCommandBinder : BinderBase<RootCommandOptions>
{
    public readonly Argument<string> link = new("link", "要下载的漫画的链接(或者是id)");
    public readonly Option<string> pages = new(new string[] { "--pages", "-p" }, "需要下载的章节，默认为所有章节");
    public readonly Option<string> sessdata = new(new string[] { "--sess" }, "b站的sessdata");

    protected override RootCommandOptions GetBoundValue(BindingContext bindingContext) => new()
    {
        Link = bindingContext.ParseResult.GetValueForArgument(link),
        PagesString = bindingContext.ParseResult.GetValueForOption(pages) ?? "all",
        Sessdata = bindingContext.ParseResult.GetValueForOption(sessdata)
    };
}
#endregion