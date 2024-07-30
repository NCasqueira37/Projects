using HtmlAgilityPack;

namespace WebScraper
{
    #region WebScraper
    class WebScraper
    {
        public static HttpClient? httpClient;
        public static string[] GetResponseResult(string url, params string[] xPath)
        {
            var response = CallUrlAsync(url).Result;
            string[] result = new string[xPath.Length];

            for (int i = 0; i < xPath.Length; i++)
            {
                result[i] = GetText(response, xPath[i]);
            }
            
            return result;
        }
        public static void CreateHttpClient()
        {
            httpClient = new HttpClient();
        }
        private static async Task<string> CallUrlAsync(string url)
        {
            if (httpClient != null)
            {
                var response = await httpClient.GetStringAsync(url);
                return response;
            }
            else
            {
                Console.WriteLine("HttpClient not set");
                Console.WriteLine("Call WebScraper.CreateHttpClient() before getting result");
                throw new NullReferenceException();
            }
        }
        private static string GetText(string response, string xPath)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            var htmlNode = htmlDocument.DocumentNode.SelectSingleNode(xPath);
            var text = htmlNode.InnerText;
            return text.ToString();
        }
    }
    #endregion
}
