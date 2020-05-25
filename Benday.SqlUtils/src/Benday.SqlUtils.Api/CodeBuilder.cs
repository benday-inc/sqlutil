using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.Api
{
	public class CodeBuilder
	{
		private StringBuilder m_code = new StringBuilder();
		private int m_currentIndentCount = 0;
		private string m_currentTabString = "";
		private bool m_tabsWrittenForCurrentLine = false;

		private const string CRLF = "\r\n";

		public CodeBuilder()
		{
			m_code = new StringBuilder();
		}

		public void IncreaseIndent()
		{
			m_currentIndentCount++;
			PopulateTabString();
		}

		public void DecreaseIndent()
		{
			if (m_currentIndentCount > 0)
			{
				m_currentIndentCount--;
				PopulateTabString();
			}
			else
			{
				m_currentIndentCount = 0;
				m_currentTabString = "";
			}
		}

		private void PopulateTabString()
		{
			if (m_currentIndentCount > 0)
			{
				StringBuilder temp = new StringBuilder();

				for (int index = 0; index < m_currentIndentCount; index++)
				{
					temp.Append("\t");
				}

				m_currentTabString = temp.ToString();
			}
			else
			{
				m_currentTabString = "";
			}
		}

		public void AppendLine(string p_appendThis)
		{
			Append(p_appendThis);
			NewLine();
		}

		public void Append(string p_appendThis)
		{
			if (m_tabsWrittenForCurrentLine == false)
			{
				m_code.Append(m_currentTabString);
				m_tabsWrittenForCurrentLine = true;
			}

			m_code.Append(p_appendThis);
		}

		public void NewLine()
		{
			m_tabsWrittenForCurrentLine = false;

			m_code.Append(CRLF);


		}

		public override string ToString()
		{
			return m_code.ToString();
		}

	}
}
