using MediatR;
using SagaOrchestrator.Contexts;

namespace SagaOrchestrator.Commands;

public class StoreProcessCommand : IRequest
{
    public string ProcessId { get; set; }
    public class StoreProcessCommandHandler : IRequestHandler<StoreProcessCommand>
    {
        private readonly RedisContext _redisContext;
        private readonly CamundaContext _camundaContext;

        public StoreProcessCommandHandler(RedisContext redisContext, CamundaContext camundaContext)
        {
            _redisContext = redisContext;
            _camundaContext = camundaContext;
        }
        public Task Handle(StoreProcessCommand request, CancellationToken cancellationToken)
        {
            var length = _redisContext.UpdateSet(Constants.SagaProcessesList, request.ProcessId);
            if (length == 1)
            {
                _camundaContext.ResumeSaga(request.ProcessId);
            }
            return Task.CompletedTask;
        }
    }
}