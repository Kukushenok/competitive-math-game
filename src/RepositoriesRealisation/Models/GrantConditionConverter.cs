using CompetitiveBackend.Core.RewardCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoriesRealisation.Models
{
    public static class GrantConditionConverter
    {
        public static bool FromJSON(JsonDocument document, out GrantCondition cond)
        {
            cond = null!;
            string val = document.RootElement.HasProperty(nameof(cond.Type))?.StringValue()!;
            switch (val)
            {
                case "rank":
                    float minRank = (float?)document.RootElement.HasProperty(nameof(RankGrantCondition.minRank))?.DoubleValue() ?? 0.0f;
                    float maxRank = (float?)document.RootElement.HasProperty(nameof(RankGrantCondition.maxRank))?.DoubleValue() ?? 1.0f;
                    cond = new RankGrantCondition(minRank, maxRank);
                    break;
                case "place":
                    int? minPlace = document.RootElement.HasProperty(nameof(PlaceGrantCondition.minPlace))?.IntValue();
                    int? maxPlace = document.RootElement.HasProperty(nameof(PlaceGrantCondition.maxPlace))?.IntValue();
                    if (minPlace != null && maxPlace != null)
                    {
                        cond = new PlaceGrantCondition(minPlace!.Value, maxPlace!.Value);
                    }
                    break;
            }
            return cond != null;
        }
        public static bool ToJSON(GrantCondition cond, out JsonDocument document)
        {
            JsonObject obj = new JsonObject
            {
                [nameof(cond.Type)] = cond.Type
            };
            document = null!;
            if (cond is PlaceGrantCondition place)
            {
                obj.Add(new KeyValuePair<string, JsonNode?>(nameof(place.maxPlace), (JsonNode)place.maxPlace));
                obj.Add(new KeyValuePair<string, JsonNode?>(nameof(place.minPlace), (JsonNode)place.minPlace));
            }
            else if (cond is RankGrantCondition rank)
            {
                obj.Add(new KeyValuePair<string, JsonNode?>(nameof(rank.maxRank), (JsonNode)rank.minRank));
                obj.Add(new KeyValuePair<string, JsonNode?>(nameof(rank.minRank), (JsonNode)rank.maxRank));
            }
            else
            {
                return false;
            }
            document = JsonDocument.Parse(obj.ToJsonString());
            return true;
        }
        public static GrantCondition? FromJSON(JsonDocument document)
        {
            return FromJSON(document, out GrantCondition cond) ? cond : null;
        }
        public static JsonDocument? ToJSON(GrantCondition cond)
        {
            return ToJSON(cond, out JsonDocument doc) ? doc : null;
        }
        private static JsonElement? HasProperty(this JsonElement elem, string key)
        {
            return elem.TryGetProperty(key, out JsonElement value) ? value : null;
        }
        private static string? StringValue(this JsonElement elem) => elem.GetString();
        private static double? DoubleValue(this JsonElement elem)
        {
            return elem.TryGetDouble(out double value) ? value : null;
        }
        private static int? IntValue(this JsonElement elem)
        {
            return elem.TryGetInt32(out int value) ? value : null;
        }
        public static string Save(this JsonDocument doc)
        {
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
            {
                doc.WriteTo(writer);
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
