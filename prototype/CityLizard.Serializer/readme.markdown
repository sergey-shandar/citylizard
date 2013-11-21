# Serialization

Same as in memory structure.

## Text (read-only)

FIXED_SIZE_OBJECT_ID
FIELD_NAME FIXED_SIZE_VALUE
...

byte: 0..255: size 3.

class A
{
  public byte B = 3;
  public byte C = 17;
  public byte D = 127;
  public string E = "XXX";
}

ABCD 4567 8907 3456 8906 4567 5671 BEDF ABCD 4567 8907 3456 8906 4567 5671 BEDF
B 03
C 07
D FF
E ABCD 4567 8907 3456 8906 4567 5671 BEDF ABCD 4567 8907 3456 8906 4567 5671 BED0 

ABCD 4567 8907 3456 8906 4567 5671 BEDF ABCD 4567 8907 3456 8906 4567 5671 BED0
XXX

## Streaming (simplified but with support of cycles)

1. create an array of objects:

object[]
{
    buffer: new byte[size],
    buffer: new byte[size2],
}

dictionary<object, int>
{
}

## Protection

## ProtoBuf (communication protocol)

the format is the same as in protobuf except messages as references.

main message is object repeat; => array of objects:

all other messages references on this array.

value types and strings have the same encoding as in ProtoBuf.

length-delimeter|0, length0, object0,
length-delimeter|0, length1, object1,
length-delimeter|0, length2, object2,
...

0 - means null.

array encoding:
  length-delimeter with one repeatable field.
  because an array is an object, for example an array of arrays.

    var x = new int[][];

