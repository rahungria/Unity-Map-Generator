using System.IO;
using UnityEngine;

public static class HexMapSaveManager {
    private static HexGrid grid;

    public static void Save(HexGrid grid) {
        DirectoryInfo info = Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "saves"));
        string path = Path.Combine(info.FullName, "test.hexsav");

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) {
            //8 empty bytes for future proofing
            Debug.Log(path);
            writer.Write((long)0);
            grid.Save(writer);
        }
        
    }
    public static void Load(HexGrid grid){
        string path = Path.Combine(Application.persistentDataPath, "test.hexsav");

        using (BinaryReader reader = new BinaryReader(File.OpenRead(path))) {
            //8 empty bytes for future proofing
            reader.ReadInt64();
            grid.Load(reader);
        }
        
    }
}