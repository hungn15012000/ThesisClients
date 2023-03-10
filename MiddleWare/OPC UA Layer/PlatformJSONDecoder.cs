using System.Reflection;
using MiddleWare.Exceptions;
using MiddleWare.Extensions;
using MiddleWare.OPCUALayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using Opc.Ua.Client;
using TypeInfo = Opc.Ua.TypeInfo;

namespace MiddleWare.OPC_UA_Layer
{
    public class PlatformJsonDecoder:  JsonDecoder
    {
        #region Private Fields
        private JsonTextReader m_reader;
        private Dictionary<string, object> m_root;
        private Stack<object> m_stack;
        private Session m_session;
        private NodeId m_currentDataType;
        private JObject m_currentJObject;
        private ServiceMessageContext m_context;
        #endregion

        #region Constructors
        
        private PlatformJsonDecoder(string json, NodeId dataTypeId, Session session, int[] dimensions): base (json, session.MessageContext)
        {
            if (session == null)
                throw new ArgumentNullException(nameof (session));
            this.Initialize();
            m_session = session;
            m_currentDataType = dataTypeId;
            m_currentJObject = JsonConvert.DeserializeObject<JObject>(json);
            m_context = session.MessageContext;
            m_reader = new JsonTextReader((TextReader) new StringReader(json));
            m_root = this.ReadObject();
            m_stack = new Stack<object>();
            m_stack.Push((object) this.m_root);
            Dimensions = dimensions;
        }
        
        private void Initialize()
        {
            m_reader = (JsonTextReader) null;
        }

        #endregion

        public int[] Dimensions { get; set; }

        public static PlatformJsonDecoder CreateDecoder(string json, NodeId dataTypeId, Session session, int[] dimensions = null)
        {
            return new PlatformJsonDecoder(json, dataTypeId, session, dimensions);
        }
        
        #region Public Methods
        
        private List<object> ReadArray()
        {
            List<object> elements = new List<object>();

            while (m_reader.Read() && m_reader.TokenType != JsonToken.EndArray)
            {
                switch (m_reader.TokenType)
                {
                    case JsonToken.Comment:
                        {
                            break;
                        }

                    case JsonToken.Boolean:
                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.String:
                        {
                            elements.Add(m_reader.Value);
                            break;
                        }

                    case JsonToken.StartArray:
                        {
                            elements.Add(ReadArray());
                            break;
                        }

                    case JsonToken.StartObject:
                        {
                            elements.Add(ReadObject());
                            break;
                        }
                }
            }

            return elements;
        }

        private Dictionary<string, object> ReadObject()
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();

            while (m_reader.Read() && m_reader.TokenType != JsonToken.EndObject)
            {
                if (m_reader.TokenType == JsonToken.PropertyName)
                {
                    string name = (string)m_reader.Value;

                     if (m_reader.Read() && m_reader.TokenType != JsonToken.EndObject)
                    {
                        switch (m_reader.TokenType)
                        {
                            case JsonToken.Comment:
                                {
                                    break;
                                }

                            case JsonToken.Null:
                            case JsonToken.Date:
                                {
                                    fields[name] = m_reader.Value;
                                    break;
                                }

                            case JsonToken.Bytes:
                            case JsonToken.Boolean:
                            case JsonToken.Integer:
                            case JsonToken.Float:
                            case JsonToken.String:
                                {
                                    fields[name] = m_reader.Value;
                                    break;
                                }

                            case JsonToken.StartArray:
                                {
                                    fields[name] = ReadArray();
                                    break;
                                }

                            case JsonToken.StartObject:
                                {
                                    fields[name] = ReadObject();
                                    break;
                                }
                        }
                    }
                }
            }

            return fields;
        }
        
        #endregion

        #region IDecoder Members
        

