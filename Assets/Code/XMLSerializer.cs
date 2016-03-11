using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class XMLSerializer : MonoBehaviour
{
    static int cx, cz, x, y, z, type;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //Serialize();
            ChunkWorldBuilder world = (ChunkWorldBuilder)FindObjectOfType(typeof(ChunkWorldBuilder));
            SaveChunkToXMLFile(world, "WorldSaveTest");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //Desirealize();
            ChunkWorldBuilder world = (ChunkWorldBuilder)FindObjectOfType(typeof(ChunkWorldBuilder));
            //world.m_chunks = LoadChunkFromXMLFile(world, "WorldSaveTest");
            ChunkBuilder[,] loadedWorld = LoadChunkFromXMLFile(world, "WorldSaveTest");
            Debug.Log("EOF");
            world.UpdateWorld(loadedWorld);
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
        for (int cz = 0; cz < world.m_worldZSize; ++cz)
        {
            for (int cx = 0; cx < world.m_worldXSize; ++cx)
            {
                //print(cx + " - " + cz);
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
        for (int z = 0; z < world.m_worldZSize; z++)
        {
            for (int x = 0; x < world.m_worldZSize; x++)
            {
                //create the new chunk
                GameObject newChunk = new GameObject();
                newChunk.name = "Chunk at: " + x + " : " + z;
                newChunk.AddComponent<ChunkBuilder>();
                newChunk.AddComponent<MeshGenerator>();

                //customise the new chunk
                ChunkBuilder cb = newChunk.GetComponent<ChunkBuilder>();
                cb.m_chunkSize = world.m_chunkSize;
                cb.m_chunkHeight = world.m_chunkHeight;
                cb.m_world = world;
                cb.GenerateChunk();
                worldArray[x, z] = cb;
            }
        }
        
        int[,,] vertexArray = new int[world.m_chunkSize, world.m_chunkSize, world.m_chunkSize];


        //get down to business
        XmlDocument xmlDocument = new XmlDocument();        xmlDocument.Load(fileName + ".xml");        //bool readingChunk = false;

        XmlNodeList chunks = xmlDocument.DocumentElement.ChildNodes;
        //print(chunks.ToString());

        foreach (XmlNode chunk in chunks)
        {
            if (chunk.HasChildNodes)
            {
                //get the voxel chunk data
                cx = int.Parse(chunk.Attributes["x"].Value);
                cz = int.Parse(chunk.Attributes["z"].Value);
                //print("reading chunk at " + cx + " ; " + cz);
                foreach (XmlNode voxel in chunk.ChildNodes)
                {
                    //print("reading voxel");
                    x = int.Parse(voxel.Attributes["x"].Value);
                    y = int.Parse(voxel.Attributes["y"].Value);
                    z = int.Parse(voxel.Attributes["z"].Value);
                    //print(voxel.FirstChild.Value);

                    //print(x + "," + y + "," + z + ";");
                    //print(worldArray[cx, cz].m_terrainArray[x, y, z]);

                    int type = int.Parse(voxel.FirstChild.Value);
                    //print(vertexArray[x, y, z]);
                }
            }
            print(cx + " " + cz);
            print(worldArray.GetLength(0));

            //worldArray[cx, cz].m_terrainArray = new int[worldArray.GetLength(0), worldArray.GetLength(0), worldArray.GetLength(0)];
            //print(worldArray[cx, cz].m_terrainArray);
            worldArray[cx, cz].m_terrainArray = vertexArray;
        }
        return worldArray;
    }
}
        /*
        while (xmlReader.Read())
        {
            //loop through the chunks
            if (xmlReader.IsStartElement("VoxelChunk"))
            {
                print("reading chunk");
                //get the voxel chunk data
                cx = int.Parse(xmlReader["x"]);
                cz = int.Parse(xmlReader["z"]);
                readingChunk = true;
            }

            if (readingChunk)
            {
                print("reading voxel");
                print(xmlReader.ToString());
                x = int.Parse(xmlReader["x"]);
                y = int.Parse(xmlReader["y"]);
                z = int.Parse(xmlReader["z"]);
                print(x + "," + y + "," + z + ";");
                int type = int.Parse(xmlReader.Value);                //create the chunk
                worldArray[cx, cz].m_terrainArray[x, y, z] = type;
                print(worldArray[cx, cz].m_terrainArray[x, y, z]);
                //xmlReader.Read();
            }
            if (xmlReader.NodeType == XmlNodeType.EndElement)
            {
                print(xmlReader.Value);
                readingChunk = false;
            }
        }
        return worldArray;
    }
    */

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