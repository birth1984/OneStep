using System.Xml;
using System.Collections.Generic;

namespace IGG.CCTwo.Data
{
    /*
     * Helper functions for reading XML elements
     * 
     * 
     */
    public class XmlHelper
    {
        delegate bool TryParser<T>(string pStr, out T pValue);

        // delegate caching
        static TryParser<int> int_TryParse = int.TryParse;
        static TryParser<float> float_TryParse = float.TryParse;
        static TryParser<bool> bool_TryParse = bool.TryParse;
        static TryParser<uint> uint_TryParse = uint.TryParse;
      

        /// <summary>
        /// Tries to read an attribute using a custom TryParse.  If value was not found or parsing failed, pValue = pDefaultValue
        /// </summary>
        /// <returns><c>true</c>, if value was found and parsed <c>false</c> otherwise.</returns>
        static T ReadAttribute<T>(XmlElement pElem, string pName, T pDefaultValue, TryParser<T> pTryParseFunc, System.Action<string> pNotFoundErrorLogger = null)
        {
            T returnValue;
            string attributeValueString = pElem.GetAttribute(pName);
            if (string.IsNullOrEmpty(attributeValueString))
            {
                if (pNotFoundErrorLogger != null)
                {
                    pNotFoundErrorLogger(MakeErrorMessage<T>(pElem, pName, "read"));
                }
                return pDefaultValue;
            }
            if (!pTryParseFunc(attributeValueString, out returnValue))
            {                
              //  IGG.Debug.Fail(MakeErrorMessage<T>(pElem, pName, "parse"));
                return pDefaultValue;
            }
            return returnValue;
        }

        [System.Diagnostics.Conditional("IGG_DEBUG")]
        [System.Diagnostics.Conditional("IGG_RELEASE")]
        static void DebugCheckHasAttribute<T>(XmlElement pElem, string pAttributeName)
        {
            //IGG.Debug.Assert(pElem.HasAttribute(pAttributeName), MakeErrorMessage<T>(pElem, pAttributeName, "find"));
        }

        static string MakeErrorMessage<T>(XmlElement pElem, string pAttributeName, string pOperation)
        {
            return string.Format("Can't {0} {1} attribute {2} at element {3}.", pOperation, typeof(T).Name, pAttributeName, pElem.Name);
        }

        

        // Reads a float attribute from an Element, given the name of the attribute.
        // Fails noisily if the attribute can't be read.
        public static float ReadFloatAttribute(XmlElement pElem, string pName)
        {
            return ReadAttribute(pElem, pName, 0f, float_TryParse/*, x => IGG.Debug.Log(x)*/);
        }

        // Reads a float attribute from an Element, given the name of the attribute.
        // If the attribute can't be read, it returns the default value.
        public static float ReadFloatAttribute(XmlElement pElem, string pName, float pDefault)
        {
            return ReadAttribute(pElem, pName, pDefault, float_TryParse);
        }

        // Reads a integer attribute from an Element, given the name of the attribute.
        // Fails noisily if the attribute can't be read.
        public static uint ReadUintegerAttribute(XmlElement pElem, string pName, uint pDefault)
        {
            return ReadAttribute(pElem, pName, pDefault, uint_TryParse);
        }

        // Reads a bool attribute from an Element, given the name of the attribute.
        // Fails noisily if the attribute can't be read.
        public static bool ReadBooleanAttribute(XmlElement pElem, string pName)
        {
            return ReadAttribute(pElem, pName, false, bool_TryParse/*, x => IGG.Debug.Log(x)*/);
        }

        // Reads a bool attribute from an Element, given the name of the attribute.
        // If the attribute can't be read, it returns the default value.
        public static bool ReadBooleanAttribute(XmlElement pElem, string pName, bool pDefault)
        {
            return ReadAttribute(pElem, pName, pDefault, bool_TryParse);
        }

        // Reads a integer attribute from an Element, given the name of the attribute.
        // Fails noisily if the attribute can't be read.
        public static int ReadIntegerAttribute(XmlElement pElem, string pName)
        {
            return ReadAttribute(pElem, pName, -1, int_TryParse/*, x => IGG.Debug.Log(x)*/);
        }
        // Reads a integer attribute from an Element, given the name of the attribute.
        // If the attribute can't be read, it returns the default value.
        public static int ReadIntegerAttribute(XmlElement pElem, string pName, int pDefault)
        {
            return ReadAttribute(pElem, pName, pDefault, int_TryParse);
        }

