using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace client
{
    class MessageRegister
    {
        private MessageRegister()
        {
            m_IntToType_dic = new Dictionary<int, Type>();
            m_TypeToInt_dic = new Dictionary<Type, int>();
        }

        private static MessageRegister Singleton = null;
        public static MessageRegister Instance()
        {
            if (Singleton == null)
            {
                Singleton = new MessageRegister();
            }
            return Singleton;
        }

        public bool Register(int TypeID, Type type)
        {
            Type tmptype;
            if (m_IntToType_dic.TryGetValue(TypeID, out tmptype) == true) return false;
            m_IntToType_dic.Add(TypeID, type);
            int tmpid;
            if (m_TypeToInt_dic.TryGetValue(type, out tmpid) == true) return false;
            m_TypeToInt_dic.Add(type, TypeID);
            return true;
        }

        public Type GetType(int TypeID)
        {
            Type tmptype;
            if (m_IntToType_dic.TryGetValue(TypeID, out tmptype) == true) return tmptype;
            return null;
        }

        public int GetID(Type type)
        {
            int tmpid;
            if (m_TypeToInt_dic.TryGetValue(type, out tmpid) == true) return tmpid;
            return -1;
        }

        private Dictionary<int, Type> m_IntToType_dic;
        private Dictionary<Type, int> m_TypeToInt_dic;
    }
}
