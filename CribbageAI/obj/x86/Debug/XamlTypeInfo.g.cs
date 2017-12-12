﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace CribbageAI
{
    public partial class App : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
    private global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider _provider;

        /// <summary>
        /// GetXamlType(Type)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            if(_provider == null)
            {
                _provider = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByType(type);
        }

        /// <summary>
        /// GetXamlType(String)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            if(_provider == null)
            {
                _provider = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByName(fullName);
        }

        /// <summary>
        /// GetXmlnsDefinitions()
        /// </summary>
        public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new global::Windows.UI.Xaml.Markup.XmlnsDefinition[0];
        }
    }
}

namespace CribbageAI.CribbageAI_XamlTypeInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByType.TryGetValue(type, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByType(type);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByName.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByName(typeName);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (string.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByName = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>
                _xamlMembers = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>();

        string[] _typeNameTable = null;
        global::System.Type[] _typeTable = null;

        private void InitTypeTables()
        {
            _typeNameTable = new string[14];
            _typeNameTable[0] = "CribbageAI.ObjectToObjectValueConverter";
            _typeNameTable[1] = "Object";
            _typeNameTable[2] = "CribbageAI.ScoreIntToStringConverter";
            _typeNameTable[3] = "CribbageAI.IntToStringConverter";
            _typeNameTable[4] = "CribbageAI.NullToBoolConverter";
            _typeNameTable[5] = "CribbageAI.StorageFileToString";
            _typeNameTable[6] = "CribbageAI.StringToImageSourceConverter";
            _typeNameTable[7] = "CribbageAI.BoolToVisibilityConverter";
            _typeNameTable[8] = "CribbageAI.StringToIntListConverter";
            _typeNameTable[9] = "CribbageAI.StringToImageBrushConverter";
            _typeNameTable[10] = "CribbageAI.NewMainPage";
            _typeNameTable[11] = "Windows.UI.Xaml.Controls.Page";
            _typeNameTable[12] = "Windows.UI.Xaml.Controls.UserControl";
            _typeNameTable[13] = "Double";

            _typeTable = new global::System.Type[14];
            _typeTable[0] = typeof(global::CribbageAI.ObjectToObjectValueConverter);
            _typeTable[1] = typeof(global::System.Object);
            _typeTable[2] = typeof(global::CribbageAI.ScoreIntToStringConverter);
            _typeTable[3] = typeof(global::CribbageAI.IntToStringConverter);
            _typeTable[4] = typeof(global::CribbageAI.NullToBoolConverter);
            _typeTable[5] = typeof(global::CribbageAI.StorageFileToString);
            _typeTable[6] = typeof(global::CribbageAI.StringToImageSourceConverter);
            _typeTable[7] = typeof(global::CribbageAI.BoolToVisibilityConverter);
            _typeTable[8] = typeof(global::CribbageAI.StringToIntListConverter);
            _typeTable[9] = typeof(global::CribbageAI.StringToImageBrushConverter);
            _typeTable[10] = typeof(global::CribbageAI.NewMainPage);
            _typeTable[11] = typeof(global::Windows.UI.Xaml.Controls.Page);
            _typeTable[12] = typeof(global::Windows.UI.Xaml.Controls.UserControl);
            _typeTable[13] = typeof(global::System.Double);
        }

        private int LookupTypeIndexByName(string typeName)
        {
            if (_typeNameTable == null)
            {
                InitTypeTables();
            }
            for (int i=0; i<_typeNameTable.Length; i++)
            {
                if(0 == string.CompareOrdinal(_typeNameTable[i], typeName))
                {
                    return i;
                }
            }
            return -1;
        }

        private int LookupTypeIndexByType(global::System.Type type)
        {
            if (_typeTable == null)
            {
                InitTypeTables();
            }
            for(int i=0; i<_typeTable.Length; i++)
            {
                if(type == _typeTable[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private object Activate_0_ObjectToObjectValueConverter() { return new global::CribbageAI.ObjectToObjectValueConverter(); }
        private object Activate_2_ScoreIntToStringConverter() { return new global::CribbageAI.ScoreIntToStringConverter(); }
        private object Activate_3_IntToStringConverter() { return new global::CribbageAI.IntToStringConverter(); }
        private object Activate_4_NullToBoolConverter() { return new global::CribbageAI.NullToBoolConverter(); }
        private object Activate_5_StorageFileToString() { return new global::CribbageAI.StorageFileToString(); }
        private object Activate_6_StringToImageSourceConverter() { return new global::CribbageAI.StringToImageSourceConverter(); }
        private object Activate_7_BoolToVisibilityConverter() { return new global::CribbageAI.BoolToVisibilityConverter(); }
        private object Activate_8_StringToIntListConverter() { return new global::CribbageAI.StringToIntListConverter(); }
        private object Activate_9_StringToImageBrushConverter() { return new global::CribbageAI.StringToImageBrushConverter(); }
        private object Activate_10_NewMainPage() { return new global::CribbageAI.NewMainPage(); }

        private global::Windows.UI.Xaml.Markup.IXamlType CreateXamlType(int typeIndex)
        {
            global::CribbageAI.CribbageAI_XamlTypeInfo.XamlSystemBaseType xamlType = null;
            global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType userType;
            string typeName = _typeNameTable[typeIndex];
            global::System.Type type = _typeTable[typeIndex];

            switch (typeIndex)
            {

            case 0:   //  CribbageAI.ObjectToObjectValueConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_0_ObjectToObjectValueConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 1:   //  Object
                xamlType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 2:   //  CribbageAI.ScoreIntToStringConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_2_ScoreIntToStringConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 3:   //  CribbageAI.IntToStringConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_3_IntToStringConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 4:   //  CribbageAI.NullToBoolConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_4_NullToBoolConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 5:   //  CribbageAI.StorageFileToString
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_5_StorageFileToString;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 6:   //  CribbageAI.StringToImageSourceConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_6_StringToImageSourceConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 7:   //  CribbageAI.BoolToVisibilityConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_7_BoolToVisibilityConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 8:   //  CribbageAI.StringToIntListConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_8_StringToIntListConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 9:   //  CribbageAI.StringToImageBrushConverter
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_9_StringToImageBrushConverter;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 10:   //  CribbageAI.NewMainPage
                userType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_10_NewMainPage;
                userType.AddMemberName("PlayerOneWinPercent");
                userType.AddMemberName("PlayerTwoWinPercent");
                userType.AddMemberName("AverageScorePlayerOne");
                userType.AddMemberName("AverageScorePlayerTwo");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 11:   //  Windows.UI.Xaml.Controls.Page
                xamlType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 12:   //  Windows.UI.Xaml.Controls.UserControl
                xamlType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 13:   //  Double
                xamlType = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;
            }
            return xamlType;
        }


        private object get_0_NewMainPage_PlayerOneWinPercent(object instance)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            return that.PlayerOneWinPercent;
        }
        private void set_0_NewMainPage_PlayerOneWinPercent(object instance, object Value)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            that.PlayerOneWinPercent = (global::System.Double)Value;
        }
        private object get_1_NewMainPage_PlayerTwoWinPercent(object instance)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            return that.PlayerTwoWinPercent;
        }
        private void set_1_NewMainPage_PlayerTwoWinPercent(object instance, object Value)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            that.PlayerTwoWinPercent = (global::System.Double)Value;
        }
        private object get_2_NewMainPage_AverageScorePlayerOne(object instance)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            return that.AverageScorePlayerOne;
        }
        private void set_2_NewMainPage_AverageScorePlayerOne(object instance, object Value)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            that.AverageScorePlayerOne = (global::System.Double)Value;
        }
        private object get_3_NewMainPage_AverageScorePlayerTwo(object instance)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            return that.AverageScorePlayerTwo;
        }
        private void set_3_NewMainPage_AverageScorePlayerTwo(object instance, object Value)
        {
            var that = (global::CribbageAI.NewMainPage)instance;
            that.AverageScorePlayerTwo = (global::System.Double)Value;
        }

        private global::Windows.UI.Xaml.Markup.IXamlMember CreateXamlMember(string longMemberName)
        {
            global::CribbageAI.CribbageAI_XamlTypeInfo.XamlMember xamlMember = null;
            global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType userType;

            switch (longMemberName)
            {
            case "CribbageAI.NewMainPage.PlayerOneWinPercent":
                userType = (global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType)GetXamlTypeByName("CribbageAI.NewMainPage");
                xamlMember = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlMember(this, "PlayerOneWinPercent", "Double");
                xamlMember.Getter = get_0_NewMainPage_PlayerOneWinPercent;
                xamlMember.Setter = set_0_NewMainPage_PlayerOneWinPercent;
                break;
            case "CribbageAI.NewMainPage.PlayerTwoWinPercent":
                userType = (global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType)GetXamlTypeByName("CribbageAI.NewMainPage");
                xamlMember = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlMember(this, "PlayerTwoWinPercent", "Double");
                xamlMember.Getter = get_1_NewMainPage_PlayerTwoWinPercent;
                xamlMember.Setter = set_1_NewMainPage_PlayerTwoWinPercent;
                break;
            case "CribbageAI.NewMainPage.AverageScorePlayerOne":
                userType = (global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType)GetXamlTypeByName("CribbageAI.NewMainPage");
                xamlMember = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlMember(this, "AverageScorePlayerOne", "Double");
                xamlMember.Getter = get_2_NewMainPage_AverageScorePlayerOne;
                xamlMember.Setter = set_2_NewMainPage_AverageScorePlayerOne;
                break;
            case "CribbageAI.NewMainPage.AverageScorePlayerTwo":
                userType = (global::CribbageAI.CribbageAI_XamlTypeInfo.XamlUserType)GetXamlTypeByName("CribbageAI.NewMainPage");
                xamlMember = new global::CribbageAI.CribbageAI_XamlTypeInfo.XamlMember(this, "AverageScorePlayerTwo", "Double");
                xamlMember.Getter = get_3_NewMainPage_AverageScorePlayerTwo;
                xamlMember.Setter = set_3_NewMainPage_AverageScorePlayerTwo;
                break;
            }
            return xamlMember;
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : global::Windows.UI.Xaml.Markup.IXamlType
    {
        string _fullName;
        global::System.Type _underlyingType;

        public XamlSystemBaseType(string fullName, global::System.Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public global::System.Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name) { throw new global::System.NotImplementedException(); }
        virtual public bool IsArray { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsReturnTypeStub { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsLocalType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType ItemType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType KeyType { get { throw new global::System.NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new global::System.NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void RunInitializer()   { throw new global::System.NotImplementedException(); }
        virtual public object CreateFromString(string input)   { throw new global::System.NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : global::CribbageAI.CribbageAI_XamlTypeInfo.XamlSystemBaseType
    {
        global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider _provider;
        global::Windows.UI.Xaml.Markup.IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;
        bool _isReturnTypeStub;
        bool _isLocalType;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        global::System.Collections.Generic.Dictionary<string, string> _memberNames;
        global::System.Collections.Generic.Dictionary<string, object> _enumValues;

        public XamlUserType(global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider provider, string fullName, global::System.Type fullType, global::Windows.UI.Xaml.Markup.IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }
        override public bool IsReturnTypeStub { get { return _isReturnTypeStub; } }
        override public bool IsLocalType { get { return _isLocalType; } }

        override public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public object CreateFromString(string input)
        {
            if (_enumValues != null)
            {
                int value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    int enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = global::System.Convert.ToInt32(valuePart.Trim());
                            }
                            catch( global::System.FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( string.Compare(valuePart.Trim(), key, global::System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( global::System.FormatException )
                    {
                        throw new global::System.ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new global::System.ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetIsReturnTypeStub()
        {
            _isReturnTypeStub = true;
        }

        public void SetIsLocalType()
        {
            _isLocalType = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new global::System.Collections.Generic.Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : global::Windows.UI.Xaml.Markup.IXamlMember
    {
        global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(global::CribbageAI.CribbageAI_XamlTypeInfo.XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public global::Windows.UI.Xaml.Markup.IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(string targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public global::Windows.UI.Xaml.Markup.IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new global::System.InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new global::System.InvalidOperationException("SetValue");
        }
    }
}

