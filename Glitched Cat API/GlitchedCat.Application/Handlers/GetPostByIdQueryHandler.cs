using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GlitchedCat.Application.Queries.Blog;
using GlitchedCat.Domain.Common.Models.Blog;
using GlitchedCat.Domain.Entities;
using GlitchedCat.Domain.Exceptions;
using GlitchedCat.Infra.Data;
using MediatR;

namespace GlitchedCat.Application.Handlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostResponse>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IRepository<Post> postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<PostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);
            if (post == null)
            {
                throw new PostNotFoundException(request.Id);
            }

            var postResponse = _mapper.Map<PostResponse>(post);
            return postResponse;
        }
    }
}