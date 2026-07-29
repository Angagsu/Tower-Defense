// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Djr9DH5phiBaofMJQq9XueOPGEycEgOWZYV2PizDG15YRwgMWdqMe0IEQZaHYPqYKPvoK4xosH8pCEwgHk6XhlL+ucNnhLYH0fSRBr6br7hW5GdEVmtgb0zgLuCRa2dnZ2NmZYW0wkhfvt5FOolzfCzcRXjowxPIQVEpb1Z0CBJ7DD15HkEm1YZkIlNKaYaUc+JG0NKpp/KwTaUT2AvMH+RnaWZW5GdsZORnZ2bbL7edH2cvGUCc2QvByG0UGOhRQqPP4YgfAZP4C7E0BrS5ZqtQTqOz9nORN/E+UI3eH5jBfSzBvxv0FN/3Xd5GdnCOIdIipxSsjsp86UuwavRLSF/mOXQ0JLGSNH8I0MSAsaWwfXB2/GyEyoVDJdw/65WgiWRlZ2Zn");
        private static int[] order = new int[] { 11,7,3,4,11,8,10,12,12,9,10,13,13,13,14 };
        private static int key = 102;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
