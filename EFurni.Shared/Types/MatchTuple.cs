#pragma warning disable 8618

namespace EFurni.Shared.Types
{
    public class MatchTuple<T,T1>
    {
        public T Entity { get; set; }
        public T1 Association { get; set; }
    }
}