using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE.Tests.Factories
{
    public static class GuidFactory
    {
        internal const string EmptyGuid = "00000000-0000-0000-0000-000000000000";
        internal const string FullGuid = "99999999-9999-9999-9999-999999999999";

        public static Guid Create()
        {
            return Create(1);
        }

        public static Guid Create(int index)
        {
            var value = index.ToString();
            return new Guid(string.Concat(EmptyGuid.AsSpan(0, EmptyGuid.Length - value.Length), value));
        }

        public static Guid Create(long index)
        {
            var value = index.ToString();
            return new Guid(string.Concat(EmptyGuid.AsSpan(0, EmptyGuid.Length - value.Length), value));
        }

        public static Guid Create(long prefix, long index)
        {
            var value = index.ToString();
            var stringPrefix = prefix.ToString();
            var stringValue = string.Concat(EmptyGuid.AsSpan(0, EmptyGuid.Length - value.Length), value);
            stringValue = string.Concat(stringPrefix, stringValue.AsSpan(stringPrefix.Length));
            return new Guid(stringValue);
        }

        public static IEnumerable<Guid> Create(IEnumerable<int> index)
        {
            return index.Select(x => Create(x));
        }
    }
}