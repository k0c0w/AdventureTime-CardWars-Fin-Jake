using System.Text;


namespace Shared.Packets;

public class Packet : IDisposable
{
    private List<byte> buffer;
    private byte[] readableBuffer;
    private int readPos;

    public Packet()
    {
        buffer = new List<byte>();
        readPos = 0;
    }

    public Packet(int id)
    {
        buffer = new List<byte>();
        readPos = 0;

        Write(id);
    }
    
    public Packet(int id, int subId) : this(id) => Write(subId);

    public Packet(byte[] data)
    {
        buffer = new List<byte>();
        readPos = 0;

        SetBytes(data);
    }

    #region Functions
    /// <summary>Sets the packet's content and prepares it to be read.</summary>
    /// <param name="data">The bytes to add to the packet.</param>
    public void SetBytes(byte[] data)
    {
        Write(data);
        readableBuffer = buffer.ToArray();
    }

    /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
    public void WriteLength()
    {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count)); // Insert the byte length of the packet at the very beginning
    }

    public void InsertInt(int value)
    {
        buffer.InsertRange(0, BitConverter.GetBytes(value)); // Insert the int at the start of the buffer
    }

    /// <summary>Gets the packet's content in array form.</summary>
    public byte[] ToArray()
    {
        readableBuffer = buffer.ToArray();
        return readableBuffer;
    }

    /// <summary>Gets the length of the packet's content.</summary>
    public int Length => buffer.Count;

    /// <summary>Gets the length of the unread data contained in the packet.</summary>
    public int UnreadLength => Length - readPos;

    /// <summary>Resets the packet instance to allow it to be reused.</summary>
    /// <param name="shouldReset">Whether or not to reset the packet.</param>
    public void Reset(bool shouldReset = true)
    {
        if (shouldReset)
        {
            buffer.Clear(); // Clear buffer
            readableBuffer = null;
            readPos = 0; // Reset readPos
        }
        else
        {
            readPos -= 4; // "Unread" the last read int
        }
    }
    #endregion

    #region Write Data
    /// <summary>Adds a byte to the packet.</summary>
    /// <param name="value">The byte to add.</param>
    public void Write(byte value)
    {
        buffer.Add(value);
    }
    /// <summary>Adds an array of bytes to the packet.</summary>
    /// <param name="value">The byte array to add.</param>
    public void Write(byte[] value)
    {
        buffer.AddRange(value);
    }
    /// <summary>Adds a short to the packet.</summary>
    /// <param name="value">The short to add.</param>
    public void Write(short value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }
    /// <summary>Adds an int to the packet.</summary>
    /// <param name="value">The int to add.</param>
    public void Write(int value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }
    /// <summary>Adds a long to the packet.</summary>
    /// <param name="value">The long to add.</param>
    public void Write(long value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }
    /// <summary>Adds a float to the packet.</summary>
    /// <param name="value">The float to add.</param>
    public void Write(float value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }
    /// <summary>Adds a bool to the packet.</summary>
    /// <param name="value">The bool to add.</param>
    public void Write(bool value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }
    /// <summary>Adds a string to the packet.</summary>
    /// <param name="_value">The string to add.</param>
    public void Write(string value)
    {
        Write(value.Length); // Add the length of the string to the packet
        buffer.AddRange(Encoding.ASCII.GetBytes(value)); // Add the string itself
    }
    #endregion

    #region Read Data
    /// <summary>Reads a byte from the packet.</summary>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public byte ReadByte(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            var value = readableBuffer[readPos]; // Get the byte at readPos' position
            if (moveReadPos)
            {
                // If _moveReadPos is true
                readPos += 1; // Increase readPos by 1
            }
            return value; // Return the byte
        }
        else
        {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }

    /// <summary>Reads an array of bytes from the packet.</summary>
    /// <param name="length">The length of the byte array.</param>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public byte[] ReadBytes(int length, bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            var value = buffer.GetRange(readPos, length).ToArray(); // Get the bytes at readPos' position with a range of _length
            if (moveReadPos)
            {
                // If _moveReadPos is true
                readPos += length; // Increase readPos by _length
            }
            return value; // Return the bytes
        }
        else
        {
            throw new Exception("Could not read value of type 'byte[]'!");
        }
    }

    /// <summary>Reads a short from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public short ReadShort(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            var value = BitConverter.ToInt16(readableBuffer, readPos); // Convert the bytes to a short
            if (_moveReadPos)
            {
                // If _moveReadPos is true and there are unread bytes
                readPos += 2; // Increase readPos by 2
            }
            return value; // Return the short
        }
        else
        {
            throw new Exception("Could not read value of type 'short'!");
        }
    }

    /// <summary>Reads an int from the packet.</summary>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public int ReadInt(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            int value = BitConverter.ToInt32(readableBuffer, readPos); // Convert the bytes to an int
            if (moveReadPos)
            {
                // If _moveReadPos is true
                readPos += 4; // Increase readPos by 4
            }
            return value; // Return the int
        }
        else
        {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    /// <summary>Reads a long from the packet.</summary>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public long ReadLong(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            var value = BitConverter.ToInt64(readableBuffer, readPos); // Convert the bytes to a long
            if (moveReadPos)
            {
                // If _moveReadPos is true
                readPos += 8; // Increase readPos by 8
            }
            return value; // Return the long
        }
        else
        {
            throw new Exception("Could not read value of type 'long'!");
        }
    }

    /// <summary>Reads a float from the packet.</summary>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public float ReadFloat(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            var value = BitConverter.ToSingle(readableBuffer, readPos); // Convert the bytes to a float
            if (moveReadPos)
            {
                // If _moveReadPos is true
                readPos += 4; // Increase readPos by 4
            }
            return value; // Return the float
        }
        else
        {
            throw new Exception("Could not read value of type 'float'!");
        }
    }

    /// <summary>Reads a bool from the packet.</summary>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public bool ReadBool(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            var value = BitConverter.ToBoolean(readableBuffer, readPos); // Convert the bytes to a bool
            if (moveReadPos)
            {
                // If _moveReadPos is true
                readPos += 1; // Increase readPos by 1
            }
            return value; // Return the bool
        }
        else
        {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    /// <summary>Reads a string from the packet.</summary>
    /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
    public string ReadString(bool moveReadPos = true)
    {
        try
        {
            var length = ReadInt(); // Get the length of the string
            var value = Encoding.ASCII.GetString(readableBuffer, readPos, length); // Convert the bytes to a string
            if (moveReadPos && value.Length > 0)
            {
                // If _moveReadPos is true string is not empty
                readPos += length; // Increase readPos by the length of the string
            }
            return value; // Return the string
        }
        catch
        {
            throw new Exception("Could not read value of type 'string'!");
        }
    }
    #endregion

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
