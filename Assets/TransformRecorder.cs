using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class TransformRecorder : MonoBehaviour {

    string _logDir;
    string _logFile;
    string _logFilePath;

    long _logTime;
    long _logChunkTime;
    StringBuilder _outStr;


    void Awake()
    {
        _outStr =  new StringBuilder();
        
        _logDir = Path.Combine(Application.persistentDataPath, "Recordings");
        _logFile = string.Format("log_{0}.csv", System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        _logFilePath = Path.Combine(_logDir, _logFile);

        UpdateOutStr();
        System.IO.File.WriteAllText(_logFilePath, "");

        UpdateOutStr();
        _logChunkTime = System.DateTime.Now.Ticks;
    }

    void UpdateOutStr()
    {
        _logTime = System.DateTime.Now.Ticks;

        _outStr.Append(Time.timeSinceLevelLoad);
        _outStr.Append(',');
        _outStr.Append(transform.position.x);
        _outStr.Append(',');
        _outStr.Append(transform.position.y);
        _outStr.Append(',');
        _outStr.Append(transform.position.z);
        _outStr.Append(',');
        _outStr.Append(transform.rotation.x);
        _outStr.Append(',');
        _outStr.Append(transform.rotation.y);
        _outStr.Append(',');
        _outStr.Append(transform.rotation.z);
        _outStr.Append(',');
        _outStr.Append(transform.rotation.w);
        _outStr.AppendLine();
    }

    void AppendOutStrToFile()
    {
        _logChunkTime = System.DateTime.Now.Ticks;

        File.AppendAllText(_logFilePath, _outStr.ToString());
        _outStr.Length = 0;
    }
	
	void Update () 
    {
        if ((System.DateTime.Now.Ticks - _logTime) / 100000 >= 1) // 100 samples per second
        {
            UpdateOutStr();
            _logTime = System.DateTime.Now.Ticks;

            if ((System.DateTime.Now.Ticks - _logChunkTime) / 50000000 >= 1) // every 5 seconds
            {
                AppendOutStrToFile();
            }
        }	
	}
}