        public new bool ReadField(string fieldName, out object token)
        {
            token = null;

            if (String.IsNullOrEmpty(fieldName))
            {
                token = m_stack.Peek();
                return true;
            }

            var context = m_stack.Peek() as Dictionary<string, object>;

            if (context == null || !context.TryGetValue(fieldName, out token))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reads a boolean from the stream.
        /// </summary>
        public new bool ReadBoolean(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as bool?;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a Boolean as expected");
            }

            return (bool)token;
        }

        /// <summary>
        /// Reads a sbyte from the stream.
        /// </summary>
        public new sbyte ReadSByte(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");
            }

            if (value < SByte.MinValue || value > SByte.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (sbyte)value;
        }

        /// <summary>
        /// Reads a byte from the stream.
        /// </summary>
        public new byte ReadByte(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a Integer as expected");
            }

            if (value < Byte.MinValue || value > Byte.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (byte)value;
        }

        /// <summary>
        /// Reads a short from the stream.
        /// </summary>
        public new short ReadInt16(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");
            }
;
            if (value < Int16.MinValue || value > Int16.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (short)value;
        }

        /// <summary>
        /// Reads a ushort from the stream.
        /// </summary>
        public new ushort ReadUInt16(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");
            }

            if (value < UInt16.MinValue || value > UInt16.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (ushort)value;
        }

        /// <summary>
        /// Reads an int from the stream.
        /// </summary>
        public new int ReadInt32(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");

            }

            if (value < Int32.MinValue || value > Int32.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (int)value;
        }

        /// <summary>
        /// Reads a uint from the stream.
        /// </summary>
        public new uint ReadUInt32(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");
            }

            if (value < UInt32.MinValue || value > UInt32.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (uint)value;
        }

        /// <summary>
        /// Reads a long from the stream.
        /// </summary>
        public new long ReadInt64(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");
            }

