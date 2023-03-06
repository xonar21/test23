using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GR.CloudStorage.Abstractions.Models
{
    public class MoveFolderContentFolderRequestModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        public string FolderName { get; set; }

        public int ProjectNumber { get; set; }

        public string NewPath { get; set; }

        public string OldPath { get; set; }

        private MoveFolderContentFolderRequestModel()
        {
            // Left empty for special purpose
        }

        public MoveFolderContentFolderRequestModel([Required] string oldPath, [Required] string newPath,
            [Required] string folderName, [Required] int projectNumber)
        {
            OldPath = oldPath;
            NewPath = newPath;
            FolderName = folderName;
            ProjectNumber = projectNumber;
        }
    }
}
