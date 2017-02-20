﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// Original file name:
// Generation date: 4/7/2016 9:56:50 AM
namespace NCrawler.DbServices
{
    
    /// <summary>
    /// There are no comments for NCrawlerEntitiesDbServices in the schema.
    /// </summary>
    public partial class NCrawlerEntitiesDbServices : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Initializes a new NCrawlerEntitiesDbServices object using the connection string found in the 'NCrawlerEntitiesDbServices' section of the application configuration file.
        /// </summary>
        public NCrawlerEntitiesDbServices() : 
                base("name=NCrawlerEntitiesDbServices", "NCrawlerEntitiesDbServices")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new NCrawlerEntitiesDbServices object.
        /// </summary>
        public NCrawlerEntitiesDbServices(string connectionString) : 
                base(connectionString, "NCrawlerEntitiesDbServices")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new NCrawlerEntitiesDbServices object.
        /// </summary>
        public NCrawlerEntitiesDbServices(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "NCrawlerEntitiesDbServices")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for CrawlHistory in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Data.Objects.ObjectQuery<CrawlHistory> CrawlHistory
        {
            get
            {
                if ((this._CrawlHistory == null))
                {
                    this._CrawlHistory = base.CreateQuery<CrawlHistory>("[CrawlHistory]");
                }
                return this._CrawlHistory;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Data.Objects.ObjectQuery<CrawlHistory> _CrawlHistory;
        /// <summary>
        /// There are no comments for CrawlQueue in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Data.Objects.ObjectQuery<CrawlQueue> CrawlQueue
        {
            get
            {
                if ((this._CrawlQueue == null))
                {
                    this._CrawlQueue = base.CreateQuery<CrawlQueue>("[CrawlQueue]");
                }
                return this._CrawlQueue;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Data.Objects.ObjectQuery<CrawlQueue> _CrawlQueue;
        /// <summary>
        /// There are no comments for CrawlHistory in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToCrawlHistory(CrawlHistory crawlHistory)
        {
            base.AddObject("CrawlHistory", crawlHistory);
        }
        /// <summary>
        /// There are no comments for CrawlQueue in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToCrawlQueue(CrawlQueue crawlQueue)
        {
            base.AddObject("CrawlQueue", crawlQueue);
        }
    }
    /// <summary>
    /// There are no comments for NCrawlerModel.CrawlHistory in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NCrawlerModel", Name="CrawlHistory")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class CrawlHistory : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new CrawlHistory object.
        /// </summary>
        /// <param name="id">Initial value of Id.</param>
        /// <param name="key">Initial value of Key.</param>
        /// <param name="groupId">Initial value of GroupId.</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static CrawlHistory CreateCrawlHistory(int id, string key, int groupId)
        {
            CrawlHistory crawlHistory = new CrawlHistory();
            crawlHistory.Id = id;
            crawlHistory.Key = key;
            crawlHistory.GroupId = groupId;
            return crawlHistory;
        }
        /// <summary>
        /// There are no comments for property Id in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// There are no comments for property Key in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this.OnKeyChanging(value);
                this.ReportPropertyChanging("Key");
                this._Key = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Key");
                this.OnKeyChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Key;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnKeyChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnKeyChanged();
        /// <summary>
        /// There are no comments for property GroupId in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int GroupId
        {
            get
            {
                return this._GroupId;
            }
            set
            {
                this.OnGroupIdChanging(value);
                this.ReportPropertyChanging("GroupId");
                this._GroupId = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("GroupId");
                this.OnGroupIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _GroupId;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnGroupIdChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnGroupIdChanged();
    }
    /// <summary>
    /// There are no comments for NCrawlerModel.CrawlQueue in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NCrawlerModel", Name="CrawlQueue")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class CrawlQueue : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new CrawlQueue object.
        /// </summary>
        /// <param name="id">Initial value of Id.</param>
        /// <param name="groupId">Initial value of GroupId.</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static CrawlQueue CreateCrawlQueue(int id, int groupId)
        {
            CrawlQueue crawlQueue = new CrawlQueue();
            crawlQueue.Id = id;
            crawlQueue.GroupId = groupId;
            return crawlQueue;
        }
        /// <summary>
        /// There are no comments for property Id in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// There are no comments for property GroupId in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int GroupId
        {
            get
            {
                return this._GroupId;
            }
            set
            {
                this.OnGroupIdChanging(value);
                this.ReportPropertyChanging("GroupId");
                this._GroupId = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("GroupId");
                this.OnGroupIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _GroupId;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnGroupIdChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnGroupIdChanged();
        /// <summary>
        /// There are no comments for property SerializedData in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public byte[] SerializedData
        {
            get
            {
                return global::System.Data.Objects.DataClasses.StructuralObject.GetValidValue(this._SerializedData);
            }
            set
            {
                this.OnSerializedDataChanging(value);
                this.ReportPropertyChanging("SerializedData");
                this._SerializedData = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("SerializedData");
                this.OnSerializedDataChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private byte[] _SerializedData;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSerializedDataChanging(byte[] value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSerializedDataChanged();
    }
}
