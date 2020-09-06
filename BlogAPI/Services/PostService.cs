using BlogAPI.DatabaseContext;
using BlogAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Services
{

    public interface IPostService
    {
        Post CreatePost(Post post);
        List<Post> GetAllPosts();
        Post GetPost(String id);
        dynamic UpdatePost(String id, Post updatedPost);
        Post DeletePost(String id);
        Comment AddComment(String blogId, Comment comment);
    }

    // Service class handles business logic related to CRUD operations of Blog Post
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Creates new blog post
        public Post CreatePost(Post post)
        {
            // Check if 'Title' or 'Content' is empty
            if (!IsValidPost(post))
                return null;

            // Adds new post and commit to DB
            post.Id = GeneratePostId(post);
            _dataContext.Posts.Add(post);
            _dataContext.SaveChanges();
            
            return post;
        }

        // Get all posts
        public List<Post> GetAllPosts()
        {
            return _dataContext.Posts.ToList();
        }

        // Get a post using the id
        public Post GetPost(string id)
        {
            var post = _dataContext.Posts.Where(s => s.Id == id).SingleOrDefault();

            if (post == null)
                return null;

            // Get all comments
            post.Comments = _dataContext.Comments.Where(c => c.PostId == id).ToList<Comment>();

            return post;
        }

        // Updates a post. (Returns null / string error message / updated post)
        public dynamic UpdatePost(string id, Post updatedPost)
        {
            // Invalid update
            if (!IsValidPost(updatedPost))
                return "Title or Content cannot be empty.";

            // Check if post exists
            var post = _dataContext.Posts.Where(s => s.Id == id).SingleOrDefault();

            if (post == null)
                // blog post doesn't exist
                return null;
        
            // Update the post
            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            post.ImageUrl = updatedPost.ImageUrl;

            // Commit update to DB
            _dataContext.Posts.Update(post);
            _dataContext.SaveChanges();

            return post;
        }

        // Delete post with Id of [id]
        public Post DeletePost(string id)
        {
            // Check if post exists
            var post = _dataContext.Posts.Where(s => s.Id == id).SingleOrDefault();

            if (post == null)
                return null;

            // Delete the post (and commit changes to db)
            _dataContext.Posts.Remove(post);
            _dataContext.SaveChanges();

            return post;
        }

        // Add comment to blog post
        public Comment AddComment(string blogId, Comment comment)
        {
            // Get the blog post
            var post = _dataContext.Posts.Where(s => s.Id == blogId).SingleOrDefault();

            if (post == null)
                // blog post doesn't exist
                return null;

            // Generate unique comment id
            comment.Id = GenerateId();
            // Comment publish date-time
            comment.PublishDate = DateTime.Now;

            // Add comment to blog post
            post.Comments.Add(comment);

            // Commit changes to db
            _dataContext.Posts.Update(post);
            _dataContext.SaveChanges();

            return comment;
        }



        // Generates unique ID string
        private string GenerateId()
        {
            // Generate unique identifier
            Guid blogId = Guid.NewGuid();
            // Remove characters which may cause problems in urls
            var idString = Convert.ToBase64String(blogId.ToByteArray());
            idString = idString.Replace("+", "");
            idString = idString.Replace("/", "");
            idString = idString.Replace("=", "");
            idString = idString.Replace(" ", "");

            return idString;

        }

        // Generate unique url containing blog post title (also the id (pk) in ms_sql server db)
        private string GeneratePostId(Post post)
        {
            return post.Title.Replace(" ", "") + "_" + GenerateId();
        }

        // Checks if a blog post contains empty title or content
        private bool IsValidPost(Post post)
        {
            if (string.IsNullOrEmpty(post.Title) || string.IsNullOrEmpty(post.Content))
                return false;
            return true;
        }

    }

}
