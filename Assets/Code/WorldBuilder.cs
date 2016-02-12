using UnityEngine;
using System.Collections;
using System;
using System.IO;

[RequireComponent(typeof(MeshGenerator))]

public class WorldBuilder : MonoBehaviour {

    MeshGenerator m_generator;
    string m_worldGrid;
    int x, y, z;

	// Use this for initialization
	void Start ()
    {
        m_generator = GetComponent<MeshGenerator>();

        x = 0;
        y = 1; //we set this to 1 to place the Lerper on top of the level
        z = 0;

        m_worldGrid =
            @"
            00003
            00011
            00010
            31110
            00000";

        m_generator.WorldInit();
        Debug.Log("World initialized");
        PopulateWorld();
        Debug.Log("World populated");
        m_generator.UpdateWorld();
        Debug.Log("World updated");
    }

    void PopulateWorld ()
    {
        //from http://stackoverflow.com/questions/1500194/c-looping-through-lines-of-multiline-string
        using (StringReader reader = new StringReader(m_worldGrid))
        {
            string line = string.Empty;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    //invert the line contents (they generate a flipped mesh for some reason, this fixes it)
                    line = Reverse(line);
                    //create a "line" of world
                    foreach(char tile in line)
                    {
                        CreateTile(tile);
                        x++;
                    }
                }
                x = 0;
                z++;
            } while (line != null);
        }
	}

    void CreateTile(char tile)
    {
        switch (tile)
        {
            //special cases here
            case 'b':
                //create the lerping gameobject
                break;

            //now for the voxels
            case '1':
                m_generator.CreateVoxel(x, y, z, "Grass");
                //Debug.Log("Grass created");
                break;

            case '3':
                m_generator.CreateVoxel(x, y, z, "Sand");
                //Debug.Log("Dirt created");
                break;
        }
    }

    //UTILITY SCRIPTS
    public static string Reverse(string s) //from http://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
