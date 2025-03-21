using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_Models.DAL.ShowTime
{
    [Table("TimeSlots")]
    public class TimeSlotDAL
    {
        [Column("TimeSlot")]
        public DateTime TimeSlot { get; set; }

        [Column("IsAvailable", TypeName = "int")]
        public int IsAvailable { get; set; }
    }
}
