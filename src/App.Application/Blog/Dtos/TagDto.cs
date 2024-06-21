using System;
namespace App.Application.Blog.Dtos
{
    [Serializable]
    public class TagDto : EntityDto<string>
    {
        public string TagName { get; set; }

        public string BGColor { get; set; }
    }
}