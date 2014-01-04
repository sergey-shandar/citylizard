# Read

1. Object message, IMessageReader messageReader, Stream stream, int field

  fieldReader = messagReader.GetFieldReader(field)

2. Stream stream, IFieldReader fieldReader, Object message

  wireValue = stream.Read<T>();

3. IFieldReader fieldReader, Object message, T value

  fieldReader.Read(message, wiredValue)
  {
      var value = (FT)wiredValue;
      SetValue(message, value)
      {
              info.SetValue(message, value);
          or
              message.Add(value);
      }
  }