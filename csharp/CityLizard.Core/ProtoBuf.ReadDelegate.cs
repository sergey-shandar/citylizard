using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ProtoBuf
{
    public abstract class ReadDelegate
    {
        private readonly ILog Log;

        private void UnexpectedType<T>(T value)
        {
            Log.UnexpectedType<T>();
        }

        public virtual void Read(Object parent, ulong value)
        {
            UnexpectedType(value);
        }

        public virtual void Read(Object parent, double value)
        {
            UnexpectedType(value);
        }

        public virtual void Read(Object parent, byte[] value)
        {
            UnexpectedType(value);
        }

        public virtual void Read(Object parent, float value)
        {
            UnexpectedType(value);
        }

        protected ReadDelegate(ILog log)
        {
            Log = log;
        }
    }

    public interface IMessageRead
    {
        ILog Log { get; }
        ReadDelegate this[int i] { get; }
    }
}
