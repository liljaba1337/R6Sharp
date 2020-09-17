using R6Sharp.Response.Statistic;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
{
    public class StatisticEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public StatisticEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        private async Task<Dictionary<string, Dictionary<string, double>>> GetStatisticsAsync(Guid[] uuids, Platform platform, StatisticsType type)
        {
            var variables = Constant.GetVariables(type);
            var results = await FetchStatisticsAsync(uuids, platform, variables).ConfigureAwait(false);
            var deserialized = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, double>>>>(results);
            return deserialized["results"];
        }

        private async Task<Dictionary<string, double>> GetStatisticsAsync(Guid uuid, Platform platform, StatisticsType type)
        {
            var result = await GetStatisticsAsync(new[] { uuid }, platform, type).ConfigureAwait(false);
            return result[uuid.ToString()];
        }

        public async Task<Dictionary<string, EquipmentStatistic>> GetEquipmentStatistics(Guid[] uuids, Platform platform)
        {
            var result = await FetchStatisticsAsync(uuids, platform, Constant.GetVariables(StatisticsType.Equipments)).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<EquipmentStatisticsFetch>(result);
            return deserialised.EquipmentStatistics;
        }

        public async Task<EquipmentStatistic> GetEquipmentStatistics(Guid uuid, Platform platform)
        {
            var result = await GetEquipmentStatistics(new Guid[] { uuid }, platform).ConfigureAwait(false);
            return result[uuid.ToString()];
        }

        public async Task<Dictionary<string, GamemodeStatistic>> GetGamemodeStatistics(Guid[] uuids, Platform platform)
        {
            var result = await FetchStatisticsAsync(uuids, platform, Constant.GetVariables(StatisticsType.Gamemodes)).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<GamemodeStatisticFetch>(result);
            return deserialised.GamemodeStatistics;
        }

        public async Task<GamemodeStatistic> GetGamemodeStatistics(Guid uuid, Platform platform)
        {
            var result = await GetGamemodeStatistics(new Guid[] { uuid }, platform).ConfigureAwait(false);
            return result[uuid.ToString()];
        }

        public async Task<Dictionary<string, OperatorStatistic>> GetOperatorStatistics(Guid[] uuids, Platform platform)
        {
            var result = await FetchStatisticsAsync(uuids, platform, Constant.GetVariables(StatisticsType.Operators)).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<OperatorStatisticFetch>(result);
            return deserialised.OperatorStatistics;
        }

        public async Task<OperatorStatistic> GetOperatorStatistics(Guid uuid, Platform platform)
        {
            var result = await GetOperatorStatistics(new Guid[] { uuid }, platform).ConfigureAwait(false);
            return result[uuid.ToString()];
        }

        public async Task<Dictionary<string, QueueStatistic>> GetQueueStatistics(Guid[] uuids, Platform platform)
        {
            var result = await FetchStatisticsAsync(uuids, platform, Constant.GetVariables(StatisticsType.Queues)).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<QueueStatisticFetch>(result);
            return deserialised.QueueStatistics;
        }

        public async Task<QueueStatistic> GetQueueStatistics(Guid uuid, Platform platform)
        {
            var result = await GetQueueStatistics(new Guid[] { uuid }, platform).ConfigureAwait(false);
            return result[uuid.ToString()];
        }

        public async Task<Dictionary<string, TerroristHuntMissionStatistic>> GetTerroristHuntMissionsStatistics(Guid[] uuids, Platform platform)
        {
            var result = await FetchStatisticsAsync(uuids, platform, Constant.GetVariables(StatisticsType.Equipments)).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<TerroristHuntMissionStatisticFetch>(result);
            return deserialised.TerroristHuntMissionStatistics;
        }

        public async Task<TerroristHuntMissionStatistic> GetTerroristHuntMissionsStatistics(Guid uuid, Platform platform)
        {
            var result = await GetTerroristHuntMissionsStatistics(new Guid[] { uuid }, platform).ConfigureAwait(false);
            return result[uuid.ToString()];
        }

        private async Task<string> FetchStatisticsAsync(Guid[] uuids, Platform platform, params string[] variables)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("populations", string.Join(',', uuids)),
                new KeyValuePair<string, string>("statistics", string.Join(',', variables))
            };

            var ticket = await _sessionHandler.GetTicketAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(Endpoints.Statistics, platform, queries, ticket).ConfigureAwait(false);
            return results;
        }
    }
}
