using System;

namespace SourceLibrary.IO.IsolatedStorage
{
	/// <summary>
	/// Summary description for IsolatedStorageSettingVersionBase.
	/// </summary>
	public abstract class IsolatedStorageSettingVersionBase : IsolatedStorageSettingBase
	{
		private int m_Version;
		private const string c_Check = "BINSETTING";
		public IsolatedStorageSettingVersionBase(int p_Version)
		{
			m_Version = p_Version;
		}
	
		public virtual int Version
		{
			get{return m_Version;}
		}
	
		protected override void OnLoad(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File)
		{
			string l_Check;
			Read(p_File, out l_Check);
			if (l_Check!=c_Check)
				throw new SourceLibrary.IO.InvalidDataException();

			int l_CurrentVersion;
			Read(p_File, out l_CurrentVersion);
			OnLoad(p_File,l_CurrentVersion);
		}
	
		protected abstract void OnLoad(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File, int p_CurrentVersion);

		protected override void OnSave(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File)
		{
			Write(p_File,c_Check);
			Write(p_File, Version);
			//custom values
		}
	}
}
