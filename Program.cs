namespace WebScraper
{
    internal class Program
    {
        static readonly string url = "https://weather.com/weather/today/l/7ba24b7649e359984c40fa1e72fefb52c16ae53e58537f3186aee60fda326d59";
        static string[]? result;
        static void Main(string[] args)
        {
            WebScraper.CreateHttpClient();

            result = WebScraper.GetResponseResult(url,
                "//*[@id=\"WxuCurrentConditions-main-eb4b02cb-917b-45ec-97ec-d4eb947f6b6a\"]/div/section/div/div/div[1]/h1",
                "//*[@id=\"WxuCurrentConditions-main-eb4b02cb-917b-45ec-97ec-d4eb947f6b6a\"]/div/section/div/div/div[1]/span",
                "//*[@id=\"WxuCurrentConditions-main-eb4b02cb-917b-45ec-97ec-d4eb947f6b6a\"]/div/section/div/div/div[2]/div[1]/div[1]/span",
                "//*[@id=\"WxuCurrentConditions-main-eb4b02cb-917b-45ec-97ec-d4eb947f6b6a\"]/div/section/div/div/div[2]/div[1]/div[1]/div[1]"
            );
            foreach (string item in result)
            {
                Console.WriteLine(item);
            }
        }
    }
}
