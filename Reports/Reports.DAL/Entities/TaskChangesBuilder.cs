using System;

namespace Reports.DAL.Entities
{
    public class TaskChangesBuilder
    {
        private TaskChanges _taskChanges;

        public TaskChangesBuilder()
        {
            _taskChanges = new TaskChanges();
        }


        public TaskChangesBuilder BuildComment(string taskChangesComment)
        {
            _taskChanges.Comment = taskChangesComment;
            return this;
        }

        public TaskChangesBuilder BuildNewEmployee(Guid id)
        {
            _taskChanges.EmployeeId = id;
            return this;
        }

        public TaskChangesBuilder BuildNewTaskState(TaskState state)
        {
            _taskChanges.State = state;
            return this;
        }

        public TaskChanges GetResult()
        {
            return _taskChanges;
        }
    }
}