using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Task_Manager.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TaskController : Controller
	{
		private List<Models.Task> _tasks;

		public TaskController()
		{
			//тут будут даные из бд
			_tasks = new List<Models.Task>() { new Models.Task() {Id= 1, Title="First", Description= "FirstDescription", IsDone=false},
			new Models.Task() {Id= 2, Title="Second", Description= "SecondDescription", IsDone= true}};
		}
		[HttpGet(Name = "GetTasks")]
		public IActionResult GetTasks()
		{
			return Ok(_tasks);
		}

		[HttpGet("{id}", Name = "GetTask")]
		public IActionResult GetTask(int id)
		{
			Models.Task? task = _tasks.FirstOrDefault(i => i.Id == id);
			if (task == null)
			{
				return NotFound();
			}
			return Ok(task);
		}

		[HttpDelete("{id}", Name = "DeleteTask")]
		public IActionResult DeleteTask(int id)
		{
			Models.Task? task = _tasks.FirstOrDefault(i => i.Id == id);
			if (task == null)
				return NotFound();

			_tasks.Remove(task);
			return NoContent();
		}

		[HttpPut("{id}", Name = "UpdateTask")]
		public IActionResult UpdateTask(int id, Models.Task updateTask)
		{
			Models.Task? task = _tasks.FirstOrDefault(i => i.Id == id);
			if (task == null)
				return NotFound();

			task.Title = updateTask.Title;
			task.Description = updateTask.Description;
			task.IsDone = updateTask.IsDone;
			task.UpdatedAt = DateTime.Now;
			return NoContent();
		}

		[HttpPost(Name = "CreateTask")]
		public IActionResult CreateTask(Models.Task task)
		{
			_tasks.Add(task);
			task.CreatedAt = DateTime.Now;
			return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
		}
	}
}