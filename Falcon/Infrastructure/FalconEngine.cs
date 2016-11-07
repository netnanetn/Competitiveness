using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using Falcon.Configuration;
using Falcon.Infrastructure.DependencyManagement;
using Falcon.Tasks;
using System.Web;

using Common.Logging;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Falcon.Infrastructure
{
    public class FalconEngine
    {
        #region Fields
        private ILog log = LogManager.GetCurrentClassLogger();

        private ContainerManager _containerManager;
        private FalconConfig _falconConfig;
        private readonly FalconLicense _falconLicense;
        private bool? _cacheLicense;

        private string _privateLicenseKey = @"<RSAKeyValue><Modulus>mzQI95i8yHqmd9B3YHHFk36LJayV/gK/B7tiDCdxI4ekzREkNvYJlWBECYovs4JmqNC2nXwwmSK6mUMO30myK9P5KlDgj6vAm55HKYMacao6qduxEpbx7Saj3XmK21aPxaF73/C6re/YtS5ISYG2XPrwrZonZib+4kUh/m8cZ/U=</Modulus><Exponent>AQAB</Exponent><P>yyNwl46rOFEbGseuhHGi5qzNEd/qvwnxIXONTzEaWwwMqS32OJKtr2UIXTjUFZhLHabzE6osdQhkwkKrFLwElw==</P><Q>w5dG80llz3fd4dV7xrLcr/T806CB6NPKvRnTUs4vWBQmKlcD+dBOMVJ8aCppHlSxwTUGbICXTyGTiLqq+pbNUw==</Q><DP>p/B7tACSEzQSaXhuEjb0SyrPOxjYtOmePV6Pk8nvlRgIG5HQ/oJdLyUi3hcOV3AZocEVMsu8h2DSFDwTvkeWrw==</DP><DQ>Y3ca7b31uHwMqjwWpgVvlUvRBw4pAm4yO9hWT1XrXa9heUanDvOP0CVSfuaSbopDmy2MBsGeMO+yR2YmbSkjYQ==</DQ><InverseQ>FRkWHcKqIPYVAvA/gKtJB3FWYrZeGuR/ZcQgvyxkiLEl6wcrn6hpAfRcJDr3c1Rh2Or7CYwJATwPov0cEYREzg==</InverseQ><D>RYiDCfamgYUdpqlBinZ+Z622horzailtz3d+nmZfVGU1iuBGos8JDSDjaoi8BUcDWlejsRvOJBWAlyCJfbUlOrXqG/6oGKC6VeTq5hp6uFlaUOCZ/AWIPOoIlSb2cynPV2yqVFS6Ml5/5pHgKGqDMWlzRxsrac8wh6EXok3D4VM=</D></RSAKeyValue>";
        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of the content engine using default settings and configuration.
        /// </summary>
        public FalconEngine() 
            : this(EventBroker.Instance, new ContainerConfigurer())
        {
        }

        public FalconEngine(EventBroker broker, ContainerConfigurer configurer)
        {
            _falconConfig = ConfigurationManager.GetSection("FalconConfig") as FalconConfig;
            //Validate Licence
            _falconLicense = ReadLicenseInfo();            
            InitializeContainer(configurer, broker);          
        }
        
        #endregion

        #region Verify RegistrationKey
        /// <summary>
        /// Kiểm tra licence có hợp lệ hay không
        /// </summary>
        /// <returns></returns>
        public bool CheckLicense()
        {
            var context = HttpContext.Current;
            if (context == null || _falconLicense == null)
            {
                return false;
            }
            try 
	        {
                if (_cacheLicense == null)
                {
                    string hostName = GetServerName(context.Request);
                    string hostAddress = GetRemoteIP(context.Request);
                    if (_IsAllowedIp(hostAddress))
                    {
                        bool hostNameIsValid = false;
                        foreach (var domain in _falconLicense.Domain)
                        {
                            hostNameIsValid = Regex.IsMatch(hostName, domain,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled);
                            if (hostNameIsValid) break;
                        }

                        //check domain
                        if (!hostNameIsValid)
                        {
                            _cacheLicense = false;
                            return false;
                        }
                    }
                    else
                    {
                        _cacheLicense = false;
                        return false;
                    }
                   
                    _cacheLicense = true;
                }
                else
                {
                    if (_cacheLicense == false)
                    {
                        return false;
                    }
                }

                // check expiration date if configured
                if (_falconLicense.ExpiryDate != DateTime.MaxValue && DateTime.Now > _falconLicense.ExpiryDate)
                {
                    log.Fatal("License expired!");
                    return false;
                }

                return true;
	        }
	        catch (Exception e)
	        {
                log.Fatal("Error Checking License: " + e.Message);
                return false;
	        }
        }

        private bool _IsAllowedIp(string hostAddress)
        {
            return !_falconLicense.IP.Any() || _falconLicense.IP.Contains(hostAddress) || _IsPrivateIp(hostAddress);
        }

        private bool _IsPrivateIp(string hostAddress)
        {
            return hostAddress.Equals("127.0.0.1") || hostAddress.Equals("::1") || hostAddress.StartsWith("192.168.") ||
                   hostAddress.StartsWith("10.");
        }
      
        /// <summary>
        ///  Verify RegistrationKey to prevent unauthorized persons want to use this Framework
        ///  license format:
        ///  <FalconLicense><Domain>www.hangtot.com hangtot.com</Domain><IP>1.2.3.4 5.6.7.8</IP><ExpiryDate>yyyy/MM/dd hh:mm:ss</ExpiryDate></FalconLicense>
        /// </summary>
        /// <returns></returns>
        private FalconLicense ReadLicenseInfo()
        {
            var result = new FalconLicense();

            string regKey = _falconConfig.RegistrationKey;

            if (string.IsNullOrWhiteSpace(regKey))
            {
                log.Fatal("Invalid license");
            }
            else
            {
                try
                {
                    string license = DecryptString(regKey, 1024, _privateLicenseKey);

                    var xmlLicense = new XmlDocument();
                    xmlLicense.LoadXml(license);

                    var falconLicenseNode = xmlLicense.SelectSingleNode("FalconLicense");
                    if (falconLicenseNode != null)
                    {
                        //domain được viết cách nhau bởi dấu cách hoặc mỗi domain trên 1 dòng. 
                        //Ví dụ: www.hangtot.com hangtot.com
                        var domainNode = falconLicenseNode.SelectSingleNode("Domain");
                        if (domainNode != null)
                        {
                            result.Domain = domainNode.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            var ipNode = falconLicenseNode.SelectSingleNode("IP");
                            if (ipNode != null)
                            {
                                result.IP = ipNode.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            }

                            var expiryDateNode = falconLicenseNode.SelectSingleNode("ExpiryDate");
                            if (expiryDateNode != null && !string.IsNullOrWhiteSpace(expiryDateNode.InnerText))
                            {
                                result.ExpiryDate = Convert.ToDateTime(expiryDateNode.InnerText, CultureInfo.CreateSpecificCulture("en-us").DateTimeFormat);
                            }

                            log.Info("License Info: Domain: " + String.Join(", ", result.Domain) + "; IP: " + String.Join(", ", result.IP) + "; Expiration date: " + result.ExpiryDate);
                        }
                        else
                        {
                            log.Fatal("Invalid license");
                        }    
                    }
                    else
                    {
                        log.Fatal("Invalid license");
                    }
                }
                catch (Exception e)
                {
                    log.Fatal("Error reading license:" + e.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// Hàm thực hiện giải mã dữ liệu theo thuật toán RSA
        /// Tham khảo: http://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET
        /// </summary>
        /// <param name="inputString">chuỗi cần giải mã</param>
        /// <param name="dwKeySize">kích thước mã hóa: 1024 bit, 2048 bit...</param>
        /// <param name="xmlString">chuỗi chứa private key</param>
        /// <returns></returns>
        private string DecryptString( string inputString, int dwKeySize, string xmlString )
		{
			// TODO: Add Proper Exception Handlers
			RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider( dwKeySize );
			rsaCryptoServiceProvider.FromXmlString( xmlString );
			int base64BlockSize = ( ( dwKeySize / 8 ) % 3 != 0 ) ? ( ( ( dwKeySize / 8 ) / 3 ) * 4 ) + 4 : ( ( dwKeySize / 8 ) / 3 ) * 4;
			int iterations = inputString.Length / base64BlockSize;
			ArrayList arrayList = new ArrayList();
			for( int i = 0; i < iterations; i++ )
			{
				byte[] encryptedBytes = Convert.FromBase64String( inputString.Substring( base64BlockSize * i, base64BlockSize ) );
				// Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
				// If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
				// Comment out the next line and the corresponding one in the EncryptString function.
				Array.Reverse( encryptedBytes );
				arrayList.AddRange( rsaCryptoServiceProvider.Decrypt( encryptedBytes, true ) );				
			}			
			return Encoding.UTF32.GetString( arrayList.ToArray( Type.GetType( "System.Byte" ) ) as byte[] );
		}
        #endregion

        #region Utilities

        private void InitPlugins()
        {
            //var bootstrapper = _containerManager.Resolve<IPluginBootstrapper>();
            //var plugins = bootstrapper.GetPluginDefinitions();
            //bootstrapper.InitializePlugins(this, plugins);
        }

        private void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
            {
                string clazz = startUpTask.GetType().FullName;

                log.Info(string.Format("Start Task {0}", clazz));

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); 
                
                startUpTask.Execute();

                stopwatch.Stop();


                log.Info(string.Format("End Task {0} cost {1}", clazz, stopwatch.Elapsed));
            }
                
        }

        //private void StartScheduledTasks()
        //{
        //    //initialize task manager
        //    if (FalconConfig.ScheduleTasks != null)
        //    {
        //        TaskManager.Instance.Initialize(FalconConfig.ScheduleTasks);
        //        TaskManager.Instance.Start();
        //    }
        //}

        private void InitializeContainer(ContainerConfigurer configurer, EventBroker broker)
        {
            var builder = new ContainerBuilder();
            
            _containerManager = new ContainerManager(builder.Build());
            
            configurer.Configure(this, _containerManager, broker, FalconConfig);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Initialize components and plugins in the falcon environment.
        /// </summary>
        public void Initialize()
        {
            //tam check dc license 
            if (_falconLicense.Domain.Count() > 0)
            {
                //plugins
                InitPlugins();

                //start components
                //this.ContainerManager.StartComponents();

                //startup tasks
                RunStartupTasks();

                //scheduled tasks
                //StartScheduledTasks();
            }
            else
            {
                log.Error("License Invalid, can not Initialize Startup Tasks");
            }    
        }
        /// <summary>
        /// Return FalconConfig section
        /// </summary>
        public FalconConfig FalconConfig
        {
            get
            {
                return _falconConfig;
            }			
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public T Resolve<T>(string named) where T : class
        {
            return ContainerManager.Resolve<T>(named);            
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public object ResolveOptional(Type type)
        {
            return ContainerManager.ResolveOptional(type);
        }

        public Array ResolveAll(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        public bool IsRegistered(Type type)
        {
            return ContainerManager.IsRegistered(type);
        }
        
        #endregion

        #region Properties

        public IContainer Container
        {
            get { return _containerManager.Container; }
        }

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
        #region IP & Domain manipulation
        /// <summary>
        /// When a client IP can't be determined
        /// </summary>
        private const string UnknownIP = "0.0.0.0";

        private readonly Regex _ipAddress = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}$",
                                                             RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// returns true if this is a private network IP  
        /// http://en.wikipedia.org/wiki/Private_network
        /// </summary>
        private bool IsPrivateIP(string s)
        {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }

        /// <summary>
        /// retrieves the IP address of the current request -- handles proxies and private networks
        /// </summary>
        private string GetRemoteIP(NameValueCollection ServerVariables)
        {
            string ip = ServerVariables["REMOTE_ADDR"]; // could be a proxy -- beware
            string ipForwarded = ServerVariables["HTTP_X_FORWARDED_FOR"];

            // check if we were forwarded from a proxy
            if (!string.IsNullOrEmpty(ipForwarded))
            {
                ipForwarded = _ipAddress.Match(ipForwarded).Value;
                if (!string.IsNullOrEmpty(ipForwarded) && !IsPrivateIP(ipForwarded))
                    ip = ipForwarded;
            }

            return !string.IsNullOrEmpty(ip) ? ip : UnknownIP;
        }

        /// <summary>
        /// Answers the current request's user's ip address; checks for any forwarding proxy
        /// </summary>
        private string GetRemoteIP(HttpRequest request)
        {
            return GetRemoteIP(request.ServerVariables);
        }

        /// <summary>
        /// Get Server name using for this web application
        /// </summary>
        /// <returns></returns>
        private string GetServerName(HttpRequest request)
        {
            return request.ServerVariables["SERVER_NAME"];
        }
        #endregion
    }
}
