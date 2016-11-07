using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon
{
	public interface ISystemSettingService
	{
        SystemSetting GetForUpdate(string key);

        SystemSetting GetSystemSetting(string key);
        
        string Get(string key);
        T Get<T>(string key) where T: struct;
        T Get<T>(string key, T defaultValue) where T : struct;

        int AddSystemSetting(SystemSetting entry);
        void UpdateSystemSetting(SystemSetting entry);
        void UpdateSystemSetting(string key, object value);
        void RemoveSystemSetting(SystemSetting entry);
        IEnumerable<SystemSetting> GetAllSystemSetting();        
	}
}