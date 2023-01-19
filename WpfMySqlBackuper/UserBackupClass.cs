using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WpfMySqlBackuper
{
	[Serializable]
	class UserBackupClass
	{

		
        public string server { get; set; }
		public string user { get; set; }
		public string password { get; set; }
		public string database { get; set; }


		public void serializeBackupData(string server,string user,string passw, string db)
		{
			UserBackupClass userbp = new UserBackupClass();
			userbp.server = server;
			userbp.user = user;
			userbp.password = passw;
			userbp.database = db;

			string fileName = "UserBpData.json";
			string jsonString = JsonSerializer.Serialize(userbp);
			File.WriteAllText(fileName, jsonString);
		}

		public UserBackupClass deserializeBackupData(string json= "UserBpData.json")
		{
			string file = File.ReadAllText(json);
			UserBackupClass deserializedUserdp = JsonSerializer.Deserialize<UserBackupClass>(file)!;
			return deserializedUserdp;
		}
	}
}
