namespace PictureStorage;

public class Picture
{
    public Picture(byte[] bytes, string name)
    {
        RawData = bytes;
        Name = name;
    }
    public byte[] RawData { get; set; }
    
    public string Name { get; set; } 
}