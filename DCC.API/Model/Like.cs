namespace DCC.API.Model
{
    public class Like
    {
        public string LikerId { get; set; }
        public string LikeeId { get; set; }
        public User Liker { get; set; }
        public User Likee { get; set; }

    }
}