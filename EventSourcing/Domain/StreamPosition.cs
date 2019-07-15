using System;

namespace LightestNight.System.EventSourcing.Domain
{
    public class StreamPosition : IComparable<StreamPosition>
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

        public int CompareTo(StreamPosition other)
        {
            // This instance is greater = 1
            // This instance is equal = 0
            // This instance is smaller = -1
            if (other == null)
                return 1;

            return Commit == other.Commit
                ? Prepare.CompareTo(other.Prepare)
                : Commit.CompareTo(other.Commit);
        }

        public static bool operator >(StreamPosition operand1, StreamPosition operand2)
        {
            var result = operand2.CompareTo(operand1);
            return result == 1;
        }

        public static bool operator <(StreamPosition operand1, StreamPosition operand2)
        {
            return operand2.CompareTo(operand1) == -1;
        }

        public static bool operator >=(StreamPosition operand1, StreamPosition operand2)
        {
            return operand2.CompareTo(operand1) >= 0;
        }

        public static bool operator <=(StreamPosition operand1, StreamPosition operand2)
        {
            return operand2.CompareTo(operand1) <= 0;
        }

        public static bool operator ==(StreamPosition operand1, StreamPosition operand2)
        {
            var null1 = operand1 as object;
            var null2 = operand2 as object;
            if (null1 == null && null2 == null)
                return true;

            if (null1 == null || null2 == null)
                return false;

            return operand1.Equals(operand2);
        }

        public static bool operator !=(StreamPosition operand1, StreamPosition operand2)
        {
            return !(operand1 == operand2);
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if (!(obj is StreamPosition streamPosition))
                return false;

            return streamPosition.Commit == Commit && streamPosition.Prepare == Prepare;
        }

        private bool Equals(StreamPosition other)
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

        public override string ToString()
        {
            return $"{Commit}, {Prepare}";
        }
    }
}