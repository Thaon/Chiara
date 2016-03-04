using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class XMLSerializer : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //Serialize();
            ChunkWorldBuilder world = (ChunkWorldBuilder) FindObjectOfType(typeof(ChunkWorldBuilder));
            SaveChunkToXMLFile(world, "WorldSaveTest");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //Desirealize();
            ChunkWorldBuilder world = (ChunkWorldBuilder)FindObjectOfType(typeof(ChunkWorldBuilder));
            //world.m_chunks = LoadChunkFromXMLFile(world, "WorldSaveTest");
            world.UpdateWorld(LoadChunkFromXMLFile(world, "WorldSaveTest"));
        }
    }

    // Write a voxel chunk to XML file
    public static void SaveChunkToXMLFile(ChunkWorldBuilder world, string fileName)
    {
        //setup the writing style
        XmlWriterSettings writerSettings = new XmlWriterSettings();
        writerSettings.Indent = true;

        // Create a write instance
        XmlWriter xmlWriter = XmlWriter.Create(fileName + ".xml", writerSettings);
        // Write the beginning of the document
        xmlWriter.WriteStartDocument();

        xmlWriter.WriteStartElement("Root");

        //now we parse all our chunks' data!
        for (int cz = 0; cz < world.m_chunks.GetLength(0); cz++)
        {
            for (int cx = 0; cx < world.m_chunks.GetLength(1); cx++)
            {
                //we get a reference to the chunk data
                int[,,] voxelArray = world.m_chunks[cx, cz].m_terrainArray;
                // Create the root element
                xmlWriter.WriteStartElement("VoxelChunk");
                //write it's coordinates
                xmlWriter.WriteAttributeString("x", cx.ToString());
                xmlWriter.WriteAttributeString("z", cz.ToString());
                //then we write all of the coordinates of the chunk at that location
                for (int x = 0; x < voxelArray.GetLength(0); x++)
                {
                    for (int y = 0; y < voxelArray.GetLength(1); y++)
                    {
                        for (int z = 0; z < voxelArray.GetLength(2); z++)
                        {
                            if (voxelArray[x, y, z] != 0)
                            {
                                // Create a single voxel element
                                xmlWriter.WriteStartElement("Voxel");
                                xmlWriter.WriteAttributeString("x", x.ToString());
                                xmlWriter.WriteAttributeString("y", y.ToString());
                                xmlWriter.WriteAttributeString("z", z.ToString());
                                xmlWriter.WriteString(voxelArray[x, y, z].ToString());
                                xmlWriter.WriteEndElement();
                            }
                        }
                    }
                }
                //end current chunk
                xmlWriter.WriteEndElement();
            }
        }
        Debug.Log("EOF");
        // End the root element
        xmlWriter.WriteEndElement();
        // Write the end of the document
        xmlWriter.WriteEndDocument();
        // Close the document to save
        xmlWriter.Close();


    }
    // Read a voxel chunk from XML file
    public static ChunkBuilder[,] LoadChunkFromXMLFile(ChunkWorldBuilder world, string fileName)
    {
        //initialise our new world
        ChunkBuilder[,] worldArray = new ChunkBuilder[world.m_worldXSize, world.m_worldZSize];
        int[,,] vertexArray = new int[world.m_chunkSize, world.m_chunkSize, world.m_chunkSize];

        //get down to business
        XmlReader xmlReader = XmlReader.Create(fileName + ".xml");        while (xmlReader.Read())
        {
            //loop through the chunks
            if (xmlReader.IsStartElement("VoxelChunk"))
            {
                //get the voxel chunk data
                int cx = int.Parse(xmlReader["x"]);
                int cz = int.Parse(xmlReader["z"]);
                if (xmlReader.IsStartElement("Voxel"))
                {
                    int x = int.Parse(xmlReader["x"]);
                    int y = int.Parse(xmlReader["y"]);
                    int z = int.Parse(xmlReader["z"]);                    int type = int.Parse(xmlReader.Value);                    //create the chunk
                    worldArray[cx, cz].m_terrainArray[x, y, z] = type;
                }
            }
        }
        return worldArray;
    }
}

/*
 public void Serialize()
 {
     Debug.Log("Serializing stuff yo");

     SerializedDataObject tdc = new SerializedDataObject(
     "MyName", "This is a serialised item");
     XmlSerializer x = new XmlSerializer(tdc.GetType());
     System.IO.FileStream file = System.IO.File.Create("TestFile.xml");
     x.Serialize(file, tdc);
     file.Close();

     Debug.Log(Application.dataPath);
     Debug.Log(Application.persistentDataPath);
 }

 public void Desirealize()
 {
     SerializedDataObject tdc = new SerializedDataObject("", "");
     XmlSerializer x = new XmlSerializer(tdc.GetType());
     System.IO.FileStream file =
    System.IO.File.OpenRead("TestFile.xml");
     tdc = (SerializedDataObject)x.Deserialize(file);
     file.Close();
     print(tdc.GetName() + ": " + tdc.GetData());
 }
 
public class SerializedDataObject
{
    public string m_name, m_data;

    SerializedDataObject()
    {

    }

    public SerializedDataObject(string name, string data)
    {
        m_name = name;
        m_data = data;
    }

    public string GetName() { return m_name; }
    public string GetData() { return m_data; }

    public void SetName(string value) { m_name = value; }
    public void SetData(string value) { m_data = value; }
}
*/