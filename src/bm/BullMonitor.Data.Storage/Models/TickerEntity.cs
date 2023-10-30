using BullMonitor.Data.Storage.Interfaces;
using System.Text.Json.Serialization;

namespace BullMonitor.Data.Storage.Models
{
    public record TickerEntity(
            Guid Id,
            string Code,
            string Name,
            Guid IndustryId,
            Guid ExchangeId,
            Guid CurrencyId)
        : IIdCode
    {
        private bool? _knownByZacks = null;
        private bool? _knownByTipRanks = null;

        public TickerEntity(
            string Code,
            string Name,
            Guid IndustryId,
            Guid ExchangeId,
            Guid CurrencyId)
            : this(
                Code,
                Name,
                IndustryId,
                ExchangeId,
                CurrencyId,
                null,
                null)
        { }

        [JsonConstructor]
        public TickerEntity(
            string Code,
            string Name,
            Guid IndustryId,
            Guid ExchangeId,
            Guid CurrencyId,
            bool? KnownByZacks,
            bool? KnownByTipRanks)
            : this(
                Guid.Empty,
                Code,
                Name,
                IndustryId,
                ExchangeId,
                CurrencyId)
        {
            _knownByZacks = KnownByZacks;
            _knownByTipRanks = KnownByTipRanks;
        }

        public bool? KnownByZacks
        {
            get => _knownByZacks;
            protected set => _knownByZacks = value;
        }

        public bool? KnownByTipRanks
        {
            get => _knownByTipRanks;
            protected set => _knownByTipRanks = value;
        }

        public bool SetKnownByZacks(bool value)
        {
            if (_knownByZacks == value)
            {
                return false;
            }

            _knownByZacks = value;

            return true;
        }

        public bool SetKnownByTipRanks(bool value)
        {
            if (_knownByTipRanks == value)
            {
                return false;
            }

            _knownByTipRanks = value;

            return true;
        }

        public virtual IndustryEntity Industry { get; set; }
        public virtual ExchangeEntity Exchange { get; set; }
        public virtual CurrencyEntity Currency { get; set; }
    }
}