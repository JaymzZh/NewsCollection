## NewsCollection

采集网站[http://news.mydrivers.com/](http://news.mydrivers.com/ "mydrivers")上的新闻数据

### 操作手册

- 运行`NewsCollection.Service`项目，按提示安装数据采集服务（只安装）
- 到项目`NewsCollection.Plugin.CollectMyDriverNews`的编译目录下拷贝文件`NewsCollection.Plugin.CollectMyDriverNews.dll`到
`NewsCollection.Service`项目的编译目录下的`Plugins`文件夹下
- 运行`NewsCollection.Service`项目，按提示开始数据采集服务
- 运行`NewsCollection.Web`项目，即可看到采集到的数据

### 技术栈

- [.NET 4.6.1](https://www.microsoft.com/en-us/download/details.aspx?id=49982) Net framework
- [XCode](http://newlifex.com/) ORM框架
- [XAgent](http://newlifex.com/) 代理服务
- [Nancy](http://nancyfx.org/) Web framework
