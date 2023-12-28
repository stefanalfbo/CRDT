namespace CRDT.Register
{
    public class LWWBool : IEquatable<LWWBool>
    {
        public bool Value { get; private set; }
        public long Timestamp { get; private set; }
        
        public LWWBool(bool initialValue)
        {
            Value = initialValue;
            Timestamp = DateTime.UtcNow.Ticks;
        }

        private LWWBool(bool value, long timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }

        public void SetValue(bool newValue)
        {
            Value = newValue;
            Timestamp = DateTime.UtcNow.Ticks;
        }

        public override bool Equals(object? obj) => Equals(obj as LWWBool);

        public bool Equals(LWWBool? b)
        {
            if (b is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, b))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (GetType() != b.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (Value == b.Value) && (Timestamp == b.Timestamp);
        }

        public override int GetHashCode() => (Value, Timestamp).GetHashCode();

        public static bool operator ==(LWWBool lhs, LWWBool rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(LWWBool lhs, LWWBool rhs) => !(lhs == rhs);

        public static LWWBool operator ^(LWWBool a, LWWBool b)
        {
            if (a.Timestamp > b.Timestamp) return new LWWBool(a.Value, a.Timestamp);
            
            return new LWWBool(b.Value, b.Timestamp);
        }
    }
}
