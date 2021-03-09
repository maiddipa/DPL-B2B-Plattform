namespace Dpl.B2b.Dal.Models
{
    public class UserSetting : OlmaAuditable
    {
        public int Id { get; set; }

        //TODO Add [Index]
        //public string Name { get; set; }

        public int UserId { get; set; }

        public Newtonsoft.Json.Linq.JObject Data { get; set; }
    }
}