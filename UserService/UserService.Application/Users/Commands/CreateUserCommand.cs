using AutoMapper;
using MediatR;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands;

public class CreateUserCommand : IRequest<int>
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<User>(request);
           
            entity.Id = Guid.NewGuid();

            _context.Users.Add(entity);

            var result = await _context.SaveChangesAsync(cancellationToken);

            //publish domain event 

            return result;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}
