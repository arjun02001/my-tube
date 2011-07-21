using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MyTube.Classes
{
    //http://code.google.com/apis/youtube/2.0/developers_guide_protocol_api_query_parameters.html
    class Constants
    {
        public const string SEARCH_URL = "http://gdata.youtube.com/feeds/api/videos?q={0}&alt=rss&max-results=10&start-index={1}&v=2";
        public const string PREVIOUS = "previous";
        public const string NEXT = "next";
        public const int MAX_RESULTS = 10;
    }
}
