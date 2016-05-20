using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Utils
{
    public static Int32 GetUnixTime()
    {
        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        return unixTimestamp;
    }

    public enum Direction
    {
        RIGHT,
        LEFT,
        BOTH,
        NONE
    }

    public enum ObjectType
    {
        PLAYER,
        ENEMY,
        STATIC
    }
}

