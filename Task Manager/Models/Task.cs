using System.ComponentModel.DataAnnotations;

namespace Task_Manager.Models
{
	public class Task
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Title { get; set; }

		[StringLength(1000)]
		public string Description { get; set; }

		public DateTime? DueDate { get; set; }

		public bool IsDone { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}
}
