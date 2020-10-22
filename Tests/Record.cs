using System;
using Store;

namespace Tests
{
    public class Record : IIdentifiable, IEquatable<Record>
    {
        public Record(int id, int balance)
        {
            Id = id;
            Balance = balance;
        }

        public int Balance { get; }

        public int Id { get; }

        public bool Equals(Record other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Balance == other.Balance && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Record) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Balance, Id);
        }
    }
}