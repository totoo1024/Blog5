using System;
using System.Collections.Generic;

namespace App.Application.Blog.Dtos
{
    [Serializable]
    public class ArticleDto : EntityDto<string>
    {
        public string Title { get; set; }

        public sbyte CreativeType { get; set; }

        public string Source { get; set; }

        public string SourceLink { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public string Thumbnail { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsTop { get; set; }

        public int ReadTimes { get; set; }

        public int MsgTimes { get; set; }

        public string TagsId { get; set; }

        public List<TagDto> Tags { get; set; }
    }
}