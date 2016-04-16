﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class XMLSerializer : MonoBehaviour
{
    static int cx, cz, x, y, z, type;
    static ChunkWorldBuilder world;
    public string m_fileName = "WorldSaveTest";

    void Start()
    {
        world = (ChunkWorldBuilder)FindObjectOfType(typeof(ChunkWorldBuilder));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //Serialize();
            SaveChunkToXMLFile(world.m_chunkSize, m_fileName);
            //print(world.m_chunks[0, 0].m_terrainArray);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            world.m_chunk.m_terrainArray = LoadChunkFromXMLFile(world.m_chunkSize, m_fileName);
            // Draw the correct faces
            world.m_chunk.UpdateChunk();
        }
    }

    // Write a voxel chunk to XML file
    public static void SaveChunkToXMLFile(int size, string fileName)
    {
        int[,,] voxelArray = world.m_chunk.m_terrainArray;
        //setup the writing style
        XmlWriterSettings writerSettings = new XmlWriterSettings();
        writerSettings.Indent = true;

        // Create a write instance
        XmlWriter xmlWriter = XmlWriter.Create(fileName + ".xml", writerSettings);
        // Write the beginning of the document
        xmlWriter.WriteStartDocument();

        xmlWriter.WriteStartElement("VoxelChunk");

        //now we parse all our chunks' data!

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
        Debug.Log("EOF");
        // End the root element
        xmlWriter.WriteEndElement();
        // Write the end of the document
        xmlWriter.WriteEndDocument();
        // Close the document to save
        xmlWriter.Close();
    }
    // Read a voxel chunk from XML file
    //public static ChunkBuilder[,] 
    public int[,,] LoadChunkFromXMLFile(int size, string fileName)
    {
        int[,,] voxelArray = new int[size, size, size];

        XmlReader xmlReader = XmlReader.Create(fileName + ".xml");

        while (xmlReader.Read())
        {
            if (xmlReader.IsStartElement("Voxel"))
            {
                int x = int.Parse(xmlReader["x"]);
                int y = int.Parse(xmlReader["y"]);
                int z = int.Parse(xmlReader["z"]);
                xmlReader.Read();
                int value = int.Parse(xmlReader.Value);
                //m_voxelType type;
                //switch (value)
                //{
                //    case 0:
                //    type = m_voxelType.empty;
                //    break;
                //    case 1:
                //    type = m_voxelType.grass;
                //    break;
                //    case 2:
                //    type = m_voxelType.stone;
                //    break;
                //    case 3:
                //    type = m_voxelType.dirt;
                //    break;
                //    case 4:
                //    type = m_voxelType.sand;
                //    break;
                //}
                voxelArray[x, y, z] = value;
                //print(x + "-" + y + "-" + z);
            }
        }
        print("EOF!");
        return voxelArray;
    }
}
/*

    //ChunkBuilder[,] worldArray = new ChunkBuilder[world.m_worldXSize, world.m_worldZSize];
        //delete previous chunks
        //create new chunks from ChunkWorldBuilder

        //get down to business
        XmlDocument xmlDocument = new XmlDocument();        xmlDocument.Load(fileName + ".xml");        //bool readingChunk = false;

        XmlNodeList chunks = xmlDocument.DocumentElement.ChildNodes;
        //print(chunks.ToString());

        foreach (XmlNode chunk in chunks)
        {
            int[,,] tempTerrainArray = new int[world.m_chunkSize, world.m_chunkSize, world.m_chunkSize];
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
                    //print(world.m_chunks[cx, cz].m_terrainArray[x, y, z]);

                    type = int.Parse(voxel.FirstChild.Value);
                    //print(vertexArray[x, y, z]);
                }
                tempTerrainArray[x, y, z] = type;
                //world.m_chunks[cx, cz].m_terrainArray = new int[world.m_chunkSize, world.m_chunkSize, world.m_chunkSize];
                //world.m_chunks[cx, cz].m_terrainArray = tempTerrainArray;
                //clear the previous mesh data
                world.m_chunks[cx, cz].m_voxelGenerator.ClearPreviousData();
                // Change the block to the required type
                world.m_chunks[cx, cz].m_terrainArray = tempTerrainArray;
                // Create the new mesh
                world.m_chunks[cx, cz].DisplayTerrain();
                // Update the mesh data
                world.m_chunks[cx, cz].m_voxelGenerator.UpdateWorld();
            }
            //print(cx + " " + cz);
            //print(worldArray.GetLength(0));

            //worldArray[cx, cz].m_terrainArray = new int[worldArray.GetLength(0), worldArray.GetLength(0), worldArray.GetLength(0)];
            //print(worldArray[cx, cz].m_terrainArray);
            //worldArray[cx, cz].m_terrainArray = vertexArray;

            //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    //Desirealize();
        //    //world.m_chunks = LoadChunkFromXMLFile(world, "WorldSaveTest");
        //    //ChunkBuilder[,] loadedWorld = 
        //    //world.m_chunks = LoadChunkFromXMLFile(world, "WorldSaveTest");
        //    //print(world.m_chunks[0, 0].m_terrainArray);
        //    //Debug.Log("EOF");
        //    /*
        //    foreach(ChunkBuilder chunk in world.m_chunks)
        //    {
        //        //clear the previous mesh data
        //        chunk.m_voxelGenerator.ClearPreviousData();
        //        // Change the block to the required type
        //        chunk.m_terrainArray =;
        //        // Create the new mesh
        //        chunk.DisplayTerrain();
        //        // Update the mesh data
        //        chunk.m_voxelGenerator.UpdateWorld();
        //    }
        //    */
//    //world.UpdateWorld(loadedWorld);
//}
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

/*
    //initialise our new world
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
    */
