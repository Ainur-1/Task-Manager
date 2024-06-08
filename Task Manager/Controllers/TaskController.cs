using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.DataAccess;
using Microsoft.Extensions.Logging;


namespace Task_Manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository taskRepository, ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}", Name = "GetTask")]
        public IActionResult GetTask(int id)
        {
            _logger.LogInformation($"User tried to get task with id: {id}");

            Models.Task? task = _taskRepository.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{id}", Name = "DeleteTask")]
        public IActionResult DeleteTask(int id)
        {
            _logger.LogInformation($"User tried to delete task with id: {id}");

            Models.Task? task = _taskRepository.GetTask(id);
            if (task == null)
                return NotFound();

            _taskRepository.DeleteTask(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}", Name = "UpdateTask")]
        public IActionResult UpdateTask(int id, Models.Task updateTask)
        {
            _logger.LogInformation($"User tried to update task with id: {id}");

            Models.Task? task = _taskRepository.GetTask(id);
            if (task == null)
                return NotFound();

            task.Title = updateTask.Title;
            task.Description = updateTask.Description;
            task.IsDone = updateTask.IsDone;
            task.UpdatedAt = DateTime.Now;

            _taskRepository.UpdateTask(id, task);
            return NoContent();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost(Name = "CreateTask")]
        public IActionResult CreateTask(Models.Task task)
        {
            _logger.LogInformation($"User tried to create a new task: {task.Title}");

            _taskRepository.CreateTask(task);
            task.CreatedAt = DateTime.Now;
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
    }
}