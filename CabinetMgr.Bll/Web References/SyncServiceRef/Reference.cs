//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 

using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;

#pragma warning disable 1591

namespace CabinetMgr.Bll.Web_References.SyncServiceRef {
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [WebServiceBinding(Name="SyncServiceSoap", Namespace="http://jhy.org/")]
    public partial class SyncService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback DownloadDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetServerTimeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SyncService() {
            this.Url = global::CabinetMgr.Bll.Properties.Settings.Default.CabinetBll_SyncServiceRef_SyncService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event DownloadDataCompletedEventHandler DownloadDataCompleted;
        
        /// <remarks/>
        public event UploadDataCompletedEventHandler UploadDataCompleted;
        
        /// <remarks/>
        public event GetServerTimeCompletedEventHandler GetServerTimeCompleted;
        
        /// <remarks/>
        [SoapDocumentMethod("http://jhy.org/DownloadData", RequestNamespace="http://jhy.org/", ResponseNamespace="http://jhy.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet DownloadData(string typeName, System.DateTime lastSync) {
            object[] results = this.Invoke("DownloadData", new object[] {
                        typeName,
                        lastSync});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void DownloadDataAsync(string typeName, System.DateTime lastSync) {
            this.DownloadDataAsync(typeName, lastSync, null);
        }
        
        /// <remarks/>
        public void DownloadDataAsync(string typeName, System.DateTime lastSync, object userState) {
            if ((this.DownloadDataOperationCompleted == null)) {
                this.DownloadDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDownloadDataOperationCompleted);
            }
            this.InvokeAsync("DownloadData", new object[] {
                        typeName,
                        lastSync}, this.DownloadDataOperationCompleted, userState);
        }
        
        private void OnDownloadDataOperationCompleted(object arg) {
            if ((this.DownloadDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DownloadDataCompleted(this, new DownloadDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [SoapDocumentMethod("http://jhy.org/UploadData", RequestNamespace="http://jhy.org/", ResponseNamespace="http://jhy.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int UploadData(string typeName, System.Data.DataSet infoSet, string dataOwner) {
            object[] results = this.Invoke("UploadData", new object[] {
                        typeName,
                        infoSet,
                        dataOwner});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void UploadDataAsync(string typeName, System.Data.DataSet infoSet, string dataOwner) {
            this.UploadDataAsync(typeName, infoSet, dataOwner, null);
        }
        
        /// <remarks/>
        public void UploadDataAsync(string typeName, System.Data.DataSet infoSet, string dataOwner, object userState) {
            if ((this.UploadDataOperationCompleted == null)) {
                this.UploadDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadDataOperationCompleted);
            }
            this.InvokeAsync("UploadData", new object[] {
                        typeName,
                        infoSet,
                        dataOwner}, this.UploadDataOperationCompleted, userState);
        }
        
        private void OnUploadDataOperationCompleted(object arg) {
            if ((this.UploadDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadDataCompleted(this, new UploadDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [SoapDocumentMethod("http://jhy.org/GetServerTime", RequestNamespace="http://jhy.org/", ResponseNamespace="http://jhy.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetServerTime() {
            object[] results = this.Invoke("GetServerTime", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetServerTimeAsync() {
            this.GetServerTimeAsync(null);
        }
        
        /// <remarks/>
        public void GetServerTimeAsync(object userState) {
            if ((this.GetServerTimeOperationCompleted == null)) {
                this.GetServerTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetServerTimeOperationCompleted);
            }
            this.InvokeAsync("GetServerTime", new object[0], this.GetServerTimeOperationCompleted, userState);
        }
        
        private void OnGetServerTimeOperationCompleted(object arg) {
            if ((this.GetServerTimeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetServerTimeCompleted(this, new GetServerTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void DownloadDataCompletedEventHandler(object sender, DownloadDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class DownloadDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DownloadDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void UploadDataCompletedEventHandler(object sender, UploadDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class UploadDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void GetServerTimeCompletedEventHandler(object sender, GetServerTimeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    public partial class GetServerTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetServerTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591