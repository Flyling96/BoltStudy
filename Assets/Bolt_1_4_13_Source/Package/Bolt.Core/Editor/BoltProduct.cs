using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Ludiq;
using UnityEditor;
using UnityEngine;

namespace Bolt
{
	[Product(ID)]
	public sealed class BoltProduct : Product
	{
		public BoltProduct() { }

		public override void Initialize()
		{
			base.Initialize();
			
			logo = BoltCore.Resources.LoadTexture("Logos/LogoBolt.png", CreateTextureOptions.Scalable)?.Single();
		}

		public override string configurationPanelLabel => "Bolt";

		public override string name => "Bolt";
		public override string description => "";
		public override string authorLabel => "Designed & Developed by ";
		public override string author => "";
		public override string copyrightHolder => "Unity";
		public override SemanticVersion version => "1.4.13";
		public override string publisherUrl => "";
		public override string websiteUrl => "";
		public override string supportUrl => "";
		public override string manualUrl => "https://docs.unity3d.com/Packages/com.unity.bolt@latest";
		public override string assetStoreUrl => "http://u3d.as/1Md2";

		public const string ID = "Bolt";

		public const int ToolsMenuPriority = -990000;
		public const int DeveloperToolsMenuPriority = ToolsMenuPriority + 1000;

		public static BoltProduct instance => (BoltProduct)ProductContainer.GetProduct(ID);
		
		public string documentationPath => Path.Combine(packagePath, "Documentation");

		[SettingsProvider]
		private static SettingsProvider BoltSettingsProvider()
		{
			var provider = new SettingsProvider("Preferences/Bolt", SettingsScope.User)
			{
				label = "Bolt",
				guiHandler = (searchContext) =>
				{
					if (EditorApplication.isCompiling)
					{
						LudiqGUI.CenterLoader();
						return;
					}

					instance.configurationPanel.PreferenceItem();
				}
			};

			return provider;
		}

		[MenuItem("Tools/Bolt/About...", priority = ToolsMenuPriority + 1)]
		private static void HookAboutWindow()
		{
			instance.aboutWindow.Show();
		}

		[MenuItem("Tools/Bolt/Manual...", priority = ToolsMenuPriority + 3)]
		private static void HookDocumentation()
		{
			WebWindow.Show(new GUIContent("Bolt Manual", LudiqCore.Icons.supportWindow?[IconSize.Small]), instance.manualUrl);
		}

		[MenuItem("Tools/Bolt/Setup Wizard...", priority = ToolsMenuPriority + 101)]
		private static void HookSetupWizard()
		{
			instance.setupWizard.Show();
		}

		[MenuItem("Tools/Bolt/Update Wizard...", priority = ToolsMenuPriority + 102)]
		private static void HookUpdateWizard()
		{
			instance.updateWizard.Show();
		}

		[MenuItem("Tools/Bolt/Configuration...", priority = ToolsMenuPriority + 201)]
		private static void HookConfiguration()
		{
			instance.configurationPanel.Show();
		}

		[MenuItem("Tools/Bolt/Internal/Prepare for Release...", priority = DeveloperToolsMenuPriority + 101)]
		private static bool PrepareForRelease()
		{
			if (!EditorUtility.DisplayDialog("Delete Generated Files", "This action will delete all generated files, including those containing user data.\n\nAre you sure you want to continue?", "Confirm", "Cancel"))
			{
				return false;
			}

			PluginConfiguration.DeleteAllProjectSettings();

			foreach (var plugin in PluginContainer.plugins)
			{
				PathUtility.DeleteDirectoryIfExists(plugin.paths.persistentGenerated);
				PathUtility.DeleteDirectoryIfExists(plugin.paths.transientGenerated);
			}

			PluginAcknowledgement.GenerateLicenseFile(Path.Combine(instance.packagePath, "LICENSES.txt"));

			return true;
		}

		[MenuItem("Tools/Bolt/Internal/Export Release Package...", priority = DeveloperToolsMenuPriority + 102)]
		private static void ExportReleasePackage()
		{
			if (!PrepareForRelease())
			{
				return;
			}

			string runtimeString;

#if UNITY_2019_3_OR_NEWER
			runtimeString = "NET4";
#else
			switch (PlayerSettings.scriptingRuntimeVersion)
			{
				case ScriptingRuntimeVersion.Latest:
					runtimeString = "NET4";
					break;

				case ScriptingRuntimeVersion.Legacy:
					runtimeString = "NET3";
					break;

				default:
					runtimeString = PlayerSettings.scriptingRuntimeVersion.ToString();
					break;
			}
#endif
			var packagePath = EditorUtility.SaveFilePanel("Export Release Package",
														  Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
														  "Bolt_" + instance.version.ToString().Replace(".", "_").Replace(" ", "_") + "_" + runtimeString,
														  "unitypackage");

			if (packagePath == null)
			{
				return;
			}

			var packageDirectory = Path.GetDirectoryName(packagePath);

			ProgressUtility.DisplayProgressBar("Exporting Release Package", "Creating Unity Package...", 0);

			var paths = new List<string>()
			{
				instance.documentationPath,
				Path.Combine(Paths.assets, "Ludiq/Assemblies")
			};

			foreach (var product in ProductContainer.products)
			{
				paths.Add(product.packagePath);
			}

			foreach (var plugin in PluginContainer.plugins)
			{
				paths.Add(plugin.paths.package);
			}

			AssetDatabase.ExportPackage(paths.Select(PathUtility.FromProject).ToArray(), packagePath, ExportPackageOptions.Recurse);

			ProgressUtility.DisplayProgressBar("Exporting Release Package", "Creating Source Archive...", 0.5f);

			var sourceArchiveFileName = Path.GetFileNameWithoutExtension(packagePath) + "_Source.zip";
			var sourceArchivePath = Path.Combine(packageDirectory, sourceArchiveFileName);
			var sourceDirectory = Directory.GetParent(Directory.GetParent(Paths.project).FullName).FullName;
			var gitArchiveProcess = new ProcessStartInfo();
			gitArchiveProcess.WorkingDirectory = sourceDirectory;
			gitArchiveProcess.FileName = "git";
			gitArchiveProcess.Arguments = $"archive -o \"{sourceArchivePath}\" HEAD";

			try
			{
				Process.Start(gitArchiveProcess).WaitForExit(3000);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Failed to create source archive: \n" + ex);
			}

			ProgressUtility.ClearProgressBar();

			if (EditorUtility.DisplayDialog("Export Release Package", "Release package export complete.\nOpen containing folder?", "Open Folder", "Close"))
			{
				Process.Start(packageDirectory);
			}
		}
	}
}