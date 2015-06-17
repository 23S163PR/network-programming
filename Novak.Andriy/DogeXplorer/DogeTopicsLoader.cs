using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DogeXplorer
{
    public class DogeTopicsLoader
    {
        private const string TopTopicsUrl = "http://www.reddit.com/r/doge/top/?sort=top&t=all";

        public IEnumerable<DogeTopicModel> LoadTopics()
        {
            var request = (HttpWebRequest)WebRequest.Create(TopTopicsUrl);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.124 Safari/537.36";
            request.Method = "GET";
            request.Accept = "text/html";

            var responseStream = request
                .GetResponse()
                .GetResponseStream();

            var document = new HtmlDocument();
            document.Load(responseStream);
            const string xPath =
                "//div[@class='midcol unvoted']/div[@class='score unvoted'] | //a[@class='title may-blank ']";
            var colection = document.DocumentNode.SelectNodes(xPath);
            //var likeColection = document.DocumentNode.SelectNodes("//div[@class='midcol unvoted']/div[@class='score unvoted']");
            //var titleColection = document.DocumentNode.SelectNodes("//a[@class='title may-blank ']");
            //var res = from like in likeColection
            //          join title in titleColection on likeColection.IndexOf(like) equals titleColection.IndexOf(title)
            //          select new DogeTopicModel()
            //          {
            //              Upvotes = like.FirstChild.InnerHtml
            //              ,
            //              Title = title.InnerHtml
            //          };
            var result = new ObservableCollection<DogeTopicModel>();
            for (var i = 0; i < colection.Count; i+=2)
            {
                result.Add(new DogeTopicModel
                {
                    Upvotes = colection[i].InnerHtml
                    ,Title = colection[i+1].InnerHtml
                });
            }
            return result;
        }
    }
}
