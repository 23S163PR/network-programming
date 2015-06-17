using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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

            var navigator = document.CreateNavigator();

            return Enumerable.Empty<DogeTopicModel>();
        }
    }
}
