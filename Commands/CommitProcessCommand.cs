using MediatR;
using SagaOrchestrator.Contexts;

namespace SagaOrchestrator.Commands;

public class CommitProcessCommand : IRequest
{
    public string ProcessId { get; set; }
    public class CommitProcessCommandHandler : IRequestHandler<CommitProcessCommand>
    {
        private readonly RedisContext _redisContext;
        private readonly ILogger<CommitProcessCommand> _logger;
        private readonly CamundaContext _camundaContext;

        public CommitProcessCommandHandler(RedisContext redisContext, CamundaContext camundaContext, ILogger<CommitProcessCommand> logger)
        {
            _redisContext = redisContext;
            _logger = logger;
            _camundaContext = camundaContext;
        }
        public Task Handle(CommitProcessCommand request, CancellationToken cancellationToken)
        {
            _redisContext.Pop(Constants.SagaProcessesList, request.ProcessId);
            var nextId = _redisContext.First(Constants.SagaProcessesList);
            if (nextId != null)
            {
                _logger.LogInformation($"[CommitProcessCommand] - Resuming next process, {nextId}");
                _camundaContext.ResumeSaga(nextId);
            }
            return Task.CompletedTask;
        }
    }
}