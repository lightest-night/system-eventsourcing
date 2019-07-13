namespace LightestNight.System.EventSourcing.Domain
{
    public class StreamPosition
    {
        /// <summary>
        /// Commit position of the Event
        /// </summary>
        public long Commit { get; }
        
        /// <summary>
        /// Prepare position of the Event
        /// </summary>
        public long Prepare { get; }

        public StreamPosition(long commit, long prepare)
        {
            Commit = commit;
            Prepare = prepare;
        }

        public bool IsGreater(long commit, long prepare)
        {
            if (commit > Commit)
                return true;

            if (commit == Commit)
                return prepare > Prepare;

            return false;
        }

        public bool IsLower(long commit, long prepare)
        {
            if (commit < Commit)
                return true;

            if (commit == Commit)
                return prepare < Prepare;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StreamPosition streamPosition))
                return false;

            return streamPosition.Commit == Commit && streamPosition.Prepare == Prepare;
        }

        protected bool Equals(StreamPosition other)
        {
            return Commit == other.Commit && Prepare == other.Prepare;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Commit.GetHashCode() * 397) ^ Prepare.GetHashCode();
            }
        }
    }
}