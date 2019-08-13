namespace ReadModel
{
    public abstract class Entity<TId>
    {
        public TId Id { get; set; }

        public long Version { get; set; }
    }
}
