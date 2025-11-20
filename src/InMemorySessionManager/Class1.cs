using System.Collections.Concurrent;
using CompetitiveBackend.Core.Objects.Riddles;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace InMemorySessionManager
{
    internal sealed class ConfigurationReaderConfig
    {
        private readonly IConfiguration conf;
        private readonly string section;
        public ConfigurationReaderConfig(IConfiguration conf, string sectionName)
        {
            section = sectionName;
            this.conf = conf;
        }

        private int TryGet(string name, int deflt)
        {
            return int.TryParse(conf.GetSection(section)[name], out int result) ? result : deflt;
        }

        public int DefaultSessionDurationMinutes => TryGet("lifetime", 20);
        public int RefreshTimeMinutes => TryGet("refreshPeriod", 5);
    }

    internal sealed class RiddleSessionManager : IRiddleSessionManager, IDisposable
    {
        private readonly ConcurrentDictionary<string, RiddleSession> sessions;
        private readonly ConfigurationReaderConfig options;
        private readonly Timer cleanupTimer;
        private readonly ILogger<RiddleSessionManager> logger;

        public RiddleSessionManager(
            IConfiguration config,
            ILogger<RiddleSessionManager> logger)
        {
            sessions = new ConcurrentDictionary<string, RiddleSession>();
            options = new ConfigurationReaderConfig(config, "Constraints:SessionConfig");
            this.logger = logger;

            // Clean up expired sessions every 5 minutes
            cleanupTimer = new Timer(
                CleanupExpiredSessions,
                null,
                TimeSpan.FromMinutes(options.RefreshTimeMinutes),
                TimeSpan.FromMinutes(options.RefreshTimeMinutes));
        }

        public Task<RiddleSession> CreateSession(RiddleGameInfo gameInfo)
        {
            string sessionId = Guid.NewGuid().ToString();
            DateTime expiresIn = DateTime.UtcNow.AddMinutes(options.DefaultSessionDurationMinutes);

            var session = new RiddleSession(sessionId, gameInfo, expiresIn);

            if (!sessions.TryAdd(sessionId, session))
            {
                // This should be very rare with GUIDs, but handle it just in case
                throw new InvalidOperationException("Failed to create session. Please try again.");
            }

            logger?.LogInformation(
                "Created new session {SessionId} expiring at {ExpiresIn}",
                sessionId,
                expiresIn);

            return Task.FromResult(session);
        }

        public Task<RiddleSession> RetrieveSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException("Session ID cannot be null or empty", nameof(sessionId));
            }

            if (sessions.TryGetValue(sessionId, out RiddleSession? session))
            {
                if (session.ExpiresIn > DateTime.UtcNow)
                {
                    return Task.FromResult(session);
                }
                else
                {
                    // Remove expired session and throw exception
                    sessions.TryRemove(sessionId, out _);
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

            bool removed = sessions.TryRemove(sessionId, out _);

            if (removed)
            {
                logger?.LogInformation("Deleted session {SessionId}", sessionId);
            }
            else
            {
                logger?.LogDebug("Attempted to delete non-existent session {SessionId}", sessionId);
            }

            return Task.FromResult(removed);
        }

        private void CleanupExpiredSessions(object? state)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                var expiredSessions = sessions.Where(kvp => kvp.Value.ExpiresIn <= now).ToList();

                foreach (KeyValuePair<string, RiddleSession> session in expiredSessions)
                {
                    if (sessions.TryRemove(session.Key, out _))
                    {
                        logger?.LogDebug("Cleaned up expired session {SessionId}", session.Key);
                    }
                }

                logger?.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error during session cleanup");
            }
        }

        public void Dispose()
        {
            cleanupTimer?.Dispose();
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
