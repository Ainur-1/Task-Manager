namespace Task_Manager.DataAccess
{
    public interface ITaskRepository
    {
        List<Models.Task> GetTasks();
        Models.Task? GetTask(int id);
        void DeleteTask(int id);
        void UpdateTask(int id, Models.Task updateTask);
        void CreateTask(Models.Task task);
    }
}