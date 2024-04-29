namespace EnglishVocab.Woker.Workflow
{
    public class ScheduleHostService : IHostedService, IDisposable
    {
        private readonly ILogger<ScheduleHostService> _logger;
        private Timer? _timer = null;

        public ScheduleHostService(ILogger<ScheduleHostService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"ScheduleHostService running at: {DateTime.Now}");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
