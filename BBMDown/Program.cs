using BBMDown;
using BBMDown.Models;
using BBMDown.Requests;

using System.CommandLine;
using System.CommandLine.Binding;
using System.Security.Cryptography;
using System.Text;

#region Build Command
var rootCommandBinder = new RootCommandBinder();
var rootCommand = new RootCommand("下载bilibili漫画")
{
    rootCommandBinder.link,
    rootCommandBinder.ep,
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
    var comicDetailRep = HttpHelper.SendAsync(new ComicDetailRequest(comicId)).Result;
    if (comicDetailRep.Data == null)
    {
        logger.Error($"在获取漫画信息时返回了空数据, StatusCode:{comicDetailRep.StatusCode}, Msg:{comicDetailRep.Msg}");
        return;
    }
    var comicDetail = comicDetailRep.Data;
    logger.Info($"漫画名: {comicDetail.Title}");

    logger.Info("正在确认待下载的章节");
    var epList = comicDetail.EpList.OrderBy(ep => ep.Order).ToArray();
    var inputEps = Utils.ParseIntRangeString(options.EpString).ToList();
    var downloadEpOrder = new List<int>();
    var downloadEps = new List<EpModel>();
    var undownloadableEps = new List<EpModel>();
    foreach (var order in inputEps)
    {
        if (order < 1 || order > epList.Length - 1)
        {
            undownloadableEps.Add(new() { ShortTitle = order.ToString(), Title = "章节不存在" });
            continue;
        }
        if (epList[order - 1].IsInFree || !epList[order - 1].IsLocked)
        {
            downloadEps.Add(epList[order - 1]);
            downloadEpOrder.Add(order);
            continue;
        }
        undownloadableEps.Add(epList[order - 1]);
    }
    if (options.EpString == "all")
    {
        downloadEps = epList.Where(ep => !ep.IsLocked || ep.IsInFree).ToList();
        downloadEpOrder = Enumerable.Range(1, downloadEps.Count).ToList();
    }
    
    logger.Info($"总计 {downloadEps.Count} 个章节准备下载" + BuildEpListString(downloadEps));
    logger.Info($"总计 {undownloadableEps.Count} 个章节无法下载" + BuildEpListString(undownloadableEps));

    logger.Info("开始下载漫画");
    for (int i = 0; i < downloadEps.Count; i++)
    {
        var ep = downloadEps[i];
        logger.Info($" [{i + 1}/{downloadEps.Count}]正在获取章节 {ep.ShortTitle} {ep.Title} 的图片信息...");
        var images = HttpHelper.SendAsync(new ImageIndexRequest(ep.Id)).Result;
        if (images.Data == null)
        {
            logger.Warn($"  在获取图片信息时返回了空数据, 跳过此章节, StatusCode:{images.StatusCode}, Msg:{images.Msg}");
            continue;
        }
        var ts = images.Data.Path.Substring(images.Data.Path.IndexOf("ts=") + 3, 8);
        logger.Info($"  获取到了 {images.Data.Images.Length} 张图片，正在获取token...");
        var tokens = HttpHelper.SendAsync(new ImageTokenRequest(images.Data.Images.Select(image => images.Data.Host + image.Path).ToArray())).Result;
        if (tokens.Data == null)
        {
            logger.Info($"   在获取图片token时返回了空数据, 跳过此章节, StatusCode:{tokens.StatusCode}, Msg:{images.Msg}");
            continue;
        }

        var directory = $"{comicDetail.Title}/{downloadEpOrder[i].ToString().PadLeft(5, '0')} {ep.ShortTitle} {ep.Title}";
        logger.Info($"   开始下载章节 \"{ep.Title}\", 将会保存到 \"{directory}\"");
        Directory.CreateDirectory(directory);
        for (int j = 0; j < tokens.Data.Length; j++)
        {
            var token = tokens.Data[j];
            var path = directory + '/' + (j + 1).ToString().PadLeft(3, '0') + token.Url.Substring(token.Url.LastIndexOf('.'), token.Url.Length - token.Url.LastIndexOf('.'));
            if (File.Exists(path) && new FileInfo(path).Length != 0) continue;
            logger.Info($"    正在下载{j + 1}/{tokens.Data.Length}");
            using (var fs = File.Create(path))
            {
                HttpHelper.DownloadFile($"{token.Url}?token={token.Token}&ts={ts}", fs).Wait();
            }
        }
    }
}

void HandleQRLoginCommand(IConsole console)
{
    var logger = new Logger(console);
    var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    if (string.IsNullOrEmpty(dataPath)) return;
    logger.Info(dataPath);
}
#endregion

string BuildEpListString(IEnumerable<EpModel> eps)
{
    var sb = new StringBuilder();
    foreach (var ep in eps)
    {
        sb.AppendLine();
        sb.Append(" - ");
        sb.Append(ep.ShortTitle);
        sb.Append(' ');
        sb.Append(ep.Title);
    }
    return sb.ToString();
}

#region Option Types
public class RootCommandOptions
{
    public string Link { get; set; } = string.Empty;
    public string EpString { get; set; } = string.Empty;
    public string? Sessdata { get; set; } = null;
}
public class RootCommandBinder : BinderBase<RootCommandOptions>
{
    public readonly Argument<string> link = new("link", "要下载的漫画的链接(或者是id)");
    public readonly Option<string> ep = new(new string[] { "-ep" }, "需要下载的章节，默认为所有章节");
    public readonly Option<string> sessdata = new(new string[] { "--sess" }, "b站的sessdata");

    protected override RootCommandOptions GetBoundValue(BindingContext bindingContext) => new()
    {
        Link = bindingContext.ParseResult.GetValueForArgument(link),
        EpString = bindingContext.ParseResult.GetValueForOption(ep) ?? "all",
        Sessdata = bindingContext.ParseResult.GetValueForOption(sessdata)
    };
}
#endregion