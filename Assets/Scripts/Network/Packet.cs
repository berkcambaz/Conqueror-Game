using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Packet
{
    private List<byte> buffer;
    private byte[] readableBuffer;
    private int readPos;

    public Packet(int _id)
    {
        buffer = new List<byte>();
        Write(_id);
    }

    public Packet(byte[] _data)
    {
        readableBuffer = _data;
        readPos = 0;
    }

    public byte[] ToArray()
    {
        return buffer.ToArray();
    }

    public int Length()
    {
        return buffer.Count;
    }

    #region Write 
    public void Write(byte _value)
    {
        buffer.Add(_value);
    }

    public void Write(byte[] _value)
    {
        buffer.AddRange(_value);
    }

    public void Write(int _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }

    public void Write(bool _value)
    {
        buffer.AddRange(BitConverter.GetBytes(_value));
    }

    public void Write(string _value)
    {
        Write(_value.Length);
        buffer.AddRange(Encoding.ASCII.GetBytes(_value));
    }

    public void Write(Vector2Int _value)
    {
        Write(_value.x);
        Write(_value.y);
    }
    #endregion

    #region Read Data
    public byte ReadByte()
    {
        if (readPos + 1 < readableBuffer.Length)
        {
            byte value = readableBuffer[readPos];
            readPos += 1;
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }

    public byte[] ReadBytes(int _length)
    {
        if (readPos + _length < readableBuffer.Length)
        {
            byte[] value = buffer.GetRange(readPos, _length).ToArray();
            readPos += _length;
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'byte[]'!");
        }
    }

    public int ReadInt()
    {
        if (readPos + 4 < readableBuffer.Length)
        {
            int value = BitConverter.ToInt32(readableBuffer, readPos);
            readPos += 4;
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    public bool ReadBool()
    {
        if (readPos + 1 < readableBuffer.Length)
        {
            bool value = BitConverter.ToBoolean(readableBuffer, readPos);
            readPos += 1;
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    public string ReadString()
    {
        try
        {
            int length = ReadInt();
            string value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
            readPos += length;
            return value;
        }
        catch
        {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    public Vector2Int ReadVector2Int()
    {
        return new Vector2Int(ReadInt(), ReadInt());
    }
    #endregion
}