using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;


public class NarrationJsonConverter
{
    private readonly string _jsonFilePath;

    public NarrationJsonConverter(string filePath) //constructor that allows the filepath to be set
    {
        _jsonFilePath = filePath;
    }

    public Dictionary<NarrationType, List<Narration>> GetNarrationsAsGameFormat()
    {
        if (!File.Exists(_jsonFilePath)) //creates file if it dosent exsist
        {
            File.Create(_jsonFilePath);

            Debug.Log("json File didn't exsist, it was created, empty dictionary returned");
            return new Dictionary<NarrationType, List<Narration>>();
        }

        //reads file and gets dictionary
        string fileText = "";

        FileStream readStream = new FileStream(_jsonFilePath, FileMode.Open, FileAccess.Read); //reads from the file

        if (readStream.CanRead)
        {
            byte[] buffer = new byte[readStream.Length];
            int bytesRead = readStream.Read(buffer, 0, buffer.Length);

            fileText = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
        else
        {
            Debug.LogError("Couldn't read from file, returning null");

            readStream.Flush();
            readStream.Close();
            return null;
        }

        readStream.Flush();
        readStream.Close();

        NarrationSerilizableContainer narrationfromJson = JsonUtility.FromJson<NarrationSerilizableContainer>(fileText);
        if(narrationfromJson != null)
        {
            return narrationfromJson.GetValuesAsGameUsable();
        }
        else
        {
            Debug.LogWarning("Json file was either empty, or failed to be retreived. returning null");
            return null;
        }

    }

    public void SaveNarration(Narration narrationToSave)
    {
        if (!File.Exists(_jsonFilePath)) //creates file if it dosent exsist
        {
            File.Create(_jsonFilePath);

            Debug.Log("json File didn't exsist, it was created, please try saving again");
            return;
        }
        {
            string fileText = "";

            FileStream readStream = new FileStream(_jsonFilePath, FileMode.Open, FileAccess.Read); //reads from the file

            if (readStream.CanRead)
            {
                byte[] buffer = new byte[readStream.Length];
                int bytesRead = readStream.Read(buffer, 0, buffer.Length);

                fileText = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            else
            {
                Debug.LogError("Couldn't read from file");

                readStream.Flush();
                readStream.Close();
            }


            readStream.Flush();
            readStream.Close();


            //gets the saved information
            NarrationSerilizableContainer narrationContainer = JsonUtility.FromJson<NarrationSerilizableContainer>(fileText);
            Dictionary<NarrationType, List<Narration>> narrationContainerAsUsable;

            if (narrationContainer != null)
            {
                narrationContainerAsUsable = narrationContainer.GetValuesAsGameUsable();
                Debug.Log("saved dictionary acsessed");
            }
            else
            {
                narrationContainerAsUsable = new Dictionary<NarrationType, List<Narration>>();
                Debug.Log("new dictionary made");
            }

            //not overwriting, slots in the new narration
            //adds in new narration
            if (narrationContainerAsUsable.TryGetValue(narrationToSave.narrationType, out List<Narration> narrationList))
            {
                //overwrite check and save
                bool overwritten = false;
                for (int i = 0; i < narrationList.Count; i++)
                {
                    if (narrationList[i].narrationName == narrationToSave.narrationName)
                    {
                        //same type and name, overwrite
                        narrationList.RemoveAt(i);
                        narrationList.Add(narrationToSave);
                        overwritten = true;
                        Debug.Log("Overwritten!");
                        break;
                    }

                }
                if (!overwritten)
                {
                    Debug.Log("Saved");
                    narrationList.Add(narrationToSave);
                }

            }
            else
            {
                narrationContainerAsUsable.Add(narrationToSave.narrationType, new List<Narration>());

                if (narrationContainerAsUsable.TryGetValue(narrationToSave.narrationType, out narrationList))
                {
                    narrationList.Add(narrationToSave);
                }
            }


            //recreates the serilizable contanner with the added narration, and then converts back to json
            narrationContainer = new NarrationSerilizableContainer(narrationContainerAsUsable);
            fileText = JsonUtility.ToJson(narrationContainer);

            //writes the new narration container to the file
            FileStream writeStream = new FileStream(_jsonFilePath, FileMode.Open, FileAccess.Write);


            if (writeStream.CanWrite)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(fileText);
                writeStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                Debug.LogError("Couldn't write to file");


                writeStream.Flush();
                writeStream.Close();
                return;
            }

            writeStream.Flush();
            writeStream.Close();
        }
    }

    public void TryDeleteNarration(Narration narrationToDelete)
    {
        if (!File.Exists(_jsonFilePath)) //creates file if it dosent exsist
        {
            File.Create(_jsonFilePath);

            Debug.Log("json File didn't exsist, it was created, please try saving again");
            return;
        }
        {
            string fileText = "";

            FileStream readStream = new FileStream(_jsonFilePath, FileMode.Open, FileAccess.Read); //reads from the file

            if (readStream.CanRead)
            {
                byte[] buffer = new byte[readStream.Length];
                int bytesRead = readStream.Read(buffer, 0, buffer.Length);

                fileText = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            else
            {
                Debug.LogError("Couldn't read from file");

                readStream.Flush();
                readStream.Close();
            }


            readStream.Flush();
            readStream.Close();


            //gets the saved information
            NarrationSerilizableContainer narrationContainer = JsonUtility.FromJson<NarrationSerilizableContainer>(fileText);
            Dictionary<NarrationType, List<Narration>> narrationContainerAsUsable;

            if (narrationContainer != null)
            {
                narrationContainerAsUsable = narrationContainer.GetValuesAsGameUsable();
                Debug.Log("saved dictionary acsessed");
            }
            else
            {
                narrationContainerAsUsable = new Dictionary<NarrationType, List<Narration>>();
                Debug.Log("new dictionary made");
            }

            //not overwriting, slots in the new narration
            //adds in new narration
            if (narrationContainerAsUsable.TryGetValue(narrationToDelete.narrationType, out List<Narration> narrationList))
            {
                for (int i = 0; i < narrationList.Count; i++)
                {
                    if (narrationList[i].narrationName == narrationToDelete.narrationName)
                    {
                        //same type and name, overwrite
                        Debug.Log("Pre erase: " + narrationList[i]);
                        narrationList.RemoveAt(i);
                        Debug.Log("Erased");

                        if(narrationList.Count < 1) //removes category if its the last one
                        {
                            narrationContainerAsUsable.Remove(narrationToDelete.narrationType);
                        }
                        break;
                    }
                }

            }


            //recreates the serilizable contanner with the added narration, and then converts back to json
            narrationContainer = new NarrationSerilizableContainer(narrationContainerAsUsable);
            fileText = JsonUtility.ToJson(narrationContainer);

            //writes the new narration container to the file
            FileStream writeStream = new FileStream(_jsonFilePath, FileMode.Truncate, FileAccess.Write);


            if (writeStream.CanWrite)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(fileText);
                writeStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                Debug.LogError("Couldn't write to file");


                writeStream.Flush();
                writeStream.Close();
                return;
            }

            writeStream.Flush();
            writeStream.Close();
        }
    }
}


[Serializable]
public class SavedNarrations //class combining and tracking serilized Narrations
{
    [SerializeField] public Dictionary<NarrationType, List<NarrationSerilizable>> _serializedNarrations; //dictionary organized by types of lists of serilized narrations
}