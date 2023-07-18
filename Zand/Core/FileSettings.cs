using System;
using System.Collections.Generic;
using System.IO;


namespace Zand
{
    public class FileSettings
    {
        private readonly Dictionary<string, int> _settings;

        public FileSettings(string filePath)
        {
            _settings = new Dictionary<string, int>();
            AddSettings(File.ReadAllLines(filePath));
        }

        public int this[string index]
        {
            get
            {
                return _settings[index];
            }
            set
            {
                _settings[index] = value;
            }
        }

        public void AddSettings(string[] lines)
        {
            foreach (string line in lines)
            {

                // Skip comments
                if (line.StartsWith('#'))
                {
                    continue;
                }

                // Parse values
                string[] setting = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                _settings.Add(setting[0], int.Parse(setting[1]));
            }
        }
    }
}
