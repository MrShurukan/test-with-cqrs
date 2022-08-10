using Application.Infrastructure.Context;

namespace Application.Features.ZoneFeatures;

#region Команда

public class CreateZoneCommand : IRequest<int>
{
    public string Name { get; init; }
    public double MaxCapacity { get; init; }
}

#endregion

#region Валидация

public class CreateZoneCommandValidator : AbstractValidator<CreateZoneCommand>
{
    public CreateZoneCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(1).WithMessage("Длина должна быть >= 1 и <= 20")
            .MaximumLength(20).WithMessage("Длина должна быть >= 1 и <= 20")
            .Must(s => s.EndsWith("_zone")).WithMessage("Имя зоны должно заканчиваться на '_zone'");
    }
}

#endregion

#region Обработчик

public class CreateZoneCommandHandler : IRequestHandler<CreateZoneCommand, int>
{
    private readonly ZoneWriteRepository _zoneWriteRepository;

    public CreateZoneCommandHandler(ZoneWriteRepository zoneWriteRepository)
    {
        _zoneWriteRepository = zoneWriteRepository;
    }

    public async Task<int> Handle(CreateZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = new Zone();
        request.Adapt(zone);
        return await _zoneWriteRepository.AddZoneAsync(zone);
    }
}

#endregion

#region Репозиторий

public partial class ZoneWriteRepository
{
    private readonly ApplicationContext _context;

    public ZoneWriteRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<int> AddZoneAsync(Zone zone)
    {
        await _context.Zones.AddAsync(zone);
        await _context.SaveChangesAsync();

        return zone.Id;
    }
}

#endregion