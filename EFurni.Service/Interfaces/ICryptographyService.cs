namespace EFurni.Services
{
    public interface ICryptographyService
    {
        string HashString(string str,string saltHash);
    }
}