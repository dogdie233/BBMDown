# BBMDown

一个用于下载哔哩哔哩漫画的图片的下载器

## 下载

Github release: https://github.com/dogdie233/BBMDown/releases  
dotnet nuget tool: https://www.nuget.org/packages/BBMDown

```
dotnet tool install --global BBMDown
```

## 使用

```
Description:
  下载bilibili漫画

Usage:
  BBMDown <link> [command] [options]

Arguments:
  <link>  要下载的漫画的链接(或者是id)

Options:
  -ep <ep>        需要下载的章节，默认为所有章节
  --sess <sess>   b站账号的sessdata
  --version       Show version information
  -?, -h, --help  Show help and usage information


Commands:
  qrlogin  通过客户端扫描二维码的方式登录
```

## 功能

已实现

- [x] 下载图片到本地
- [x] 通过sessdata登录账号后下载
- [x] 通过客户端扫描二维码登录账号

将来实现

- 自动购买需要下载的章节
- 自定义下载的位置
- 自动重试
- 更好的下载进度提示

## 致谢(排名不分先后)

https://github.com/dotnet/command-line-api  
https://github.com/SocialSisterYi/bilibili-API-collect  
https://github.com/codebude/QRCoder
