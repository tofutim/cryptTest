using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace cryptPcl
{
    public class fromPcl
    {
		private static readonly int _keyLen = 32;
		private static readonly int _ivLen = 16;

		// TODO: Should this be hardcoded?
		private static string _pepper = "U2FsdCBpcyBnb29kOiBidXQgaWYgdGhlIHNhbHQgaGF2ZSBsb3N0IGhpcyBzYWx0bmVzcywgd2hlcmV3aXRoIHdpbGwgeWUgc2Vhc29uIGl0PyBIYXZlIHNhbHQgaW4geW91cnNlbHZlcywgYW5kIGhhdmUgcGVhY2Ugb25lIHdpdGggYW5vdGhlci4=";
		private static int _iterations = 1335;

		public static void Test()
		{
			byte[] signal = { 1, 2, 3 };
			byte[] result;

			var passwordBytes = Encoding.UTF8.GetBytes("potato");
			var pepperBytes = Convert.FromBase64String(_pepper);

			var derivedKey = new Rfc2898DeriveBytes(passwordBytes, pepperBytes, _iterations);
			var symKey = derivedKey.GetBytes(_keyLen);

			using (var aes = Aes.Create())
			{
				var encryptor = aes.CreateEncryptor();
				using (var ms = new MemoryStream())
				{
					using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					{
						cs.Write(signal, 0, signal.Length);
						cs.FlushFinalBlock();
					}
					result = ms.ToArray();
				}
			}

			Debug.WriteLine(result.Length);
		}
    }
}
