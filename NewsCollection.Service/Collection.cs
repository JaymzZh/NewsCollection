using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsCollention.Entity;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using XCode;

namespace NewsCollection.Service
{
    public class Collection
    {
        /// <summary>
        /// 抓取http://news.mydrivers.com/上的新闻
        /// </summary>
        public static void GetDriverNewsByDate(DateTime date, int page = 1)
        {
            var uri = new Uri($"http://news.mydrivers.com/getnewsupdatelistdata.aspx?data={date.ToString("yyyy-MM-dd")}&pageid={page}");
            var browser = new ScrapingBrowser { Encoding = Encoding.Default };
            // fix OutOfMemoryException issue
            var htmlStr = browser.DownloadString(uri);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlStr);
            var html = doc.DocumentNode;

            //没有页面标签则表示没有内容
            var pageNode = html.CssSelect("div.postpage");
            if(pageNode == null)
                return;
            
            var parentNode = html.CssSelect("div.pce_lb");
            foreach (var newNode in parentNode)
            {
                New @new = new New();

                var titleNode = newNode.CssSelect("div.pce_lb1").FirstOrDefault();
                var a = titleNode?.CssSelect("a");

                var title = a.FirstOrDefault()?.InnerText.Trim();
                @new.Title = title;

                var url = a.FirstOrDefault()?.Attributes["href"]?.Value.Trim();
                //判断该新闻是否已入库
                var n = New.FindByUrl(url);
                if(n != null)
                    continue;

                @new.Url = url;

                @new.Content = GetNewContent(url);

                //查找作者信息
                var authorNode = a.LastOrDefault();
                var authorName = authorNode?.InnerText;
                var author = Author.FindByName(authorName);//首先查询数据库里有该作者没，没有则添加
                if (author == null)
                {
                    author = new Author {Name = authorName };
                    var authorUrl = authorNode?.Attributes["href"]?.Value.Trim();
                    author.Url = authorUrl;
                    author.Save();
                }
                @new.AuthorId = author.Id;

                var time = $"{date.Year}-{titleNode?.CssSelect("ul.hui2 > li").LastOrDefault()?.InnerText}";
                DateTime t;
                DateTime.TryParse(time, out t);
                @new.CreateTime = t;

                //查找标签信息
                var tagsNode = newNode.CssSelect("div.pce_lb2 div.pce_lb2_right1 a");
                var tags = new EntityList<Tag>();
                foreach (var node in tagsNode)
                {
                    var tagName = node.InnerText.Trim();
                    var tag = Tag.FindByName(tagName);
                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Name = node.InnerText.Trim(),
                            Url = node.Attributes["href"]?.Value
                        };
                        tags.Add(tag);
                    }
                }
                tags.Save();

                @new.Save();

                AddNewTag(@new, tags);

                //延迟一定时间，避免IP被封
                Task.Delay(300);
            }

            GoNext(html, date, page);
        }

        private static void GoNext(HtmlNode html, DateTime date, int page)
        {
            //包含首页、上一页、1、下一页、尾页共5个a
            var pagecount = html.CssSelect("div.postpage > a")?.Count() - 5;
            if (pagecount > 0 && pagecount >= page)
                //嵌套循环
                GetDriverNewsByDate(date, page);
        }

        private static void AddNewTag(New @new, List<Tag> tags)
        {
            EntityList<NewTag> newTags = new EntityList<NewTag>();
            foreach (var tag in tags)
            {
                var newTag = new NewTag
                {
                    NewId = @new.Id,
                    TagId = tag.Id
                };
                newTags.Add(newTag);
            }
            newTags.Save();
        }

        /// <summary>
        /// 获取新闻内容
        /// </summary>
        /// <param name="url">新闻链接</param>
        public static string GetNewContent(string url)
        {
            var uri = new Uri(url);
            var browser = new ScrapingBrowser { Encoding = Encoding.Default };
            var html1 = browser.DownloadString(uri);
            var doc = new HtmlDocument();
            doc.LoadHtml(html1);
            var html = doc.DocumentNode;
            var content = new StringBuilder();
            //新闻提取
            var htmlNode = html.CssSelect("div.news_info").FirstOrDefault();
            if (htmlNode != null)
            {
                foreach (var node in htmlNode.CssSelect("p"))
                {
                    var classAttr = node.Attributes["class"];
                    //排除新闻详情下面无用信息（文章纠错、微信）
                    if (classAttr != null && (classAttr.Value.Equals("jcuo1", StringComparison.OrdinalIgnoreCase)
                        || classAttr.Value.Equals("news_bq", StringComparison.OrdinalIgnoreCase)))
                        continue;

                    content.Append(node.OuterHtml);
                }
            }
            else
            {
                //测评提取
                htmlNode = html.CssSelect("div.pc_info").FirstOrDefault();
                if (htmlNode != null)
                {
                    foreach (var node in htmlNode.CssSelect("p"))
                    {
                        var classAttr = node.Attributes["class"];
                        //排除测评详情下面无用信息（更多、相关阅读）
                        if (classAttr != null && classAttr.Value.Equals("news_bq", StringComparison.OrdinalIgnoreCase))
                            break;

                        content.Append(node.OuterHtml);
                    }
                }
            }

            return content.ToString();
        }
    }
}