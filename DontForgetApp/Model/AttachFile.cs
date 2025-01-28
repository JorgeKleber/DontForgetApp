using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Model
{
	[Table("AttachFile")]
	public class AttachFile
	{
		[PrimaryKey, AutoIncrement]
		public int IdAttach { get; set; }
		public int IdReminder { get; set; }
		public string FileName { get; set; }
		public byte[] FileBytes { get; set; }
	}
}