            return (long)value;
        }

        /// <summary>
        /// Reads a ulong from the stream.
        /// </summary>
        public new ulong ReadUInt64(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = long.TryParse(Convert.ToString(token), out var tmp) ? tmp : (long?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Integer as expected");
            }

            if (value < 0)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (ulong)value;
        }

        /// <summary>
        /// Reads a float from the stream.
        /// </summary>
        public new float ReadFloat(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = double.TryParse(Convert.ToString(token), out var tmp) ? tmp : (double?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a Number as expected");
            }

            if (value < Single.MinValue || value > Single.MaxValue)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return (float)value;
        }

        /// <summary>
        /// Reads a double from the stream.
        /// </summary>
        public new double ReadDouble(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = double.TryParse(Convert.ToString(token), out var tmp) ? tmp : (double?) null;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a Number as expected");
            }

            return (double)value;
        }

        /// <summary>
        /// Reads a string from the stream.
        /// </summary>
        public new string ReadString(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as string;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a String as expected");
            }

            if (m_context.MaxStringLength > 0 && m_context.MaxStringLength < value.Length)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return value;
        }

        /// <summary>
        /// Reads a UTC date/time from the stream.
        /// </summary>
        public new DateTime ReadDateTime(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as DateTime?;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a date string as expected");
            }

            return (DateTime)value;
        }

        /// <summary>
        /// Reads a GUID from the stream.
        /// </summary>
        public new Uuid ReadGuid(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as string;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a string as expected");
            }

            try
            {
                return new Uuid(value);
            }
            catch (FormatException exc)
            {
                throw new ValueToWriteTypeException("String not formatted correctly. " + exc.Message);
            }
        }

        /// <summary>
        /// Reads a byte string from the stream.
        /// </summary>
        public new byte[] ReadByteString(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as string;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a String as expected");
            }

            var bytes = Convert.FromBase64String(value);

            if (m_context.MaxByteStringLength > 0 && m_context.MaxByteStringLength < bytes.Length)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is out of range");
            }

            return bytes;
        }

        
        /// <summary>
        /// Reads an NodeId from the stream.
        /// </summary>
        public new NodeId ReadNodeId(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as string;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not a String as expected");
            }

            return PlatformUtils.ParsePlatformNodeIdString(value);
        }
        
        /// <summary>
        /// Reads an ExpandedNodeId from the stream.
        /// </summary>
        public new ExpandedNodeId ReadExpandedNodeId(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            string nodeId = null;
            string namespaceUri = null;
            uint serverIndex = 0;

            var value = token as Dictionary<string, object>;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Object as expected");
            }

            try
            {
                m_stack.Push(value);

                if (!value.ContainsKey("NodeId") || !value.ContainsKey("NamespaceUri") || !value.ContainsKey("ServerIndex"))
                {
                    throw new ValueToWriteTypeException("Error: Property named " + fieldName + " must have the properties NodeId, NamespaceUri and ServerIndex");
                }

                nodeId = ReadString("NodeId");
                namespaceUri = ReadString("NamespaceUri");
                serverIndex = ReadUInt32("ServerIndex");
               
            }
            finally
            {
                m_stack.Pop();
            }

            return new ExpandedNodeId(nodeId, namespaceUri, serverIndex);
            
        }

        /// <summary>
        /// Reads an StatusCode from the stream.
        /// </summary>
        public new StatusCode ReadStatusCode(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            string stringCode = null;
            uint uintCode = 0;
            
            
            var value = token as Dictionary<string, object>;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Object as expected");
            }

            try
            {
                m_stack.Push(value);

                if (!value.ContainsKey("Code") || !value.ContainsKey("StructureChanged"))
                {
                    throw new ValueToWriteTypeException("Error: Property named " + fieldName + " must have the properties Code and StructureChanged");
                }

                stringCode = ReadString("NodeId");
                // Get a PropertyInfo of specific property type(T).GetProperty(....)
                PropertyInfo propertyInfo = typeof(Opc.Ua.StatusCodes).GetProperty(stringCode, BindingFlags.Public | BindingFlags.Static); 
                if (propertyInfo == null)
                    throw new ValueToWriteTypeException("The string in code is not a valid value");
                object codeValue = propertyInfo.GetValue(null, null);

                uintCode = (uint) codeValue;
                
            }
            finally
            {
                m_stack.Pop();
            }
            
            return new StatusCode(uintCode);

        }

        /// <summary>
        /// Reads an DiagnosticInfo from the stream.
        /// </summary>
        public new DiagnosticInfo ReadDiagnosticInfo(string fieldName)
        {
            object token = (object) null;
            if (!ReadField(fieldName, out token))
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            Dictionary<string, object> dictionary = token as Dictionary<string, object>;
            if (dictionary == null)
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an object as expected");
            try
            {
                m_stack.Push(dictionary);
                
                if (!dictionary.ContainsKey("SymbolicId") || !dictionary.ContainsKey("NamespaceUri") || 
                    !dictionary.ContainsKey("Locale") || !dictionary.ContainsKey("LocalizedText") || 
                    !dictionary.ContainsKey("AdditionalInfo") || !dictionary.ContainsKey("InnerStatusCode") || 
                    !dictionary.ContainsKey("InnerDiagnosticInfo"))
                {
                    throw new ValueToWriteTypeException("Error: Property named " + fieldName + " must have the properties SymbolicId, NamespaceUri, Locale, LocalizedText, AdditionaInfo," +
                                                        "InnerStatusCode and InnerDiagnosticInfo ");
                }
                
                var diagnosticInfo = new DiagnosticInfo();
                diagnosticInfo.SymbolicId = ReadInt32("SymbolicId");
                diagnosticInfo.NamespaceUri = ReadInt32("NamespaceUri");
                diagnosticInfo.Locale = ReadInt32("Locale");
                diagnosticInfo.LocalizedText = ReadInt32("LocalizedText");
                diagnosticInfo.AdditionalInfo = ReadString("AdditionalInfo");
                diagnosticInfo.InnerStatusCode = ReadStatusCode("InnerStatusCode");
                diagnosticInfo.InnerDiagnosticInfo = ReadDiagnosticInfo("InnerDiagnosticInfo");
                return diagnosticInfo;
            }
            finally
            {
                m_stack.Pop();
            }
        }

        /// <summary>
        /// Reads an QualifiedName from the stream.
        /// </summary>
        public new QualifiedName ReadQualifiedName(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as Dictionary<string, object>;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Object as expected");
            }
            
            string name = null;
            ushort namespaceIndex = 0;
            
            try
            {
                m_stack.Push(value);

                if (!value.ContainsKey("Name") || !value.ContainsKey("NamespaceIndex"))
                {
                    throw new ValueToWriteTypeException("Error: Property named " + fieldName + " must have the properties Name and NamespaceIndex");
                }

                name = ReadString("Name");
                namespaceIndex = ReadUInt16("NamespaceIndex");
               
            }
            finally
            {
                m_stack.Pop();
            }

            return new QualifiedName(name, namespaceIndex);
        }

        /// <summary>
        /// Reads an LocalizedText from the stream.
        /// </summary>
        public new LocalizedText ReadLocalizedText(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as Dictionary<string, object>;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Object as expected");
            }
            
            string locale = null;
            string text = null;
            
            try
            {
                m_stack.Push(value);

                if (!value.ContainsKey("Locale") || !value.ContainsKey("Text"))
                {
                    throw new ValueToWriteTypeException("Error: Property named " + fieldName + " must have the properties Locale, and Text");
                }

                locale = ReadString("Locale");
                text = ReadString("Text");
               
            }
            finally
            {
                m_stack.Pop();
            }

            return new LocalizedText(locale, text);
        }
        
        /// <summary>
        /// Reads an Enumeration from the stream.
        /// </summary>
        public int ReadEnumeration(string fieldName)
        {
            object token = null;

            if (!ReadField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            var value = token as Dictionary<string, object>;

            if (value == null)
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " is not an Object as expected");
            }
            
            int enumIndex;
            string enumValue = null;
            
            try
            {
                m_stack.Push(value);

                if (!value.ContainsKey("EnumIndex") || !value.ContainsKey("EnumValue"))
                {
                    throw new ValueToWriteTypeException("Error: Property named " + fieldName + " must have the properties EnumIndex, and EnumValues");
                }

                enumIndex = ReadInt32("EnumIndex");
                enumValue = ReadString("EnumValue");
               
            }
            finally
            {
                m_stack.Pop();
            }

            //check if the index match the enumValue
            var enumTuple = m_session.GetEnumStrings(m_currentDataType);

            if (enumTuple.enumStrings != null && (enumIndex > enumTuple.enumStrings.Length || enumIndex < 0 || enumTuple.enumStrings[enumIndex] != enumValue))
                throw new ValueToWriteTypeException("Wrong corrispondence between EnumIndex and EnumValue");

            if (enumTuple.enumValues != null)
            {
                var enVal = enumTuple.enumValues.SingleOrDefault(s => s.Value == enumIndex);
                if (enVal == null || enVal.DisplayName.Text != enumValue)
                    throw new ValueToWriteTypeException("Wrong corrispondence between EnumIndex and EnumValue");
            }
            
            return enumIndex;
        }
        
        /// <summary>
        /// Reads an extension object from the stream.
        /// </summary>
        public new ExtensionObject ReadExtensionObject(string fieldName)
        {
            var jObject = m_currentJObject[fieldName].ToObject<JObject>();
            
            if (m_currentDataType.NamespaceIndex != 0)
            {
                var analyzer = new DataTypeAnalyzer(m_session);
                var encodingNodeId = analyzer.GetDataTypeEncodingNodeId(m_currentDataType);
                var descriptionNodeId = analyzer.GetDataTypeDescriptionNodeId(encodingNodeId);
                var dictionary = analyzer.GetDictionary(descriptionNodeId);
                var descriptionId = m_session.ReadNodeAttribute(descriptionNodeId, Attributes.Value)[0].Value.ToString();

                var structuredEncoder = new StructuredEncoder(dictionary);
                var value = structuredEncoder.BuildExtensionObjectFromJsonObject(descriptionId, jObject, m_session.MessageContext, encodingNodeId);

                return value;
            }

            var systemType = TypeInfo.GetSystemType(m_currentDataType, m_session.Factory);
            var jsonDecoder = CreateDecoder(jObject.ToString(), m_currentDataType, m_session);

            if (!(Activator.CreateInstance(systemType) is IEncodeable valueToWrite))
            {
                throw new ValueToWriteTypeException("Found a Standard Structured DataType not IEncodable");
            }
            
            valueToWrite.Decode(jsonDecoder);
            return new ExtensionObject(m_currentDataType, valueToWrite);
        }
        
        /// <summary>
        /// Reads an extension object from the stream.
        /// </summary>
        public new ExtensionObjectCollection ReadExtensionObjectArray(string fieldName)
        {
            var extensionObjectCollection = new ExtensionObjectCollection();
            var jArray = m_currentJObject[fieldName].ToObject<JArray>();

            if (m_currentDataType.NamespaceIndex != 0)
            {
                var analyzer = new DataTypeAnalyzer(m_session);
                var encodingNodeId = analyzer.GetDataTypeEncodingNodeId(m_currentDataType);
                var descriptionNodeId = analyzer.GetDataTypeDescriptionNodeId(encodingNodeId);
                string dictionary = analyzer.GetDictionary(descriptionNodeId);
                string descriptionId = m_session.ReadNodeAttribute(descriptionNodeId, Attributes.Value)[0].Value.ToString();
                StructuredEncoder structuredEncoder = new StructuredEncoder(dictionary);
                foreach (var jObj in jArray)
                {
                   extensionObjectCollection.Add(structuredEncoder.BuildExtensionObjectFromJsonObject(descriptionId, jObj.ToObject<JObject>(),
                        m_session.MessageContext, encodingNodeId));
                }

                return extensionObjectCollection;
            }
            
            var systemType = TypeInfo.GetSystemType(m_currentDataType, m_session.Factory);
            foreach (var jObj in jArray)
            {
                var jsonDecoder = CreateDecoder(jObj.ToObject<JObject>().ToString(), m_currentDataType, m_session);
                if (!(Activator.CreateInstance(systemType) is IEncodeable valueToWrite))
                {
                    throw new ValueToWriteTypeException("Found a Standard Structured DataType not IEncodable");
                }
        
                valueToWrite.Decode(jsonDecoder);
                extensionObjectCollection.Add(new ExtensionObject(m_currentDataType, valueToWrite));
            }

            return extensionObjectCollection;
        }
        
        
        
        
        
        

        private bool ReadArrayField(string fieldName, out List<object> array)
        {
            object token = array = null;

            if (!ReadField(fieldName, out token))
            {
                return false;
            }
            
            var value = token as List<object>;
            
            array = token as List<object>;

            if (array == null)
            {
                return false;
            }

            if (m_context.MaxArrayLength > 0 && m_context.MaxArrayLength < array.Count)
            {
                throw new ServiceResultException(Opc.Ua.StatusCodes.BadEncodingLimitsExceeded);
            }

            return true;
        }

        /// <summary>
        /// Reads a boolean array from the stream.
        /// </summary>
        public new BooleanCollection ReadBooleanArray(string fieldName)
        {
            var values = new BooleanCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadBoolean(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a sbyte array from the stream.
        /// </summary>
        public new SByteCollection ReadSByteArray(string fieldName)
        {
            var values = new SByteCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadSByte(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a byte array from the stream.
        /// </summary>
        public new ByteCollection ReadByteArray(string fieldName)
        {
            var values = new ByteCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadByte(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a short array from the stream.
        /// </summary>
        public new Int16Collection ReadInt16Array(string fieldName)
        {
            var values = new Int16Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadInt16(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a ushort array from the stream.
        /// </summary>
        public new UInt16Collection ReadUInt16Array(string fieldName)
        {
            var values = new UInt16Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadUInt16(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a int array from the stream.
        /// </summary>
        public new Int32Collection ReadInt32Array(string fieldName)
        {
            var values = new Int32Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadInt32(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a uint array from the stream.
        /// </summary>
        public new UInt32Collection ReadUInt32Array(string fieldName)
        {
            var values = new UInt32Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadUInt32(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a long array from the stream.
        /// </summary>
        public new Int64Collection ReadInt64Array(string fieldName)
        {
            var values = new Int64Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadInt64(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a ulong array from the stream.
        /// </summary>
        public new UInt64Collection ReadUInt64Array(string fieldName)
        {
            var values = new UInt64Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadUInt64(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a float array from the stream.
        /// </summary>
        public new FloatCollection ReadFloatArray(string fieldName)
        {
            var values = new FloatCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadFloat(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a double array from the stream.
        /// </summary>
        public new DoubleCollection ReadDoubleArray(string fieldName)
        {
            var values = new DoubleCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadDouble(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a string array from the stream.
        /// </summary>
        public new StringCollection ReadStringArray(string fieldName)
        {
            var values = new StringCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadString(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a UTC date/time array from the stream.
        /// </summary>
        public new DateTimeCollection ReadDateTimeArray(string fieldName)
        {
            var values = new DateTimeCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    values.Add(ReadDateTime(null));
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a GUID array from the stream.
        /// </summary>
        public new UuidCollection ReadGuidArray(string fieldName)
        {
            var values = new UuidCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadGuid(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads a byte string array from the stream.
        /// </summary>
        public new ByteStringCollection ReadByteStringArray(string fieldName)
        {
            var values = new ByteStringCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadByteString(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an XmlElement array from the stream.
        /// </summary>
        public new XmlElementCollection ReadXmlElementArray(string fieldName)
        {
            var values = new XmlElementCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadXmlElement(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an NodeId array from the stream.
        /// </summary>
        public new NodeIdCollection ReadNodeIdArray(string fieldName)
        {
            var values = new NodeIdCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadNodeId(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an ExpandedNodeId array from the stream.
        /// </summary>
        public new ExpandedNodeIdCollection ReadExpandedNodeIdArray(string fieldName)
        {
            var values = new ExpandedNodeIdCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadExpandedNodeId(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an StatusCode array from the stream.
        /// </summary>
        public new StatusCodeCollection ReadStatusCodeArray(string fieldName)
        {
            var values = new StatusCodeCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadStatusCode(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an DiagnosticInfo array from the stream.
        /// </summary>
        public new DiagnosticInfoCollection ReadDiagnosticInfoArray(string fieldName)
        {
            var values = new DiagnosticInfoCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadDiagnosticInfo(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an QualifiedName array from the stream.
        /// </summary>
        public new QualifiedNameCollection ReadQualifiedNameArray(string fieldName)
        {
            var values = new QualifiedNameCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadQualifiedName(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        /// <summary>
        /// Reads an LocalizedText array from the stream.
        /// </summary>
        public new LocalizedTextCollection ReadLocalizedTextArray(string fieldName)
        {
            var values = new LocalizedTextCollection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            for (int ii = 0; ii < token.Count; ii++)
            {
                try
                {
                    m_stack.Push(token[ii]);
                    var element = ReadLocalizedText(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }
        
        public Int32Collection ReadEnumerationArray(string fieldName)
        {
            var values = new Int32Collection();

            List<object> token = null;

            if (!ReadArrayField(fieldName, out token))
            {
                throw new ValueToWriteTypeException("Error: Property named " + fieldName + " missing");
            }

            foreach (var t in token)
            {
                try
                {
                    m_stack.Push(t);
                    var element = ReadEnumeration(null);
                    values.Add(element);
                }
                finally
                {
                    m_stack.Pop();
                }
            }

            return values;
        }

        
        #endregion

        #region Private Methods
        
        
        #endregion
    }
}