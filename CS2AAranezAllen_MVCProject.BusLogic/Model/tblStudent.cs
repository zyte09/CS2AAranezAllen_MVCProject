using System.ComponentModel.DataAnnotations;

namespace CS2AAranezAllen_MVCProject.BusLogic.Model
{
    public class tblStudent
    {
        [Key]
        public int ID { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? Age { get; set; }
        public string? Course { get; set; }

    }
}
