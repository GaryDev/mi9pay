using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Mi9Pay.Web.Helpers
{
    public class AssemblyHelper
    {
        #region Private Fields

        private Assembly _assembly;

        #endregion

        #region Constructors

        public AssemblyHelper(Assembly assembly)
        {
            _assembly = assembly;
        }

        #endregion

        #region Public Properties

        public string Company { get { return GetExecutingAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company); } }
        public string Product { get { return GetExecutingAssemblyAttribute<AssemblyProductAttribute>(a => a.Product); } }
        public string Copyright { get { return GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright); } }
        public string Trademark { get { return GetExecutingAssemblyAttribute<AssemblyTrademarkAttribute>(a => a.Trademark); } }
        public string Title { get { return GetExecutingAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title); } }
        public string Description { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
        public string Configuration { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
        public string FileVersion { get { return GetExecutingAssemblyAttribute<AssemblyFileVersionAttribute>(a => a.Version); } }

        public Version Version { get { return _assembly.GetName().Version; } }
        public string VersionFull { get { return Version.ToString(); } }
        public string VersionMajor { get { return Version.Major.ToString(); } }
        public string VersionMinor { get { return Version.Minor.ToString(); } }
        public string VersionBuild { get { return Version.Build.ToString(); } }
        public string VersionRevision { get { return Version.Revision.ToString(); } }

        #endregion

        #region Private Members

        private string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(_assembly, typeof(T));
            return value.Invoke(attribute);
        }

        #endregion
    }
}