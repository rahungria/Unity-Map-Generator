using System.IO;
using UnityEngine;

public static class HexMapSaveManager {
    private static HexGrid grid;

    public static void Save(HexGrid grid) {
        string path = Path.Combine(Application.persistentDataPath, "test.hexsav");

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) {
            //8 empty bytes for future proofing
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