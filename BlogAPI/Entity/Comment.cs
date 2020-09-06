using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Entity
{
    public class Comment
    {
        public string Id { get; set; }

        public string PostId { get; set; }

        public string CommentContent { get; set; }

        public DateTime PublishDate { get; set; }

    }
}
