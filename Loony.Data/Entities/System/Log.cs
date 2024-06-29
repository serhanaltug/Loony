using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.System
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime RecordDate { get; set; }

        public Log()
        {
            RecordDate = DateTime.Now;
        }
    }
}
