﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GlobalTrackDesktop.GlobalTrackService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TrackableItem", Namespace="http://schemas.datacontract.org/2004/07/ClientDataModel")]
    [System.SerializableAttribute()]
    public partial class TrackableItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsSecuredField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private GlobalTrackDesktop.GlobalTrackService.TrackableItemState[] StatesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SupportsGeolocationServicesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SupportsUserInformationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSecured {
            get {
                return this.IsSecuredField;
            }
            set {
                if ((this.IsSecuredField.Equals(value) != true)) {
                    this.IsSecuredField = value;
                    this.RaisePropertyChanged("IsSecured");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public GlobalTrackDesktop.GlobalTrackService.TrackableItemState[] States {
            get {
                return this.StatesField;
            }
            set {
                if ((object.ReferenceEquals(this.StatesField, value) != true)) {
                    this.StatesField = value;
                    this.RaisePropertyChanged("States");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool SupportsGeolocationServices {
            get {
                return this.SupportsGeolocationServicesField;
            }
            set {
                if ((this.SupportsGeolocationServicesField.Equals(value) != true)) {
                    this.SupportsGeolocationServicesField = value;
                    this.RaisePropertyChanged("SupportsGeolocationServices");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool SupportsUserInformation {
            get {
                return this.SupportsUserInformationField;
            }
            set {
                if ((this.SupportsUserInformationField.Equals(value) != true)) {
                    this.SupportsUserInformationField = value;
                    this.RaisePropertyChanged("SupportsUserInformation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserId {
            get {
                return this.UserIdField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIdField, value) != true)) {
                    this.UserIdField = value;
                    this.RaisePropertyChanged("UserId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TrackableItemState", Namespace="http://schemas.datacontract.org/2004/07/ClientDataModel")]
    [System.SerializableAttribute()]
    public partial class TrackableItemState : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoginResponse", Namespace="http://schemas.datacontract.org/2004/07/GlobalTrackService")]
    [System.SerializableAttribute()]
    public partial class LoginResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SessionIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage {
            get {
                return this.ErrorMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true)) {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId {
            get {
                return this.SessionIdField;
            }
            set {
                if ((object.ReferenceEquals(this.SessionIdField, value) != true)) {
                    this.SessionIdField = value;
                    this.RaisePropertyChanged("SessionId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GlobalTrackService.IGlobalTrackServicev1")]
    public interface IGlobalTrackServicev1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGlobalTrackServicev1/GetTrackings", ReplyAction="http://tempuri.org/IGlobalTrackServicev1/GetTrackingsResponse")]
        ClientDataModel.Tracking[] GetTrackings(string sessionId, System.Nullable<System.DateTime> from, System.Nullable<System.DateTime> to, string searchString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGlobalTrackServicev1/GetTrackableItems", ReplyAction="http://tempuri.org/IGlobalTrackServicev1/GetTrackableItemsResponse")]
        GlobalTrackDesktop.GlobalTrackService.TrackableItem[] GetTrackableItems(string sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGlobalTrackServicev1/Login", ReplyAction="http://tempuri.org/IGlobalTrackServicev1/LoginResponse")]
        GlobalTrackDesktop.GlobalTrackService.LoginResponse Login(string userName, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGlobalTrackServicev1Channel : GlobalTrackDesktop.GlobalTrackService.IGlobalTrackServicev1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GlobalTrackServicev1Client : System.ServiceModel.ClientBase<GlobalTrackDesktop.GlobalTrackService.IGlobalTrackServicev1>, GlobalTrackDesktop.GlobalTrackService.IGlobalTrackServicev1 {
        
        public GlobalTrackServicev1Client() {
        }
        
        public GlobalTrackServicev1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GlobalTrackServicev1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GlobalTrackServicev1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GlobalTrackServicev1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ClientDataModel.Tracking[] GetTrackings(string sessionId, System.Nullable<System.DateTime> from, System.Nullable<System.DateTime> to, string searchString) {
            return base.Channel.GetTrackings(sessionId, from, to, searchString);
        }
        
        public GlobalTrackDesktop.GlobalTrackService.TrackableItem[] GetTrackableItems(string sessionId) {
            return base.Channel.GetTrackableItems(sessionId);
        }
        
        public GlobalTrackDesktop.GlobalTrackService.LoginResponse Login(string userName, string password) {
            return base.Channel.Login(userName, password);
        }
    }
}
