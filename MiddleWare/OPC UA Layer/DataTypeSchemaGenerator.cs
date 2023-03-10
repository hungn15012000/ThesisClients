using Newtonsoft.Json.Schema;
using Opc.Ua;

namespace MiddleWare.OPCUALayer
{
    public static class DataTypeSchemaGenerator
    {
        public static JSchema GenerateSchemaForArray(int[] dimensions, JSchema innermostSchema)
        {
            var dimLen = dimensions.Length;

            if (dimLen == 1)
            {
                var schema = new JSchema
                {
                    Type = JSchemaType.Array,
                    Items = { innermostSchema },
                    MinimumItems = dimensions[0],
                    MaximumItems = dimensions[0] 
                };
                
                return schema;
            } 
            
            JSchema innerSchema = new JSchema();
            JSchema outerSchema = new JSchema();
            for (int dim = dimLen-1; dim >= 0; dim--)
            {
                if (dim == dimLen-1)
                {
                    innerSchema = new JSchema
                    {
                        Type = JSchemaType.Array,
                        Items = { innermostSchema },
                        MinimumItems = dimensions[dim],
                        MaximumItems = dimensions[dim]
                    };
                }
                else
                {
                    outerSchema = new JSchema
                    {
                        Type = JSchemaType.Array,
                        Items = { innerSchema },
                        MinimumItems = dimensions[dim],
                        MaximumItems = dimensions[dim]
                    };
                    innerSchema = outerSchema;
                }
            }

            return outerSchema;
        }

        /// <summary>
        /// Used for generating schema based on standard types used in DataTypeDictionary.
        /// They are defined in Part 3 - Table C.9
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static JSchema GenerateSchemaForStandardTypeDescription(BuiltInType type)
        {
            switch (type)
            {
                    case BuiltInType.Boolean:
                        return new JSchema { Type = JSchemaType.Boolean };
                    case BuiltInType.SByte: case BuiltInType.Byte:
                    case BuiltInType.Int16: case BuiltInType.UInt16:
                    case BuiltInType.Int32: case BuiltInType.UInt32:
                    case BuiltInType.Int64: case BuiltInType.UInt64:
                    case BuiltInType.Float: case BuiltInType.Double:
                        return new JSchema { Type = JSchemaType.Number };
                    case BuiltInType.String: case BuiltInType.DateTime: 
                    case BuiltInType.Guid:   case BuiltInType.DiagnosticInfo:
                    case BuiltInType.NodeId: case BuiltInType.ExpandedNodeId:
                    case BuiltInType.XmlElement: case BuiltInType.ByteString:
                        return new JSchema { Type = JSchemaType.String };
                    case BuiltInType.LocalizedText:
                        return new JSchema()
                        {
                            Type = JSchemaType.Object,
                            Properties = {
                                { "Locale", new JSchema { Type = JSchemaType.String } },
                                { "Text", new JSchema { Type = JSchemaType.String } }
                            }
                        };;
                    case BuiltInType.StatusCode:
                        return new JSchema
                        {
                            Type = JSchemaType.Object,
                            Properties =
                            {
                                { "code", new JSchema
                                    { 
                                        Type = JSchemaType.String, 
                                        Enum = { "Good", "Uncertain", "Bad" } 
                                    } 
                                },
                                { "structureChanged", new JSchema{ Type = JSchemaType.Boolean } }
                            }
                        };;
                    case BuiltInType.QualifiedName:
                        return new JSchema
                        {
                            Type = JSchemaType.Object,
                            Properties =
                            {
                                { "NamespaceIndex", new JSchema{ Type = JSchemaType.Integer} },
                                { "Name", new JSchema{ Type = JSchemaType.String } }
                            }
                        };
                    case BuiltInType.Enumeration:
                        return new JSchema
                        {
                            Type = JSchemaType.Object,
                            Properties =
                            {
                                { "EnumValue", new JSchema { Type = JSchemaType.Integer } },
                                { "EnumLabel", new JSchema { Type = JSchemaType.String } }
                            }
                        };;
                    default:
                        return new JSchema();
            }
        }
    }
}