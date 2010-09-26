/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using System.Xml.Serialization;

using SolidOpt.Services.Subsystems.HetAccess.Importers;
using SolidOpt.Services.Subsystems.HetAccess;

namespace SolidOpt.Services.WebServices.SolidOptWebService
{
	public class ConfigParamDef
	{
		public string Name;
		public string Caption;
		public object Value;
		public object DefaultValue;
		public string Description;
		
		public ConfigParamDef()
		{	
		}
		
		public ConfigParamDef(string name, string caption, object value, object defaultValue, string description)
		{
			this.Name = name;
			this.Caption = caption;
			this.Value = value;
			this.DefaultValue = defaultValue;
			this.Description = description;
		}
	}
	
	public class ConfigParamsDef
	{
		[XmlElement("ConfigParamDef")]
		public List<ConfigParamDef> configParamsDef;
		
		public ConfigParamsDef()
		{
			configParamsDef = new List<ConfigParamDef>();
		}
	}
	
	public class ConfigParam
	{
		public string Name;
		public object Value;
		
		public ConfigParam()
		{	
		}
		
		public ConfigParam(string name, object value)
		{
			this.Name = name;
			this.Value = value;
		}
	}

	public class ConfigParams
	{
		[XmlElement("ConfigParam")]
		public List<ConfigParam> configParams;
		
		public ConfigParams()
		{
			configParams = new List<ConfigParam>();
		}
	}
	
	public class TransformMethod
	{
		public string Name;
		public string Caption;
		public string Version;
		public string Status;
		public string FullName;
		public string Description;
		public ConfigParamsDef ConfigParamsDef;
		
		public TransformMethod()
		{
		}
		
		public TransformMethod(string name, string caption, string version, string status, string fullName, string description, ConfigParamsDef configParamsDef)
		{
			this.Name = name;
			this.Caption = caption;
			this.Version = version;
			this.Status = status;
			this.FullName = fullName;
			this.Description = description;
			this.ConfigParamsDef = configParamsDef;
		}
	}
	
	public class TransformMethods
	{
		[XmlElement("TransformMethod")]
		public List<TransformMethod> methods;

		public TransformMethods()
		{
			methods = new List<TransformMethod>();
		}
		
	}
	
