using MediatR;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Handlers.MediatR;

public sealed class DeleteBlogPostHandler(IBlogPostService service)
    : IRequestHandler<DeleteBlogPostRequest, DeleteBlogPostResponse>
{
    public async Task<DeleteBlogPostResponse> Handle(DeleteBlogPostRequest request, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);

        return new DeleteBlogPostResponse
        {
            Id = request.Id,
            Deleted = deleted
        };
    }
}
