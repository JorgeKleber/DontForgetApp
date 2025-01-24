
using SQLite;

namespace DontForgetApp.Model
{
	[Table("Reminder")]
	public class Reminder
	{
		[PrimaryKey, AutoIncrement]
		public int IdReminder { get; set; }
		[MaxLength(100), NotNull]
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime RemindDateTime { get; set; }
		//public Byte[] FileAttached { get; set; }
	}
}
