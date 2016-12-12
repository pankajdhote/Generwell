using Generwell.Core.Model;
using Generwell.Modules.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Management
{
    public interface ITaskManagement
    {
        Task<TaskDetailsViewModel> GetTaskDetails(string taskId, string accessToken, string tokenType);
        Task<string> UpdateTaskDetails(string Content, string taskId, string accessToken, string tokenType);
        Task<List<TaskViewModel>> GetTasks(string accessToken, string tokenType);
        Task<List<TaskViewModel>> GetTasksByWellId(string wellId, string accessToken, string tokenType);
    }
}
