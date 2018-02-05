using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Linq
{
    [Serializable]
    public sealed class ParseException : Exception
    {
        // Fields
        private int position;

        // Methods
        public ParseException(string message, int position) : base(message)
        {
            this.position = position;
        }

        public override string ToString()
        {
            return string.Format("{0} (at index {1})", Message, position);
        }

        // Properties
        public int GetPosition()
        {
            return position;
        }
    }
}
