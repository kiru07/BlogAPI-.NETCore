using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.DatabaseContext;
using BlogAPI.Entity;
using BlogAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        // Instances obtained using DI
        private readonly DataContext _dataContext;
        private readonly IPostService _postService;

        // Using Dependency Injection (DI) to obtain instance of DataContext service for db operations and PostService for CRUD operations
        public PostController(DataContext dataContext, IPostService postService)
        {
            _dataContext = dataContext;
            _postService = postService;
        }

        // Get All Blog Posts
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var allPosts = _postService.GetAllPosts();
            return Ok(allPosts);
        }

        // Get Blog Post Using Id (Also returns all comments on blog post)
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetPost(string id)
        {

            var post = _postService.GetPost(id);

            if (post == null)
                return NotFound($"No post found for Id {id}");

            return Ok(post);
        }

        // Add New Blog Post
        [HttpPost]
        public IActionResult CreatePost([FromBody]Post newPost)
        {
            var createdPost = _postService.CreatePost(newPost);

            if (createdPost == null)
                return BadRequest("Title or Content cannot be empty");
            return Ok(createdPost);
        }

        // Update Blog Post
        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdatePost(string id, [FromBody]Post updatedPost)
        {
            var result = _postService.UpdatePost(id, updatedPost);

            if (result == null)
                return NotFound($"No post found for id {id}");
            else if (result is string)
                return BadRequest(result);

            return Ok(result);
        }

        // Delete Blog Post
        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeletePost(string id)
        {

            var post = _postService.DeletePost(id);

            if (post == null)
                return NotFound($"No post found with id {id}");
            
            return Ok(post);
        }

        // Add comment
        [Route("{blogId}/comment")]
        [HttpPost]
        public IActionResult AddComment(string blogId, [FromBody]Comment comment)
        {
            var newComment = _postService.AddComment(blogId, comment);

            if (newComment == null)
                return NotFound($"No post found with id {blogId}");

            return Ok(comment);
        }
    }
}