    [WebService(
		Namespace = "http://www.solidopt.org/ws/SolidOptService.asmx",
		Description = "This is a SolidOpt Web Service.")]
    [WebServiceBinding(
		Namespace = "http://www.solidopt.org/ws/SolidOptService.asmx",
		ConformsTo = WsiProfiles.BasicProfile1_1,
//	    EmitConformanceClaims = true,
	    Location = "http://www.solidopt.org/ws/SolidOptService.asmx")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
	public class SolidOptService : WebService
	{
		/// <summary>
		/// Logs into the web service
		/// </summary>
		/// <param name="userName">The User Name to login in as</param>
		/// <param name="password">User's password</param>
		/// <returns>True on successful login.</returns>
		[WebMethod(EnableSession=true)]
		public bool Login(string userName, string password)
		{
			//NOTE: There are better ways of doing authentication. This is just illustrates Session usage.
			UserName = userName;
			Session["opt"] = new MySession();
			return true;
		}
		
		/// <summary>
		/// Logs out of the Session.
		/// </summary>
		[WebMethod(EnableSession=true)]
		public void Logout()
		{    
			Session.Abandon();
		}
		
		/// <summary>
		/// Get optimization methods info
		/// </summary>
		[WebMethod(Description = "Get optimization methods info.")]
//		[SoapInclude(typeof(CompositeTransformMethod))]
//		[XmlInclude(typeof(CompositeTransformMethod))]
		public TransformMethods GetTransformMethods()
		{
			TransformMethods result = new TransformMethods();
			
			TransformMethod m;
			ConfigParamsDef p;
				
			p = new ConfigParamsDef();
			p.configParamsDef.Add(new ConfigParamDef("level", "Level", 0, 5, "Optimization level."));
			m = new TransformMethod("inline-0.5-beta", "Inline", "0.5", "beta", "SolidOpt.InlineTransformer, SolidOpt, Version=0.5.3700.223, Culture=neutral, PublicKeyToken=null", "Inline methods.", p);
			result.methods.Add(m);

			p = new ConfigParamsDef();
			p.configParamsDef.Add(new ConfigParamDef("level", "Level", 0, 5, "Optimization level."));
			p.configParamsDef.Add(new ConfigParamDef("clone", "Clone", true, true, "Use method clone."));
			m = new TransformMethod("inline-1.0-stable", "Inline", "1.0", "stable", "SolidOpt.InlineTransformer, SolidOpt, Version=1.0.3800.323, Culture=neutral, PublicKeyToken=null", "Inline all possible methods.", p);
			result.methods.Add(m);
			
			p = new ConfigParamsDef();
			p.configParamsDef.Add(new ConfigParamDef("level", "Level", 0, 5, "Optimization level."));
			p.configParamsDef.Add(new ConfigParamDef("clone", "Clone", true, true, "Use method clone."));
			p.configParamsDef.Add(new ConfigParamDef("cloneSensitivity", "Clone Sensitivity", (decimal)1.5, (decimal)1.0, "Method clone sensitivity."));
			m = new TransformMethod("inline-2.0-devel", "Inline", "2.0", "devel", "SolidOpt.InlineTransformer, SolidOpt, Version=2.0.4700.523, Culture=neutral, PublicKeyToken=null", "Inline all possible methods.", p);
			result.methods.Add(m);
			
			p = new ConfigParamsDef();
			p.configParamsDef.Add(new ConfigParamDef("propagationLevel", "Propagation Level", 0, 5, "Optimization level."));
			p.configParamsDef.Add(new ConfigParamDef("interMethodPropagation", "Inter-method propagation", true, true, "Make inter-method propagation of constants."));
			m = new TransformMethod("constantPropagation-1.0-devel", "Constant Propagation", "1.0", "devel", "SolidOpt.ConstantPropagationTransformer, SolidOpt, Version=1.0.1100.100, Culture=neutral, PublicKeyToken=null", "Make constant propagations.", p);
			result.methods.Add(m);
			
			p = new ConfigParamsDef();
			m = new TransformMethod("expressionSimplify-1.0-devel", "Expression simplify", "1.0", "devel", "Make expressions simplifications.", "SolidOpt.SimplifyExpressionTransformer, SolidOpt, Version=1.0.1100.100, Culture=neutral, PublicKeyToken=null", p);
			result.methods.Add(m);
			
			p = new ConfigParamsDef();
			p.configParamsDef.Add(new ConfigParamDef("methods", "Sub methods", new TransformMethods(), new TransformMethods(), "Composite container."));
			m = new TransformMethod("composite-1.0-stable", "Composite", "1.0", "stable", "SolidOpt.CompositeTransformer, SolidOpt, Version=1.0.2100.200, Culture=neutral, PublicKeyToken=null", "Composite sequence from optimization methods.", p);
			result.methods.Add(m);
			
			p = new ConfigParamsDef();
			p.configParamsDef.Add(new ConfigParamDef("descending", "Descending order", false, false, "Handle methods descending."));
			p.configParamsDef.Add(new ConfigParamDef("methods", "Sub methods", new TransformMethods(), new TransformMethods(), "Composite container."));
			m = new TransformMethod("compositeII-1.0-stable", "Composite II", "1.0", "stable", "SolidOpt.TwoWayCompositeTransformer, SolidOpt, Version=1.0.2100.200, Culture=neutral, PublicKeyToken=null", "Two way composite sequence from optimization methods.", p);
			result.methods.Add(m);
			
			return result;
		}
		
		/// <summary>
		/// Start new optimization
		/// </summary>
		[WebMethod(Description = "Start new optimization.", EnableSession=true)]
		public void NewOptimization()
		{
			if (Session["opt"] == null) {
				Session["opt"] = new MySession();
			} else {
				((MySession)Session["opt"]).optFiles.Clear();
				((MySession)Session["opt"]).resultFiles.Clear();
			}
		}
		
		/// <summary>
		/// Add URI to optimization files.
		/// </summary>
		[WebMethod(Description = "Add URI to optimization files.", EnableSession=true)]
		public bool AddOptimizationURI(string uri)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			
			byte[] b = x.ComputeHash(System.Text.Encoding.UTF8.GetBytes(uri));
			string fi = string.Format("/var/www/solidopt.org-ws/work/in/{0}", BitConverter.ToString(b).Replace("-", ""));
			//string fi = "/var/www/solidopt.org-ws/work/in/" + Convert.ToBase64String(b);
			
			// URIManager um = new URIManager();
			// um.Importers.Add(new FileImporter());
			// Stream s = um.GetResource(new Uri(uri));
			// FileStream r = fi.OpenWrite();
			// CopyStream(s, r);
			// s.Close();
			// r.Close();
			
			System.Net.WebClient client = new System.Net.WebClient();
			byte[] data = client.DownloadData(uri);
			FileStream r = new FileStream(fi, FileMode.Create);
			r.Write(data, 0, data.Length);
			r.Close();
			
			if (Session["opt"] == null) Session["opt"] = new MySession();
			((MySession)Session["opt"]).optFiles.Add(fi);
			
			return true;
		}
		
		/// <summary>
		/// Add binary data to optimization files.
		/// </summary>
		[WebMethod(Description = "Add binary data to optimization files.", EnableSession=true)]
		public bool AddOptimizationFile(byte[] binary)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] b = x.ComputeHash(binary);
			string fi = string.Format("/var/www/solidopt.org-ws/work/in/{0}", BitConverter.ToString(b).Replace("-", ""));
			//string fi = "/var/www/solidopt.org-ws/work/in/" + Convert.ToBase64String(b);

			
			FileStream r = new FileStream(fi, FileMode.Create);
			r.Write(binary, 0, binary.Length);
			r.Close();
			
