using System;
using System.Device.I2c;
using System.Threading;

class Program
{
    // I2C address (default for IS31FL3731/IS31FL3741, check your board)
    const int I2cAddress = 0x74;
    // Register addresses
    const byte PageSelect = 0xFD; // Function register page select
    const byte ConfigReg = 0x00;  // Configuration register
    const byte LedControlBase = 0x00; // LED control base (Page 0)

    // Matrix dimensions
    const int Width = 13;  // Columns
    const int Height = 9;  // Rows

    static void Main()
    {
        // Open I2C bus 1 (Raspberry Pi default)
        I2cConnectionSettings settings = new(1, I2cAddress);
        using I2cDevice device = I2cDevice.Create(settings);

        // Initialize the chip
        Initialize(device);

        // Turn on some LEDs (e.g., at (0,0), (6,4), (12,8))
        SetLedState(device, 0, 0, true);  // Top-left
        SetLedState(device, 6, 4, true);  // Middle-ish
        SetLedState(device, 12, 8, true); // Bottom-right

        Console.WriteLine("LEDs on! Press Ctrl+C to exit.");
        Thread.Sleep(Timeout.Infinite);
    }

    static void Initialize(I2cDevice device)
    {
        // Select Function Register page
        device.Write(new byte[] { PageSelect, 0x0B });
        // Set to normal operation (disable shutdown)
        device.Write(new byte[] { ConfigReg, 0x01 });

        // Select LED Control page (Page 0)
        device.Write(new byte[] { PageSelect, 0x00 });
        // Enable LEDs for 13x9 matrix (117 LEDs, 15 bytes needed)
        for (byte i = 0; i < 15; i++) // 117 bits = 14.625 bytes, round up to 15
        {
            device.Write(new byte[] { (byte)(LedControlBase + i), 0xFF });
        }
    }

    static void SetLedState(I2cDevice device, int x, int y, bool state)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            Console.WriteLine($"Invalid coordinates: ({x}, {y})");
            return;
        }

        // Map 13x9 coordinates to LED register
        int ledIndex = x + (y * Width); // Linear index for 13x9
        byte register = (byte)(LedControlBase + (ledIndex / 8));
        byte bit = (byte)(ledIndex % 8);
        byte mask = (byte)(1 << bit);

        // Read current state, modify, write back
        byte[] readBuffer = new byte[1];
        device.WriteRead(new byte[] { register }, readBuffer);
        byte current = readBuffer[0];
        byte newValue = state ? (byte)(current | mask) : (byte)(current & ~mask);
        device.Write(new byte[] { register, newValue });
    }
}