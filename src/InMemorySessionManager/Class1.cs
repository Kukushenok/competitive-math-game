
using CompetitiveBackend.Core.Objects.Riddles;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using CompetitiveBackend.Services.Exceptions;
namespace InMemorySessionManager
{

    internal class ConfigurationReaderConfig
    {
        private IConfiguration conf;
        private string _section;
        public ConfigurationReaderConfig(IConfiguration conf, string sectionName)
        {
            _section = sectionName;
            this.conf = conf;
        }
        private int TryGet(string name, int deflt)
        {
            return int.TryParse(conf.GetSection(_section)[name], out int result) ? result : deflt;
        }

        public int DefaultSessionDurationMinutes => TryGet("lifetime", 20);
        public int RefreshTimeMinutes => TryGet("refreshPeriod", 5);
    }

    internal class RiddleSessionManager : IRiddleSessionManager, IDisposable
    {
        private readonly ConcurrentDictionary<string, RiddleSession> _sessions;
        private readonly ConfigurationReaderConfig _options;
        private readonly Timer _cleanupTimer;
        private readonly ILogger<RiddleSessionManager> _logger;

        public RiddleSessionManager(
            IConfiguration config,
            ILogger<RiddleSessionManager> logger)
        {
            _sessions = new ConcurrentDictionary<string, RiddleSession>();
            _options = new ConfigurationReaderConfig(config, "Constraints:SessionConfig");
            _logger = logger;

            // Clean up expired sessions every 5 minutes
            _cleanupTimer = new Timer(CleanupExpiredSessions, null,
                TimeSpan.FromMinutes(_options.RefreshTimeMinutes), TimeSpan.FromMinutes(_options.RefreshTimeMinutes));
        }

        public Task<RiddleSession> CreateSession(RiddleGameInfo gameInfo)
        {
            var sessionId = Guid.NewGuid().ToString();
            var expiresIn = DateTime.UtcNow.AddMinutes(_options.DefaultSessionDurationMinutes);

            var session = new RiddleSession(sessionId, gameInfo, expiresIn);

            if (!_sessions.TryAdd(sessionId, session))
            {
                // This should be very rare with GUIDs, but handle it just in case
                throw new InvalidOperationException("Failed to create session. Please try again.");
            }

            _logger?.LogInformation("Created new session {SessionId} expiring at {ExpiresIn}",
                sessionId, expiresIn);

            return Task.FromResult(session);
        }

        public Task<RiddleSession> RetrieveSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException("Session ID cannot be null or empty", nameof(sessionId));
            }

            if (_sessions.TryGetValue(sessionId, out var session))
            {
                if (session.ExpiresIn > DateTime.UtcNow)
                {
                    return Task.FromResult(session);
                }
                else
                {
                    // Remove expired session and throw exception
                    _sessions.TryRemove(sessionId, out _);
                    throw new GameSessionInvalidException($"Session {sessionId} has expired");
                }
            }

            throw new GameSessionInvalidException($"Session {sessionId} not found");
        }
        public Task<bool> DeleteSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException("Session ID cannot be null or empty", nameof(sessionId));
            }

            var removed = _sessions.TryRemove(sessionId, out _);

            if (removed)
            {
                _logger?.LogInformation("Deleted session {SessionId}", sessionId);
            }
            else
            {
                _logger?.LogDebug("Attempted to delete non-existent session {SessionId}", sessionId);
            }

            return Task.FromResult(removed);
        }
        private void CleanupExpiredSessions(object? state)
        {
            try
            {
                var now = DateTime.UtcNow;
                var expiredSessions = _sessions.Where(kvp => kvp.Value.ExpiresIn <= now).ToList();

                foreach (var session in expiredSessions)
                {
                    if (_sessions.TryRemove(session.Key, out _))
                    {
                        _logger?.LogDebug("Cleaned up expired session {SessionId}", session.Key);
                    }
                }

                _logger?.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during session cleanup");
            }
        }

        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }
    }
    public static class GameSessionInstaller
    {
        public static IServiceCollection AddInMemorySessions(this IServiceCollection coll)
        {
            coll.AddSingleton<IRiddleSessionManager, RiddleSessionManager>();
            return coll;
        }
    }
}
