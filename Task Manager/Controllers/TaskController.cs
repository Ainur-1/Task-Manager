using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.DataAccess;
using Task_Manager.Models;
using Task_Manager.Services;

namespace Task_Manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;
        private readonly EmailService _emailService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TaskController(ITaskRepository taskRepository, ILogger<TaskController> logger, EmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _emailService = emailService;
            _hostingEnvironment = webHostEnvironment;
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

        [Authorize(Roles = "Admin")]
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _emailService.SendEmailAsync(email.ToEmail, email.Subject, email.Body);

            return Ok();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("{id}/upload-file", Name = "UploadFile")]
        public async Task<IActionResult> UploadFile(int id, IFormFile file)
        {
            _logger.LogInformation($"User tried to upload file for task with id: {id}");

            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok();
        }

    }
}