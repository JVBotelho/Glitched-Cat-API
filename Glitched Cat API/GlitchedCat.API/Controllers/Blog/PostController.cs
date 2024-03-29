using System;
using System.Threading.Tasks;
using AutoMapper;
using GlitchedCat.Application.Commands;
using GlitchedCat.Application.Queries.Blog;
using GlitchedCat.Domain.Common.Models.Blog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlitchedCat.API.Controllers.Blog
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PostController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var query = new GetAllPostsQuery();
            var result = await _mediator.Send(query);

            return Ok(_mapper.Map<PostResponse[]>(result));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var query = new GetPostByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostResponse>(result));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreatePost([FromBody] PostRequest postRequest)
        {
            var command = _mapper.Map<CreatePostCommand>(postRequest);
            var postId = await _mediator.Send(command);

            if (postId == Guid.Empty)
            {
                return BadRequest();
            }

            return CreatedAtRoute(nameof(GetPostById), new { id = postId }, postId);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostRequest postRequest)
        {
            var command = _mapper.Map<UpdatePostCommand>(postRequest);
            command.Id = id;

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var command = new DeletePostCommand { Id = Guid.Parse(id) };
            await _mediator.Send(command);

            return Accepted();
        }
    }
}
