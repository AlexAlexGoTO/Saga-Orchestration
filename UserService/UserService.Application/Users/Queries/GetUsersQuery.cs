using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Dto;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.Users.Queries;

public class GetUsersQuery : IRequest<IList<UserDto>>
{

}

public class GetPatientsQueryHandler : IRequestHandler<GetUsersQuery, IList<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPatientsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _context.Users.ToListAsync();

            var dtos = _mapper.Map<List<UserDto>>(users);

            return dtos;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}