        // Reads a integer array attribute from an Element, given the name of the attribute.
        // The array should be a list of integers separated by comma, e.g. "1,2,3,4,5,6,7".
        // If an integer can't be read, it returns the default value.
        public static int[] ReadIntegerArrayAttribute(XmlElement pElem, string pName, string splitStr=",")
        {
            if (!pElem.HasAttribute(pName))
            {
                return null;
            }
            string attrStr = pElem.GetAttribute(pName);

            if (string.IsNullOrEmpty(attrStr))
                return null;

            string[] split = attrStr.Split(splitStr.ToCharArray());
            int len = split.Length;

            if (len > 1 && string.IsNullOrEmpty(split[len - 1]))  //避免策划尾巴多加;或,
                len--;

            int[] result = new int[len];
            for (int i = 0; i < split.Length; i++)
            {
                int bType = int.MinValue;
                if (!int.TryParse(split[i].Trim(), out bType))
                {
                    //IGG.Debug.Warn(false, "Cant parse int array attribute " + pName + " at element " + pElem.Name);
                }
                result[i] = bType;
            }

            return result;
        }

        // Reads a integer 2D array attribute from an Element, given the name of the attribute.
        // The array should be a list of integers separated by comma, and semicolon, e.g. "1,2,3;4,5,6;7,8,9;10,11,12;13,14,15".
        // You need to inform the Dimension for the array. In the example aforementioned, the dimension is 3.
        // If an integer can't be read, it returns the default value.
        public static int[,] ReadInteger2DArrayAttribute(XmlElement pElem, string pName, int pDimSize=2)
        {
            if (!pElem.HasAttribute(pName))
            {
                return null;
            }
            string attrStr = pElem.GetAttribute(pName);

            //为处理策划多加了; 需要做个检测
            if (attrStr.Length>1 && (attrStr.LastIndexOf(";") == (attrStr.Length - 1)))
            {
                attrStr = attrStr.Substring(0, attrStr.Length - 1);                
            }

            string[] split1 = attrStr.Split(";".ToCharArray());
            

            int[,] result = new int[split1.Length, pDimSize];
            for (int i = 0; i < split1.Length; i++)
            {
                if(string.IsNullOrEmpty(split1[i]))
                    continue;                
                string[] split2 = split1[i].Trim().Split(",".ToCharArray());

                //IGG.Debug.Assert(split2.Length <= pDimSize, "Informed Dimension size " + pDimSize + " does not match the one at attribute " + pName);

                for (int j = 0; j < split2.Length; j++)
                {
                    int readInt = int.MinValue;
                    if (!int.TryParse(split2[j].Trim(), out readInt))
                    {
                        //IGG.Debug.Warn(false,"Can't parse int array 2D attribute " + pName + " at element " + pElem.Name + "  " + split2[j].Trim());
                    }
                    result[i, j] = readInt;
                }
            }

            return result;
        }

        // Reads a float array attribute from an Element, given the name of the attribute.
        // The array should be a list of float separated by comma, e.g. "1.0,2,3.99,4,5.2344,6,7".
        // If an float can't be read, it returns the default value.
        public static float[] ReadFloatArrayAttribute(XmlElement pElem, string pName)
        {
            if (!pElem.HasAttribute(pName))
            {
                return null;
            }
            string attrStr = pElem.GetAttribute(pName);

            if (string.IsNullOrEmpty(attrStr))
                return null;

            string[] split = attrStr.Split(",".ToCharArray());
            float[] result = new float[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                float bType = float.MinValue;
                if (!float.TryParse(split[i].Trim(), out bType))
                {
                    //IGG.Debug.Warn(false,"Cant parse float array attribute " + pName + " at element " + pElem.Name);
                }
                result[i] = bType;
            }

            return result;
        }

        // Reads a string attribute from an Element, given the name of the attribute.
        // It fails if the attribute can't be read.
        public static string ReadStringAttribute(XmlElement pElem, string pName)
        {
            //IGG.Debug.Warn(pElem.HasAttribute(pName), "Attribute " + pName + " not found in element  " + pElem.Name);
            
            return pElem.GetAttribute(pName).Trim();
        }

        // Reads a string attribute from an Element, given the name of the attribute.
        // If the attribute can't be read, it returns the default value.
        public static string ReadStringAttribute(XmlElement pElem, string pName, string pDefault)
        {
            if (!pElem.HasAttribute(pName))
            {
                return pDefault;
            }
            return pElem.GetAttribute(pName).Trim();
        }

    }

   
}


