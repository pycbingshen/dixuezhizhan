using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace client
{
    class RoundRobinQueue
    {
        const int QUEUELENGTH = 100000;
        CMessage[] m_que = new CMessage[QUEUELENGTH];
        int m_head, m_tail;
        public int m_lastframe;
		public RoundRobinQueue()
        {
			m_head = m_tail = 0;
            m_lastframe = -1;
        }

        public void init()
        {
            m_head = m_tail = 0;
            m_lastframe = -1;
        }

		public bool empty()
        {
			if(m_head == m_tail) return true;
			return false;
        }

 		public CMessage front()
        {
			if(m_tail == m_head) return null;
			return m_que[m_head];
        }

        public void pop()
        {
            if (m_tail == m_head) return;
            m_head = (m_head + 1) % QUEUELENGTH;
        }

		public void push(CMessage x)
        {
			System.Diagnostics.Debug.Assert((m_tail+1)%QUEUELENGTH != m_head);
			m_que[m_tail] = x;
            m_tail = (m_tail + 1) % QUEUELENGTH;
			m_lastframe = Math.Max(m_lastframe, x.m_head.m_framenum);
        }

        public int GetLastFrame()
        {
            if (m_tail == m_head) return -1;
            return m_lastframe;
        }
    }
}
