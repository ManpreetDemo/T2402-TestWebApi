namespace TestWebApi230809.Model
{
    public class StoryJson
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public int Id { get; set; }
        public List<int> Kids { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
    public class Story
    {
        public string title { get; set; }
        public string uri { get; set; }
        public string postedBy { get; set; }
        public string time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }
    }
}
