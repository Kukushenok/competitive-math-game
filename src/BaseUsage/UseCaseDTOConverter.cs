using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage
{
    static class UseCaseDTOConverter
    {
        public static PlayerProfileDTO Convert(this PlayerProfile p)
        {
            return new PlayerProfileDTO(p.Name, p.Description, p.Id);
        } 
        public static PlayerProfile Convert(this PlayerProfileDTO dto)
        {
            return new PlayerProfile(dto.Name ?? string.Empty, dto.Description, dto.ID);
        }
        public static LargeData Convert(this LargeDataDTO dto)
        {
            return new LargeData(dto.Data);
        }
        public static LargeDataDTO Convert(this LargeData data)
        {
            return new LargeDataDTO(data.Data ?? Array.Empty<byte>());
        }
        public static CompetitionDTO Convert(this Competition comp)
        {
            return new CompetitionDTO(comp.Id, comp.Name, comp.Description, comp.StartDate, comp.EndDate);
        }
        public static Competition Convert(this CompetitionDTO comp)
        {
            return new Competition(comp.Name ?? string.Empty, comp.Description ?? string.Empty, comp.StartDate, comp.EndDate, comp.ID);
        }
        public static DataLimiter Convert(this DataLimiterDTO limiter)
        {
            return new DataLimiter(limiter.Page, limiter.Count);
        }
        public static IEnumerable<T> Convert<T, C>(IEnumerable<C> convertable, Func<C, T> converter)
        {
            foreach (C q in convertable) yield return converter(q);
        }
    }
}
