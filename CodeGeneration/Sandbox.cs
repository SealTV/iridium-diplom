using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Tech.CodeGeneration.Internals;

namespace Tech.CodeGeneration
{
    public sealed class Sandbox : IDisposable
    {
        public Sandbox(params IPermission[] additionalPermissions)
            : this(Environment.CurrentDirectory, additionalPermissions)
        {
        }


        public Sandbox(string applicationBase, params IPermission[] additionalPermissions)
        {
            applicationBase = Path.GetFullPath(applicationBase);
            var appDomainSetup = new AppDomainSetup { ApplicationBase = applicationBase };

            var permissions = CreatePermissionSet(applicationBase, additionalPermissions);

            var appDomainName = "Sandbox_" + Guid.NewGuid();
            _appDomain = AppDomain.CreateDomain(appDomainName, null, appDomainSetup, permissions);
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                AppDomain.Unload(_appDomain);

                _loadedAssemblies.ForEach(File.Delete);
                _loadedAssemblies.Clear();

                _disposed = true;
            }
        }
        
        internal string ApplicationBase
        {
            get { return _appDomain.SetupInformation.ApplicationBase; }
        }


        internal CodeProxy CreateCodeProxy(string asmLocation)
        {
            var type = typeof(CodeProxy);

//             using (var file = new FileStream("C:/ouput1.txt", FileMode.CreateNew))
//                {
//                    using (var writer = new StreamWriter(file))
//                    {
//                        writer.WriteLine(type.Assembly.Location);
//                        writer.WriteLine(asmLocation);
////                        foreach (var referencedAssembly in referencedAssemblies)
////                        {
////                            writer.WriteLine(referencedAssembly);
////                        }
//                    }
//                }
            var marshaler = (CodeProxy)_appDomain.CreateInstanceFromAndUnwrap(
                       assemblyFile: type.Assembly.Location,
                           typeName: type.FullName,
                         ignoreCase: false,
                        bindingAttr: BindingFlags.Default,
                             binder: null,
                               args: new object[] { asmLocation },
                            culture: null,
               activationAttributes: null);

            _loadedAssemblies.Add(asmLocation);

            return marshaler;
        }

        internal CodeProxy CreateCodeProxy(string asmLocation, string mainMethodName)
        {
            var type = typeof(CodeProxy);
            var k = _appDomain.CreateInstanceFromAndUnwrap(
                       assemblyFile: type.Assembly.Location,
                           typeName: type.FullName,
                         ignoreCase: false,
                        bindingAttr: BindingFlags.Default,
                             binder: null,
                               args: new object[] { asmLocation, mainMethodName },
                            culture: null,
               activationAttributes: null);
            var marshaler = (CodeProxy)k;
            

            _loadedAssemblies.Add(asmLocation);

            return marshaler;
        }


        private static PermissionSet CreatePermissionSet(string applicationBase, 
            params IPermission[] additionalPermissions)
        {
            var permissions = new PermissionSet(null);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));

            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read
                | FileIOPermissionAccess.PathDiscovery, applicationBase));

            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read
                | FileIOPermissionAccess.PathDiscovery, typeof(Sandbox).Assembly.Location));

            foreach (var additionalPermission in additionalPermissions)
                permissions.AddPermission(additionalPermission);
            
            return permissions;
        }


        private bool _disposed;
        private readonly AppDomain _appDomain;
        private readonly List<string> _loadedAssemblies = new List<string>();
    }
}
