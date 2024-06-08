
namespace Task_Manager.DataAccess
{
    public class TaskRepository : ITaskRepository
    {
        private List<Models.Task> _tasks;

        public TaskRepository()
        {
            _tasks = new List<Models.Task>() 
                {
                    new Models.Task() {Id= 1, Title="First", Description= "FirstDescription", IsDone=false},
                    new Models.Task() {Id= 2, Title="Second", Description= "SecondDescription", IsDone= true}
                };
        }

        public List<Models.Task> GetTasks()
        {
            return _tasks;
        }

        public Models.Task? GetTask(int id)
        {
            return _tasks.FirstOrDefault(i => i.Id == id);
        }

        public void DeleteTask(int id)
        {
            Models.Task? task = _tasks.FirstOrDefault(i => i.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }

        public void UpdateTask(int id, Models.Task updateTask)
        {
            Models.Task? task = _tasks.FirstOrDefault(i => i.Id == id);
            if (task != null)
            {
                task.Title = updateTask.Title;
                task.Description = updateTask.Description;
                task.IsDone = updateTask.IsDone;
                task.UpdatedAt = DateTime.Now;
            }
        }

        public void CreateTask(Models.Task task)
        {
            _tasks.Add(task);
            task.CreatedAt = DateTime.Now;
        }
    }
}