			if (Session["opt"] == null) Session["opt"] = new MySession();
			((MySession)Session["opt"]).optFiles.Add(fi);
			
			return true;
		}
		
		/// <summary>
		/// Set optimization methods params.
		/// </summary>
		[WebMethod(Description = "Set optimization methods params.", EnableSession=true)]
		public bool SetOptimizationParams(ConfigParams config)
		{
			if (Session["opt"] == null) Session["opt"] = new MySession();
			((MySession)Session["opt"]).config = config;
			
			return true;
		}
		
		/// <summary>
		/// Optimize files.
		/// </summary>
		[WebMethod(Description = "Optimize files.", EnableSession=true)]
		public int Optimize()
		{
			int result = 0;
			
			if (Session["opt"] == null) Session["opt"] = new MySession();
			
			((MySession)Session["opt"]).resultFiles.Clear();
			foreach (string fi in ((MySession)Session["opt"]).optFiles) {
				FileStream s = new FileStream(fi, FileMode.Open);
				string fiResult = "/var/www/solidopt.org-ws/work/out/" + Path.GetFileName(fi);
				FileStream r = new FileStream(fiResult, FileMode.Create);
				CopyStream(s, r);
				s.Close();
				r.Close();
				((MySession)Session["opt"]).resultFiles.Add(fiResult);
				result++;
			}
			
			return result;
		}
		
		/// <summary>
		/// Get optimization results.
		/// </summary>
		[WebMethod(Description = "Get optimization results.", EnableSession=true)]
		public string GetResultURI(int result)
		{
			return "http://www.solidopt.org/ws/work/out/" + Path.GetFileName(((MySession)Session["opt"]).resultFiles[result]);
		}
		
		/// <summary>
		/// UserName of the logged in user.
		/// </summary>
		private string UserName {
			get {return (string)Context.Session["User"];}
			set {Context.Session["User"] = value;}
		}
		
		internal static void CopyStream(Stream source, Stream dest)
		{
			byte[] buffer = new byte[65536];
			int read;
			do {
				read = source.Read(buffer, 0, buffer.Length);
				dest.Write(buffer, 0, read);
			} while (read != 0);
		}
		
		internal class MySession
		{
			public ConfigParams config = new ConfigParams();
			public List<string> optFiles = new List<string>();
			public List<string> resultFiles = new List<string>();
		}
	}
}
