﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

class UpgradeVSProject
{
	[MenuItem("File/Target Framework/Reset Solution Frameworks to v4.0")]
	static void UpgradeSolutions()
	{
		string currentDir = Directory.GetCurrentDirectory();
		string[] slnFile = Directory.GetFiles(currentDir, "*.sln");
		string[] csprojFile = Directory.GetFiles(currentDir, "*.csproj");
		List<string> formatUpdates = new List<string>();
		List<string> toolsUpdates = new List<string>();
		List<string> frameworkUpdates = new List<string>();
		
		if (slnFile != null)
		{
			for (int i = 0; i < slnFile.Length; i++)
			{
				if (ReplaceInFile(slnFile[i], "Format Version 10.00", "Format Version 11.00"))
				{
					formatUpdates.Add(Path.GetFileNameWithoutExtension(slnFile[i]));
				}
			}
		}
		
		if (csprojFile != null)
		{
			for (int i = 0; i < csprojFile.Length; i++)
			{
				if (ReplaceInFile(csprojFile[i], "ToolsVersion=\"3.5\"", "ToolsVersion=\"4.0\""))
				{
					toolsUpdates.Add(Path.GetFileNameWithoutExtension(csprojFile[i]));
				}
				
				if (ReplaceInFile(csprojFile[i], "<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>", "<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>"))
				{
					frameworkUpdates.Add(Path.GetFileNameWithoutExtension(csprojFile[i]));
				}
			}
		}
		
		if (formatUpdates.Count > 0 || toolsUpdates.Count > 0 || frameworkUpdates.Count > 0)
		{
			StringBuilder sb = new StringBuilder(512);
			sb.AppendFormat("The following solution and project files were updated...{0}", Environment.NewLine);
			
			if (formatUpdates.Count > 0)
			{
				sb.AppendFormat("{0}Project Format Update:{0}", Environment.NewLine);
				foreach(string formatUpdate in formatUpdates)
					sb.AppendFormat("  - {0}{1}", formatUpdate, Environment.NewLine);
			}
			
			if (toolsUpdates.Count > 0)
			{
				sb.AppendFormat("{0}Tools Update:{0}", Environment.NewLine);
				foreach(string toolsUpdate in toolsUpdates)
					sb.AppendFormat("  - {0}{1}", toolsUpdate, Environment.NewLine);
			}
			
			if (frameworkUpdates.Count > 0)
			{
				sb.AppendFormat("{0}Framework Update:{0}", Environment.NewLine);
				foreach(string frameworkUpdate in frameworkUpdates)
					sb.AppendFormat("  - {0}{1}", frameworkUpdate, Environment.NewLine);
			}
			
			EditorUtility.DisplayDialog("Framework Update", sb.ToString(), "OK");
		}
		else
		{
			EditorUtility.DisplayDialog("Framework Update", "No solutions were changed", "OK");
		}
	}
	
	static private bool ReplaceInFile(string filePath, string searchText, string replaceText)
	{
		StreamReader reader = new StreamReader(filePath);
		string content = reader.ReadToEnd();
		reader.Close();
		if (content.IndexOf(searchText) != -1)
		{
			content = Regex.Replace(content, searchText, replaceText);
			StreamWriter writer = new StreamWriter(filePath);
			writer.Write(content);
			writer.Close();
			return true;
		}
		
		return false;
	}
}