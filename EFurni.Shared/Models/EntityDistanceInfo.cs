#pragma warning disable 8618

namespace EFurni.Shared.Models
{
    public sealed class EntityDistanceInfo<T>
    {
        public T Entity { get; set; }
        public double DistanceFromSourceInMeter { get; set; }
    }
}