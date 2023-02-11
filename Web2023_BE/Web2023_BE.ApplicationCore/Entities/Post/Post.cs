using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel;
using Nest;

namespace Web2023_BE.ApplicationCore.Entities
{
    /// <summary>
    /// Thực thể bài viết
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(PostID)), Description("posts")]
    public class Post : BaseEntity
    {

        //public Post(string id)
        //{
        //    PostID = Guid.Parse(id);
        //}

        #region Property
        /// <summary>
        /// Id bài viết
        /// </summary>
        [Key]
        public Guid PostID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Id bài viết
        /// </summary>
        public Guid MenuID { get; set; } = Guid.Empty;
        

        public Guid AccountID { get; set; }

        /// <summary>
        /// Mã bài viết
        /// </summary>
        [IDuplicate]
        [IRequired]
        [Display(Name = "Tiêu đề bài viết")]
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Title { get; set; }

        /// <summary>
        /// Họ và tên bài viết
        /// </summary>
        [Display(Name = "Alias bài viết")]
        [IDuplicate]
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Slug { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [Display(Name = "Mô tả của bài viết")]
        [Text(Index = true, Fielddata = true, Analyzer = "casesensitive_text")]
        public string Description { get; set; }

        /// <summary>
        /// Nội dung html
        /// </summary>
        [Display(Name = "Nội dung html")]
        public string Content { get; set; }

        /// <summary>
        /// Ảnh xem trước
        /// </summary>
        [Display(Name = "Ảnh xem trước")]
        public string Image { get; set; }

        /// <summary>
        /// Lượt xem
        /// </summary>
        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }

        /// <summary>
        /// Loại bài viết
        /// </summary>
        [Display(Name = "Loại bài viết")]
        public int Type { get; set; }
        #endregion
    }
}
