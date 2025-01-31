using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Model
{
	public class EventModel
	{
		public DateTime DateEvent { get; set; }
		public List<Reminder> RemindersList { get; set; }
	}
}
