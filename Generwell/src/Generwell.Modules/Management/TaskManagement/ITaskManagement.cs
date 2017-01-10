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
        Task<TaskDetailsModel> GetTaskDetails(string taskId, string accessToken, string tokenType);
        Task<string> UpdateTaskDetails(string Content, string taskId, string accessToken, string tokenType);
        Task<List<TaskModel>> GetTasks(string accessToken, string tokenType);
        Task<List<DictionaryModel>> GetDictionaries(string accessToken, string tokenType);
        Task<List<ContactInformationModel>> GetContactInformation(string accessToken, string tokenType);
        Task<List<TaskModel>> GetTasksByAssetsId(string url, string Id, string accessToken, string tokenType);
    }
}
