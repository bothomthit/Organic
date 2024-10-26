using System.Security.Cryptography;
using System.Text;

namespace WebApp.Models;
public static class Helper
{
    public static string Hash(string plaintext){
        HashAlgorithm algorithm = SHA512.Create();
        return Convert.ToHexString(algorithm.ComputeHash(Encoding.ASCII.GetBytes(plaintext)));
        
    }
}