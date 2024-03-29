﻿//Arjun Mukherji - Rights to distribute and modify granted.

namespace MyTube.Classes
{
    //api information at http://code.google.com/apis/youtube/2.0/developers_guide_protocol_api_query_parameters.html
    class Constants
    {
        public const string SEARCH_URL = "http://gdata.youtube.com/feeds/api/videos?q={0}&alt=rss&max-results={1}&start-index={2}&v=2";
        public const string PREVIOUS = "previous";
        public const string NEXT = "next";
        public const int MAX_RESULTS = 10;
        public const string EXTRACTION_COMPLETE = "Extraction Complete";
        public const string AUDIO = "audio";
        public const string VIDEO = "video";
    }
}
